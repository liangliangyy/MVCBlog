using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCBlog.Common.OAuth;
using System.Web.Mvc;
using System.Configuration;
using System.Web;
using MVCBlog.Common;
using MVCBlog.Common.OAuth.Models;
using MVCBlog.Service.Interfaces;
using MVCBlog.Service;

namespace MVCBlog.Web.CommonHelper
{
    public class OAuthClientFactory
    {
        private static string weoBoCallbackUrl
        {
            get { return "http://lylinux.org/OAuth/AuthorizedWeiBo"; }
        }

        private static WeiBoOpenAuthentication GetWeiBoClient(string callbackUrl = "http://lylinux.org/OAuth/WeiBoAuthorized")
        {
            HttpRequest request = HttpContext.Current.Request;
            var session = HttpContext.Current.Session;
            var cookies = request.Cookies;

            var accessToken = session["access_token"] == null ? string.Empty : (string)session["access_token"];
            var uid = cookies["uid"] == null ? string.Empty : cookies["uid"].Value;
            var client = new WeiBoOpenAuthentication(ConfigInfo.SinaAppKey, ConfigInfo.SinaAppSecret, callbackUrl, accessToken, uid);
            return client;
        }

        private static QQOpenAuthentication GetQQClient(string callbackUrl = "http://lylinux.org/OAuth/QQAuthorized")
        {
            HttpRequest request = HttpContext.Current.Request;
            var session = HttpContext.Current.Session;
            var cookies = request.Cookies;

            var accessToken = session["access_token"] == null ? string.Empty : (string)session["access_token"];
            var uid = cookies["uid"] == null ? string.Empty : cookies["uid"].Value;
            var client = new QQOpenAuthentication(ConfigInfo.QQAppKey, ConfigInfo.QQAppSecret, HttpUtility.UrlEncode(callbackUrl), accessToken, uid);
            return client;
        }

        public static OpenAuthenticationBase GetOAuthClient(OAuthSystemType systemtype)
        {
            switch (systemtype)
            {
                case OAuthSystemType.Weibo:
                    return GetWeiBoClient();
                case OAuthSystemType.QQ:
                    return GetQQClient();
                default:
                    throw new NotSupportedException();
            }
        }

        public static bool AuthorizedCode(OAuthSystemType systemtype, string code)
        {
            var client = GetOAuthClient(systemtype);
            client.GetAccessTokenByCode(code);
            if (client.IsAuthorized)
            {
                HttpContext.Current.Session["access_token"] = client.AccessToken;
                HttpContext.Current.Response.AppendCookie(new HttpCookie("uid", client.UID) { Expires = DateTime.Now.AddDays(7) });
                log4net.LogManager.GetLogger("api").Info(string.Format("SystemType:{0}--AccessToken:{1}--Uid:{2}", systemtype.ToString(), client.AccessToken, client.UID));
                return true;
            }
            else
            {
                return false;
            }
        }

        public static OAuthUserInfo GetOAuthUserInfo(OAuthSystemType systemtype)
        {
            var client = GetOAuthClient(systemtype);
            var userinfo = client.GetOAuthUserInfo();
            return userinfo;
        }
        public static void UpdateUserOAuthInfo(OAuthUserInfo userinfo)
        {
            if (userinfo != null)
            {
                var loginuser = UserHelper.GetLogInUserInfo();
                if (loginuser != null)
                {
                    IUserService userService = (IUserService)ResolverHelper.GetResolver<UserService>();
                    if (userinfo.SystemType == OAuthSystemType.QQ)
                    {
                        loginuser.QQAccessToken = userinfo.AccessToken;
                        loginuser.QQUid = userinfo.Uid;
                        loginuser.QQAvator = userinfo.ProfileImgUrl;
                        loginuser.Name = userinfo.Name;
                        userService.UpdateAsync(loginuser);

                    }
                    if (userinfo.SystemType == OAuthSystemType.Weibo)
                    {
                        loginuser.WeiBoAccessToken = userinfo.AccessToken;
                        loginuser.WeiBoUid = userinfo.Uid;
                        loginuser.WeiBoAvator = userinfo.ProfileImgUrl;
                        loginuser.Name = userinfo.Name;
                        userService.UpdateAsync(loginuser);
                    }
                }
            }
        }

        public static string GetOAuthLoginUrl(OAuthSystemType systemtype)
        {
            var client = GetOAuthClient(systemtype);
            return client.GetAuthorizationUrl();
        }
    }
}
