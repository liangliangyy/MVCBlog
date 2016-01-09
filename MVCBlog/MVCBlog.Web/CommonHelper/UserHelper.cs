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

            IIdentity id = HttpContext.Current.User.Identity;
            if (id.IsAuthenticated)
            {
                UserService userservice = (UserService)ResolverHelper.GetResolver<UserService>();
                var userinfo = userservice.GetUserInfo(id.Name);
                return userinfo;
            }
            return null;
        }
    }
}
