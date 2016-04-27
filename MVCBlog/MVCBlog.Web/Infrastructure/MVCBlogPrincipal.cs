using MVCBlog.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Web.Infrastructure
{
    public class MVCBlogPrincipal : IPrincipal
    {
        private IIdentity _identity { get; set; }
        private UserRole[] _roles { get; set; }
        public MVCBlogPrincipal(IIdentity identity, UserRole[] roles)
        {
            this._identity = identity;
            this._roles = roles;
        }

        public IIdentity Identity
        {
            get
            {
                return _identity;
            }
        }

        public bool IsInRole(string role)
        {
            UserRole roleinfo = UserRole.作者;
            if (Enum.TryParse(role, out roleinfo))
            {
                return _roles.Any(x => x == roleinfo);
            }
            return false;
        }
    }
}
