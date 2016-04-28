using Autofac;
using MVCBlog.Common;
using MVCBlog.Common.OAuth.Models;
using MVCBlog.Entities.Enums;
using MVCBlog.Entities.Models;
using MVCBlog.Service;
using MVCBlog.Service.Interfaces;
using MVCBlog.Web.Infrastructure;
using MVCBlog.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace MVCBlog.Web.CommonHelper
{
    public class UserHelper
    {
        public static UserInfo GetLogInUserInfo()
        {
            try
            {
                MVCBlogIdentity mvcblogidentity = HttpContext.Current.User.Identity as MVCBlogIdentity;
                if (mvcblogidentity != null && mvcblogidentity.IsAuthenticated)
                {
                    IUserService userservice = ApplicationContainer.Container.Resolve<IUserService>();
                    var userinfo = userservice.GetById(mvcblogidentity.UserData.Id);
                    return userinfo;
                }
            }
            catch
            {
                //HttpContext.Current.User = null;
                //HttpContext.Current.Response.Redirect("/Admin/LogIn");
            }
            return null;
        }

        public static void UserLogOut()
        {
            try
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    MVCBlogIdentity mvcblogidentity = HttpContext.Current.User.Identity as MVCBlogIdentity;
                    if (mvcblogidentity != null)
                    {
                        var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                        cookie.Expires = DateTime.Now.AddDays(-1);
                        HttpContext.Current.Response.Cookies.Add(cookie);
                        HttpContext.Current.User = null;
                    }
                }
            }
            catch
            {

            }
        }

        public static void CreateNewUserByOAuthUser(OAuthUserInfo oauthUser)
        {
            if (oauthUser != null)
            {
                UserInfo u = new UserInfo()
                {
                    Email = oauthUser.Uid,
                    Password = ConfigInfo.UserDefaultPassword,
                    CreateTime = DateTime.Now,
                    Name = oauthUser.Name,
                    UserRole = UserRole.读者,
                    UserStatus = UserStatus.正常
                };
                switch (oauthUser.SystemType)
                {
                    case OAuthSystemType.Weibo:
                        {
                            u.WeiBoAccessToken = oauthUser.AccessToken;
                            u.WeiBoAvator = oauthUser.ProfileImgUrl;
                            u.WeiBoUid = oauthUser.Uid;
                            break;
                        }
                    case OAuthSystemType.QQ:
                        {
                            u.QQAccessToken = oauthUser.AccessToken;
                            u.QQAvator = oauthUser.ProfileImgUrl;
                            u.QQUid = oauthUser.Uid;
                            break;
                        }
                    default:
                        break;
                }
                if (!string.IsNullOrEmpty(u.QQUid) || !string.IsNullOrEmpty(u.WeiBoUid))
                {
                    IUserService service = ApplicationContainer.Container.Resolve<IUserService>();
                    service.Insert(u);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public static void HandleOauthUserLogIn(OAuthUserInfo oauthUserinfo)
        {
            if (oauthUserinfo == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                OAuthClientFactory.UpdateUserOAuthInfo(oauthUserinfo);
                IUserService userService = ApplicationContainer.Container.Resolve<IUserService>();
                var userinfo = userService.GetUserInfoByUid(oauthUserinfo.Uid, oauthUserinfo.SystemType);
                if (userinfo == null)
                {
                    CreateNewUserByOAuthUser(oauthUserinfo);
                    userinfo = userService.GetUserInfoByUid(oauthUserinfo.Uid, oauthUserinfo.SystemType);
                }
                UserDataModel userData = new UserDataModel()
                {
                    Id = userinfo.Id,
                    Email = userinfo.Email,
                    Name = userinfo.Name,
                    SystemType = OAuthSystemType.Weibo,
                    Uid = oauthUserinfo.Uid,
                    AccessToken = oauthUserinfo.AccessToken,
                    UserRoles = new List<UserRole>() { userinfo.UserRole }
                };
                SetFormsAuthenticationTicket(userinfo.Email, userData, true);
            }
        }

        public static void SetFormsAuthenticationTicket(string email, UserDataModel userdata, bool isRemember)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("user email cannot be null");
            }
            string userDataJson = JsonConvert.SerializeObject(userdata);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, email, DateTime.Now, DateTime.Now.AddMonths(1), isRemember, userDataJson, FormsAuthentication.FormsCookiePath);

            string encTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            if (isRemember)
            {
                cookie.Expires = ticket.Expiration;
            }

            cookie.Domain = FormsAuthentication.CookieDomain;

            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
