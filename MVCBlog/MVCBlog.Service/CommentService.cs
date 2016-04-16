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
    public class CommentService : BaseService<CommentInfo>, ICommentService
    {
        private MVCBlogContext Context;
        public CommentService(MVCBlogContext _contest) : base(_contest)
        {
            this.Context = _contest;
        }
        
        public override void Delete(CommentInfo model)
        {
            var entity = Context.CommentInfo.Find(model.Id);
            entity.IsDelete = true;
            Context.SaveChanges();
            base.Delete(model);
        }

        public override async Task DeleteAsync(CommentInfo model)
        {
            var entity = Context.CommentInfo.Find(model.Id);
            entity.IsDelete = true;
            await SaveChanges();
            await base.DeleteAsync(model);
        }
         
        public override string GetModelKey(int id)
        {
            return RedisKeyHelper.GetCommentKey(id);
        }

        public override void Insert(CommentInfo model, int userid = 0)
        {
            model.CommentUser = Context.UserInfo.Find(model.CommentUser.Id);
            Context.CommentInfo.Add(model);
            Context.SaveChanges();
            base.Insert(model, userid);
        }

        public override async Task InsertAsync(CommentInfo model, int userid = 0)
        {
            model.CommentUser = await Context.UserInfo.FindAsync(userid);
            Context.CommentInfo.Add(model);
            await SaveChanges();
            await base.InsertAsync(model, userid);
        }

        public override async Task<int> SaveChanges()
        {
            return await Context.SaveChangesAsync();
        }

        public override void Update(CommentInfo model)
        {
            var entity = Context.CommentInfo.Find(model.Id);
            entity.CommentContent = model.CommentContent;
            Context.SaveChanges();
            base.Update(model);
        }

        public override async Task UpdateAsync(CommentInfo model)
        {
            var entity = await Context.CommentInfo.FindAsync(model.Id);
            entity.CommentContent = model.CommentContent;
            await SaveChanges();
            await base.UpdateAsync(model);
        }
    }
}
