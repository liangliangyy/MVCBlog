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

namespace MVCBlog.Web.CommonHelper
{
    public class OAuthHelper
    {
        private static string defaultCallbackUrl
        {
            get { return "http://lylinux.org/OAuth/Authorized"; }
        }
        public static string GetWeiBoLoginUrl()
        {
            var client = GetWeiBoClient(defaultCallbackUrl);
            return client.GetAuthorizationUrl();

        }
        public static WeiBoOpenAuthentication GetWeiBoClient(string callbackUrl = "http://lylinux.org/OAuth/Authorized")
        {
            HttpRequest request = HttpContext.Current.Request;
            var session = HttpContext.Current.Session;
            var cookies = request.Cookies;

            var accessToken = session["access_token"] == null ? string.Empty : (string)session["access_token"];
            var uid = cookies["uid"] == null ? string.Empty : cookies["uid"].Value;
            var client = new WeiBoOpenAuthentication(ConfigInfo.SinaAppKey, ConfigInfo.SinaAppSecret, callbackUrl, accessToken, uid);
            return client;
        }

        public static QQOpenAuthentication GetQQClient(string callbackUrl = "http://lylinux.org/OAuth/QQAuthorized")
        {
            HttpRequest request = HttpContext.Current.Request;
            var session = HttpContext.Current.Session;
            var cookies = request.Cookies;

            var accessToken = session["access_token"] == null ? string.Empty : (string)session["access_token"];
            var uid = cookies["uid"] == null ? string.Empty : cookies["uid"].Value;
            var client = new QQOpenAuthentication(ConfigInfo.QQAppKey, ConfigInfo.QQAppSecret, HttpUtility.UrlEncode(callbackUrl), accessToken, uid);
            return client;
        }
    }

}
