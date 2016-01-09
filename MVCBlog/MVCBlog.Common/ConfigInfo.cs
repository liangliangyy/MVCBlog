using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Common
{
    public static class ConfigInfo
    {
        public static string GetUserKey(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                throw new NotSupportedException("email不能为空");
            }
            return string.Format("{0}:{1}", UserCackeKey, Email);
        }
        private static string UserCackeKey { get; set; }
        static ConfigInfo()
        {
            UserCackeKey = "MVCBlog:UserInfo";
        }
    }
}
