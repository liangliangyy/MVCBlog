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

        public event EventHandler<ModelCacheEventArgs> ModelCreateEventHandler;
        public event EventHandler<ModelCacheEventArgs> ModelDeleteEventHandler;
        public event EventHandler<ModelCacheEventArgs> ModelUpdateEventHandler;

        public void AddCommentInfo(CommentInfo entity)
        {
            var postinfo = Context.PostInfo.Find(entity.PostID);
            postinfo.CommentCount += 1;
            entity.CommentUser = Context.UserInfo.Find(entity.CommentUser.Id);

            Context.CommentInfo.Add(entity);
            Context.SaveChanges();
            RedisHelper.AddEntityToList(ConfigInfo.GetCommentKey(entity.PostID), entity);
            RedisHelper.DeleteEntity(ConfigInfo.GetPostKey(entity.PostID));
        }

        public List<CommentInfo> CommentList(int postid)
        {
            Func<List<CommentInfo>> getbydb = () =>
            {
                return Context.CommentInfo.Where(x => x.PostID == postid).ToList();
            };
            string key = ConfigInfo.GetCommentKey(postid);
            var list = RedisHelper.GetEntityByList(key, getbydb);
            return list;
        }

        public void Delete(CommentInfo model)
        {
            var entity = Context.CommentInfo.Find(model.Id);
            entity.IsDelete = true;
            Context.SaveChanges();
            if (ModelDeleteEventHandler != null)
            {
                ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetCommentKey(model.PostID), ID = model.Id };
                ModelDeleteEventHandler(this, e);
            }
        }

        public async Task DeleteAsync(CommentInfo model)
        {
            var entity = Context.CommentInfo.Find(model.Id);
            entity.IsDelete = true;
            await SaveChanges();
            if (ModelDeleteEventHandler != null)
            {
                ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetCommentKey(model.PostID), ID = model.Id };
                ModelDeleteEventHandler(this, e);
            }
        }

        public CommentInfo GetById(int id)
        {
            var entity = Context.CommentInfo.Find(id);
            return entity;
        }

        public async Task<CommentInfo> GetByIdAsync(int id)
        {
            var entity = await Context.CommentInfo.FindAsync(id);
            return entity;
        }

        public CommentInfo GetFromDB(int id)
        {
            return Context.CommentInfo.Find(id);
        }

        public void Insert(CommentInfo model, int userid = 0)
        {
            model.CommentUser = Context.UserInfo.Find(userid);
            Context.CommentInfo.Add(model);
            Context.SaveChanges();
            if (ModelCreateEventHandler != null)
            {
                ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetCommentKey(model.PostID), ID = model.Id };
                ModelCreateEventHandler(this, e);
            }
        }

        public async Task InsertAsync(CommentInfo model, int userid = 0)
        {
            model.CommentUser = await Context.UserInfo.FindAsync(userid);
            Context.CommentInfo.Add(model);
            await SaveChanges();
            if (ModelCreateEventHandler != null)
            {
                ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetCommentKey(model.PostID), ID = model.Id };
                ModelCreateEventHandler(this, e);
            }
        }

        public async Task<int> SaveChanges()
        {
            return await Context.SaveChangesAsync();
        }

        public void Update(CommentInfo model)
        {
            var entity = Context.CommentInfo.Find(model.Id);
            entity.CommentContent = model.CommentContent;
            Context.SaveChanges();
            if (ModelUpdateEventHandler != null)
            {
                ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetCommentKey(model.PostID), ID = model.Id };
                ModelUpdateEventHandler(this, e);
            }
        }

        public async Task UpdateAsync(CommentInfo model)
        {
            var entity = await Context.CommentInfo.FindAsync(model.Id);
            entity.CommentContent = model.CommentContent;
            await SaveChanges();
            if (ModelUpdateEventHandler != null)
            {
                ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetCommentKey(model.PostID), ID = model.Id };
                ModelUpdateEventHandler(this, e);
            }
        }
    }
}
