using MVCBlog.Common.OAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Common.OAuth
{
    public abstract class OpenAuthenticationBase
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CallbackUrl { get; set; }
        public string AccessToken { get; set; }
        public string UID { get; set; }
        public OAuthSystemType oAuthSystemType { get; set; }
        protected bool isAccessTokenSet = false;

        public bool IsAuthorized { get { return isAccessTokenSet && !string.IsNullOrEmpty(AccessToken); } }

        public OpenAuthenticationBase(string clientId, string clientSecrte, string callbackUrl, string accessToken = null)
        {
            this.ClientId = clientId;
            this.ClientSecret = clientSecrte;
            this.CallbackUrl = callbackUrl;
            if (!string.IsNullOrEmpty(accessToken))
            {
                this.AccessToken = accessToken;
                this.isAccessTokenSet = true;
            }
        }

        protected abstract string AuthorizationCodeUrl { get; }
        protected abstract string AccessTokenUrl { get; }
        public abstract string GetAuthorizationUrl();

        public abstract OAuthUserInfo GetOAuthUserInfo();

        public abstract void GetAccessTokenByCode(string code);
    }
}
