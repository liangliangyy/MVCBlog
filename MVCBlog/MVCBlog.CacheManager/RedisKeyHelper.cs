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
            UserKey = "MVCBlog:UserInfo";
            PostListKey = "MVCBlog:PostInfo";
            CommentKey = "MVCBlog:CommentInfo";
            CategoryKey = "MVCBlog:CategoryInfo";
            PostMetasKey = "MVCBlog:PostMetas";
        }
        public static string GetUserKey(int id)
        {
            return string.Format("{0}:{1}", UserKey, id);
        }
        private static string UserKey { get; set; }
        private static string PostListKey { get; set; }
        private static string CommentKey { get; set; }
        private static string CategoryKey { get; set; }
        private static string PostMetasKey { get; set; }
        public static string GetPostKey(int id)
        {
            return string.Format("{0}:{1}", PostListKey, id);
        }
        public static string GetCommentKey(int id)
        {
            return string.Format("{0}:{1}", CommentKey, id);
        }
        public static string GetCategoryKey(int id)
        {
            return string.Format("{0}:{1}", CategoryKey, id);
        }
        public static string GetPostMeatsKey(int id)
        {
            return string.Format("{0}:{1}", PostMetasKey, id);
        }
    }
}
