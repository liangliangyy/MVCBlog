using MVCBlog.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCBlog.Entities.Models;
using MVCBlog.Repository;
using MVCBlog.CacheManager;
using MVCBlog.Common;

namespace MVCBlog.Service
{
    public class CommentService : ICommentService
    {
        private MVCBlogContext Context;
        public CommentService(MVCBlogContext _contest)
        {
            this.Context = _contest;
        }
        public void AddCommentInfo(CommentInfo entity)
        {
            var postinfo = Context.PostInfo.Find(entity.PostID);
            postinfo.CommentCount += 1;
            entity.CommentUser = Context.UserInfo.Find(entity.CommentUser.Id);
            
            Context.CommentInfo.Add(entity);
            Context.SaveChanges();
            RedisHelper.DeleteEntity(ConfigInfo.GetPostKey(entity.PostID));
        }

        public List<CommentInfo> CommentList(int postid)
        {
            var res = Context.CommentInfo.Where(x => x.PostID == postid).ToList();
            return res;
        }
    }
}
