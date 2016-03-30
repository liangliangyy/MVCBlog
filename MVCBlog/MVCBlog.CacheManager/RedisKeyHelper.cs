using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.CacheManager
{
    public static class RedisKeyHelper
    {
        static RedisKeyHelper()
        {
            UserCackeKey = "MVCBlog:UserInfo";
            PostListKey = "MVCBlog:PostInfo";
            CommentKey = "MVCBlog:CommentInfo";
            CategoryKey = "MVCBlog:CategoryInfo";
        }
        public static string GetUserKey(int id)
        {
            return string.Format("{0}:{1}", UserCackeKey, id);
        }
        private static string UserCackeKey { get; set; }
        private static string PostListKey { get; set; }
        private static string CommentKey { get; set; }
        private static string CategoryKey { get; set; }
        public static string GetPostKey(int id)
        {
            return string.Format("{0}:{1}", PostListKey, id);
        }
        public static string GetCommentKey(int postid)
        {
            return string.Format("{0}:{1}", CommentKey, postid);
        }
        public static string GetCategoryKey(int id)
        {
            return string.Format("{0}:{1}", CategoryKey, id);
        }
    }
}
