using MVCBlog.Common;
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
                    IUserService userservice = (IUserService)ResolverHelper.GetResolver<UserService>();
                    var userinfo = userservice.GetByIdAsync(mvcblogidentity.UserData.Id).Result;
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

        public static void SetFormsAuthenticationTicket(string email, UserDataModel userdata, bool isRemember)
        {
            if (string.IsNullOrEmpty(email))
            {
                email = StringHelper.CreateMD5(userdata.Id.ToString() + userdata.Uid);
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
