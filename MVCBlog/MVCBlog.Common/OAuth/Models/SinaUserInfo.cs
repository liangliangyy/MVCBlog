using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Common.OAuth.Models
{
    public class SinaUserInfo
    {
        public string screen_name { get; set; }
        public string profile_image_url { get; set; }
        public string description { get; set; }
        public string location { get; set; }
     
    }
}
