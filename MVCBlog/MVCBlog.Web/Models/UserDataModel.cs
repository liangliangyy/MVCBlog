using MVCBlog.Common.OAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Web.Models
{
    public class UserDataModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Uid { get; set; }
        public string AccessToken { get; set; }
        public OAuthSystemType SystemType { get; set; }
    }
}
