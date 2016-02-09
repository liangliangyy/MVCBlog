using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCBlog.Web.Models;
using Microsoft.AspNet.Identity;

namespace MVCBlog.Web.App_Start
{
    public class IdentityConfig
    {
        
    }
    public class UserManager:UserManager<ApplicationUser>
    {
        public UserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

    }
}
