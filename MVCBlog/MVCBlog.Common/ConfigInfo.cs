using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Common
{
    public static class ConfigInfo
    {
        public static int RecentPostCount { get { return 5; } }

        public static int PageCount { get { return 20; } }
        public static string TimeFormat { get { return "yyyy-MM-dd HH:mm:ss"; } }
        public static string UserDefaultPassword { get { return "1"; } }
        public static string SinaAppKey { get { return "4134375127"; } }
        public static string SinaAppSecret { get { return "77fb53aef8cd89a29a07a17fcb3a5561"; } }

        public static string QQAppKey { get { return "101293291"; } }
        public static string QQAppSecret { get { return "1dca3b9dbe972057436d6f5f9c8a40a4"; } }
    }
}
