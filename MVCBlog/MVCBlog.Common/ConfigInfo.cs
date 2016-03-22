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

        public static string GetPostKey(int id)
        {
            if (id == 0)
            {
                throw new NotSupportedException("id必须大于0");
            }
            return string.Format("{0}:{1}", PostListKey, id);
        }
        public static string GetCommentKey(int postid)
        {
            if (postid == 0)
            {
                throw new NotSupportedException("id必须大于0");
            }
            return string.Format("{0}:{1}", CommentKey, postid);
        }
        public static string GetCategoryKey(int id)
        {
            return string.Format("MVCBlog:CategoryInfo:{0}",id);
        }

        public static int RecentPostCount { get { return 5; } }

        public static int PageCount { get { return 20; } }
        public static string TimeFormat { get { return "yyyy-MM-dd HH:mm:ss"; } }
        public static string UserDefaultPassword { get { return "1"; } }

        private static string UserCackeKey { get; set; }
        private static string PostListKey { get; set; }
        private static string CommentKey { get; set; }


        //sinaconfig
        public static string SinaAppKey { get { return "1072707227"; } }
        public static string SinaAppSecret { get { return "db87257dbcbaac074e8a3efecbff968e"; } }

        public static string QQAppKey { get { return "101293291"; } }
        public static string QQAppSecret { get { return "1dca3b9dbe972057436d6f5f9c8a40a4"; } }

        static ConfigInfo()
        {
            UserCackeKey = "MVCBlog:UserInfo";
            PostListKey = "MVCBlog:PostInfo";
            CommentKey = "MVCBlog:CommentInfo";
        }
    }
}
