using MVCBlog.Common.OAuth.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Common.OAuth
{
    public class WeiBoOpenAuthentication : OpenAuthenticationBase
    {
        private const string AUTH_URL = "https://api.weibo.com/oauth2/authorize";
        private const string TOKEN_URL = "https://api.weibo.com/oauth2/access_token";
        private const string API_URL = "https://api.weibo.com/2/";
        private const string USER_INFO_API = "users/show.json";


        private static CookieCollection cookies { get { return new CookieCollection(); } }
        public string UID { get; set; }

        public WeiBoOpenAuthentication(string appKey, string appSecret, string callbackUrl, string accessToken = null, string uid = null)
            : base(appKey, appSecret, callbackUrl, accessToken)
        {
            ClientName = "SinaWeibo";
            UID = uid;
            if (!string.IsNullOrEmpty(accessToken) && string.IsNullOrEmpty(uid))
            {
                isAccessTokenSet = true;
            }
        }

        public SinaUserInfo GetUserInfo(string accesstoken, string uid)
        {
            var ub = new UriBuilder(API_URL + USER_INFO_API);
            if (string.IsNullOrEmpty(accesstoken) || string.IsNullOrEmpty(uid))
            {
                throw new ArgumentNullException("存在空参数");
            }
            ub.Query = string.Format("uid={0}&access_token={1}", uid, accesstoken);
            string url = ub.ToString();
            HttpWebResponse response = HttpHelper.CreateGetHttpResponse(url);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string content = reader.ReadToEndAsync().Result;
                var model = JsonConvert.DeserializeObject<SinaUserInfo>(content);
                if (model != null)
                {
                    model.access_token = accesstoken;
                    model.uid = uid;
                }
                return model;
            }
        }
        protected override string AuthorizationCodeUrl
        {
            get { return AUTH_URL; }
        }

        protected override string AccessTokenUrl
        {
            get { return TOKEN_URL; }
        }


        public override string GetAuthorizationUrl()
        {
            var ub = new UriBuilder(AuthorizationCodeUrl);
            ub.Query = string.Format("client_id={0}&response_type=code&redirect_uri={1}", ClientId, Uri.EscapeDataString(CallbackUrl));
            return ub.ToString();
        }

        public override void GetAccessTokenByCode(string code)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("client_id", ClientId);
            parameters.Add("client_secret", ClientSecret);
            parameters.Add("grant_type", "authorization_code");
            parameters.Add("code", code);
            parameters.Add("redirect_uri", CallbackUrl);
            HttpWebResponse response = HttpHelper.CreatePostHttpResponse(TOKEN_URL, parameters);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return;
            }
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string content = reader.ReadToEndAsync().Result;
                var result = JObject.Parse(content);
                if (result["access_token"] == null)
                {
                    return;
                }
                AccessToken = result.Value<string>("access_token");
                UID = result.Value<string>("uid");
                isAccessTokenSet = true;
            }

        }
    }
}

