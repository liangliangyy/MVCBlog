using MVCBlog.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCBlog.Entities.Models;
using MVCBlog.Repository;

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
            Context.CommentInfo.Add(entity);
            Context.SaveChanges();
        }

        public List<CommentInfo> CommentList(int postid)
        {
            var res = Context.CommentInfo.Where(x => x.CommentPost.Id == postid).ToList();
            return res;
        }
    }
}
