using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBlog.Service.Interfaces;
using MVCBlog.Web.CommonHelper.Menu;
using Newtonsoft.Json;
using PagedList;
using MVCBlog.Entities.Models;
using MVCBlog.Common;
using MVCBlog.Entities;
using MVCBlog.Web.CommonHelper;
using MVCBlog.Common.OAuth.Models;
using MVCBlog.Service;
using System.Web.Security;
using MVCBlog.Web.Models;

namespace MVCBlog.Web.Controllers
{
    public class OAuthController : Controller
    {
        private IUserService userService = (IUserService)ResolverHelper.GetResolver<UserService>();

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetOauthLoginUrl(OAuthSystemType systemtype)
        {
            var client = OAuthClientFactory.GetOAuthClient(systemtype);
            JsonResult jr = new JsonResult();
            jr.Data = new { url = client.GetAuthorizationUrl() };
            return jr;
        }


        [HttpGet]
        public ActionResult WeiBoAuthorized(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return RedirectToAction("index");
            }
            bool result = OAuthClientFactory.AuthorizedCode(OAuthSystemType.Weibo, code);
            if (result)
            {
                var oauthUserinfo = OAuthClientFactory.GetOAuthUserInfo(OAuthSystemType.Weibo);
                OAuthClientFactory.UpdateUserOAuthInfo(oauthUserinfo);
                var userinfo = userService.GetUserInfoByUid(oauthUserinfo.Uid, OAuthSystemType.Weibo);
                if (userinfo != null)
                {
                    UserDataModel userData = new UserDataModel()
                    {
                        Id = userinfo.Id,
                        Email = userinfo.Email,
                        Name = userinfo.Name,
                        SystemType = OAuthSystemType.Weibo,
                        Uid =oauthUserinfo.Uid,
                        AccessToken = oauthUserinfo.AccessToken
                    };
                    UserHelper.SetFormsAuthenticationTicket(string.Empty, userData, true);
                }
            }
            return RedirectToAction("index");
        }



        [HttpGet]
        public ActionResult QQAuthorized(string code)
        {
            bool result = OAuthClientFactory.AuthorizedCode(OAuthSystemType.QQ, code);
            if (result)
            {
                var oauthUserinfo = OAuthClientFactory.GetOAuthUserInfo(OAuthSystemType.QQ);
                OAuthClientFactory.UpdateUserOAuthInfo(oauthUserinfo);
                var userinfo = userService.GetUserInfoByUid(oauthUserinfo.Uid, OAuthSystemType.QQ);
                if (userinfo != null)
                {
                    UserDataModel userData = new UserDataModel()
                    {
                        Id = userinfo.Id,
                        Email = userinfo.Email,
                        Name = userinfo.Name,
                        SystemType = OAuthSystemType.Weibo,
                        Uid = oauthUserinfo.Uid,
                        AccessToken = oauthUserinfo.AccessToken
                    };
                    UserHelper.SetFormsAuthenticationTicket(string.Empty, userData, true);
                }
            }
            return RedirectToAction("index");
        }
        [HttpPost]
        [Authorize]
        public JsonResult BindUserWithWeibo()
        {
            JsonResult jr = new JsonResult();
            //var cookie = Request.Cookies;
            //SinaUserInfo sinauser = Session["sinauserinfo"] == null ? null : (SinaUserInfo)Session["sinauserinfo"];
            //if (sinauser == null)
            //{
            //    jr.Data = new { code = 0, msg = "未登录第三方" };
            //}
            //else
            //{
            //    var userinfo = UserHelper.GetLogInUserInfo();
            //    userinfo.WeiBoAccessToken = sinauser.access_token;
            //    userinfo.Name = sinauser.screen_name;
            //    userinfo.WeiBoUid = sinauser.uid;
            //    userinfo.WeiBoAccessToken = sinauser.access_token;
            //    userinfo.WeiBoAvator = sinauser.profile_image_url;
            //    userService.UpdateAsync(userinfo);
            //    jr.Data = new { code = 1, msg = "绑定成功!" };
            //}
            return jr;
        }

        [HttpGet]
        [Authorize]
        public ActionResult BindUserInfo()
        {
            var userinfo = UserHelper.GetLogInUserInfo();
            return View(userinfo);
        }
    }
}