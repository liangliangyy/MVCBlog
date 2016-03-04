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
        private IIdentity identity { get; set; }
        public IIdentity Identity
        {
            get { return identity; }
            set { identity = value; }
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}
