using MVCBlog.Common.OAuth.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MVCBlog.Common.OAuth
{
    public class QQOpenAuthentication : OpenAuthenticationBase
    {
        private const string AUTH_URL = "https://graph.qq.com/oauth2.0/authorize";
        private const string TOKEN_URL = "https://graph.qq.com/oauth2.0/token";
        private const string API_URL = "https://graph.qq.com/oauth2.0/me";
        private int STATE { get { return 1; } }


        public QQOpenAuthentication(string appKey, string appSecret, string callbackUrl, string accessToken = null, string uid = null)
            : base(appKey, appSecret, callbackUrl, accessToken)
        {
            oAuthSystemType = OAuthSystemType.QQ;
            UID = uid;
            if (!string.IsNullOrEmpty(accessToken) && string.IsNullOrEmpty(uid))
            {
                isAccessTokenSet = true;
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
            ub.Query = string.Format("response_type=code&client_id={0}&redirect_uri={1}&state={2}", ClientId, CallbackUrl, STATE);
            return ub.ToString();
        }

        public override void GetAccessTokenByCode(string code)
        {
            var ub = new UriBuilder(TOKEN_URL);
            ub.Query = string.Format("grant_type=authorization_code&client_id={0}&client_secret={1}&code={2}&redirect_uri={3}", ClientId, ClientSecret, code, CallbackUrl);

            HttpWebResponse response = HttpHelper.CreateGetHttpResponse(ub.ToString());
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return;
            }
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string content = reader.ReadToEndAsync().Result;

                var accessToken = string.Empty;

                var pattern = @"access_token=(([\d|a-zA-Z]*))";

                if (Regex.IsMatch(content, pattern))
                {
                    accessToken = Regex.Match(content, pattern).Groups[1].Value;
                }
                ub = new UriBuilder(API_URL);
                ub.Query = string.Format("access_token={0}", accessToken);
                string GetOpenApiUrl = ub.ToString();
                response = response = HttpHelper.CreateGetHttpResponse(GetOpenApiUrl);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    return;

                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                pattern = @"\""openid\"":\""([\d|a-zA-Z]+)\""";

                if (!Regex.IsMatch(result, pattern))
                {
                    return;
                }

                AccessToken = accessToken;
                UID = Regex.Match(result, pattern).Groups[1].Value;
                isAccessTokenSet = true;

            }
        }

        public override OAuthUserInfo GetOAuthUserInfo()
        {
            if (!IsAuthorized)
            {
                return null;
            }
            var ub = new UriBuilder("https://graph.qq.com/user/get_user_info");

            ub.Query = string.Format("access_token={0}&oauth_consumer_key={1}&openid={2}", AccessToken, ClientId, UID);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("access_token", AccessToken);
            parameters.Add("oauth_consumer_key", ClientId);
            parameters.Add("openid", UID);
            HttpWebResponse response = HttpHelper.CreatePostHttpResponse("https://graph.qq.com/user/get_user_info", parameters);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string content = reader.ReadToEndAsync().Result;
                QQUserInfo qqUserinfo = JsonConvert.DeserializeObject<QQUserInfo>(content);
                return new OAuthUserInfo()
                {
                    AccessToken = AccessToken,
                    Uid = UID,
                    SystemType = OAuthSystemType.QQ,
                    Name = qqUserinfo.nickname,
                    ProfileImgUrl = qqUserinfo.figureurl_qq_2
                };
            } 
        }
    }
}
