using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Common.OAuth.Models
{
    public class OAuthUserInfo
    {
        public string Uid { get; set; }
        public string AccessToken { get; set; }
        public string Name { get; set; }
        public OAuthSystemType SystemType { get; set; }
        public string ProfileImgUrl { get; set; }
        
    }
}
