using MVCBlog.Entities.Models;
using MVCBlog.Service;
using MVCBlog.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MVCBlog.Web.CommonHelper
{
    public class UserHelper
    {
        public static UserInfo GetLogInUserInfo()
        {
            try
            {
                IIdentity id = HttpContext.Current.User.Identity;
                if (id.IsAuthenticated)
                {
                    IUserService userservice = (IUserService)ResolverHelper.GetResolver<UserService>();
                    var userinfo = userservice.GetUserInfo(id.Name);
                    return userinfo;
                }
            }
            catch 
            {
                HttpContext.Current.User = null;
                HttpContext.Current.Response.Redirect("/Admin/LogIn");
            }
            return null;
        }
    }
}
