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
        public static string CategoryKey { get { return "MVCBlog:CategoryList"; } }

        public static int RecentPostCount { get { return 5; } }

        public static int PageCount { get { return 20; } }
        public static string TimeFormat { get { return "yyyy-MM-dd HH:mm:ss"; } }
        public static string UserDefaultPassword { get { return "1"; } }

        private static string UserCackeKey { get; set; }
        private static string PostListKey { get; set; }
        private static string CommentKey { get; set; }
        static ConfigInfo()
        {
            UserCackeKey = "MVCBlog:UserInfo";
            PostListKey = "MVCBlog:PostInfo";
            CommentKey = "MVCBlog:CommentInfo";
        }
    }
}
