using MVCBlog.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace MVCBlog.Web.Infrastructure
{
    public class MVCBlogIdentity : IIdentity
    {
        public UserDataModel UserData { get; set; }

        private FormsAuthenticationTicket ticket;

        public string AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }

        public string Name { get; set; }

        public FormsAuthenticationTicket Ticket
        {
            get { return ticket; }
            set { ticket = value; }
        }
        public MVCBlogIdentity(string userData)
        {
            UserData = JsonConvert.DeserializeObject<UserDataModel>(userData);
        }
    }
}
