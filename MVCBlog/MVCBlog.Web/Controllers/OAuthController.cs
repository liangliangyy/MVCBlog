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
        public JsonResult GetUserState()
        {
            JsonResult jr = new JsonResult();

            var client = OAuthHelper.GetWeiBoClient();
            if (!client.IsAuthorized)
            {
                jr.Data = new { code = 0, msg = "未授权", url = client.GetAuthorizationUrl() };
            }
            else
            {
                jr.Data = new { code = 1, msg = "已授权!" };
            }
            return jr;
        }

        [HttpGet]
        public ActionResult Authorized(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return RedirectToAction("index");
            }
            var client = OAuthHelper.GetWeiBoClient();
            client.GetAccessTokenByCode(code);
            if (client.IsAuthorized)
            {
                Session["access_token"] = client.AccessToken;
                Response.AppendCookie(new HttpCookie("uid", client.UID) { Expires = DateTime.Now.AddDays(7) });
                SinaUserInfo userinfo = client.GetUserInfo(client.AccessToken, client.UID);

                if (userinfo != null)
                {
                    Session["sinauserinfo"] = userinfo;
                    var entity = userService.GettUserInfoByUid(client.UID);
                    if (entity != null)
                    {
                        entity.Name = userinfo.screen_name;
                        entity.uid = userinfo.uid;
                        entity.access_token = userinfo.access_token;
                        entity.avator = userinfo.profile_image_url;
                        userService.UpdateAsync(entity);
                        FormsAuthentication.SetAuthCookie(entity.Email, false);
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize]
        public JsonResult BindUserWithWeibo()
        {
            JsonResult jr = new JsonResult();
            var cookie = Request.Cookies;
            SinaUserInfo sinauser = Session["sinauserinfo"] == null ? null : (SinaUserInfo)Session["sinauserinfo"];
            if (sinauser == null)
            {
                jr.Data = new { code = 0, msg = "未登录第三方" };
            }
            else
            {
                var userinfo = UserHelper.GetLogInUserInfo();
                userinfo.access_token = sinauser.access_token;
                userinfo.Name = sinauser.screen_name;
                userinfo.uid = sinauser.uid;
                userinfo.access_token = sinauser.access_token;
                userinfo.avator = sinauser.profile_image_url;
                userService.UpdateAsync(userinfo);
                jr.Data = new { code = 1, msg = "绑定成功!" };
            }
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