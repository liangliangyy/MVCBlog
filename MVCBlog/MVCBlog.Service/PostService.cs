using System;
using System.Collections.Generic;
using System.Linq;
using MVCBlog.Entities.Models;
using MVCBlog.Service.Interfaces;
using MVCBlog.Repository;
using MVCBlog.Entities.Enums;
using PagedList;
using MVCBlog.Common;
using MVCBlog.CacheManager;
using MVCBlog.Entities;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace MVCBlog.Service
{
    public class PostService : BaseService<PostInfo>, IPostService
    {
        private MVCBlogContext Context;


        public PostService(MVCBlogContext _contest) : base(_contest)
        {
            this.Context = _contest;
        }

        public override void Delete(PostInfo model)
        {
            if (model != null)
            {
                var entity = Context.PostInfo.Find(model.Id);
                entity.IsDelete = true;
                entity.PostStatus = PostStatus.删除;
                Context.SaveChanges();
                base.Delete(model);
            }
        }

        public override async Task DeleteAsync(PostInfo model)
        {
            if (model != null)
            {
                var entity = Context.PostInfo.Find(model.Id);
                entity.IsDelete = true;
                entity.PostStatus = PostStatus.删除;
                await SaveChanges();
                await base.DeleteAsync(model);
            }
        }
     

        public List<PostInfo> GetRecentPost(int count)
        {
            var res = Context.PostInfo.OrderByDescending(x => x.Id).Take(count).Select(x => x.Id).ToList();
            if (res != null && res.Count > 0)
            {
                List<PostInfo> list = new List<PostInfo>();
                foreach (var item in res)
                {
                    string key = RedisKeyHelper.GetPostKey(item);
                    Func<PostInfo> GetDb = () => Context.PostInfo.Find(item);
                    PostInfo info = RedisHelper.GetEntity<PostInfo>(key, GetDb);
                    list.Add(info);
                }
                return list;
            }
            return new List<PostInfo>();
        }

        public async Task<List<PostInfo>> GetRecentPostAsync(int count)
        {
            Func<List<int>> GetIds = () =>
            {
                return Context.PostInfo.OrderByDescending(x => x.Id).Take(count).Select(x => x.Id).ToList();
            };
            //var res = Context.PostInfo.OrderByDescending(x => x.Id).Take(count).Select(x => x.Id).ToList();
            var res = await GetIds.WithCurrentCulture<List<int>>();
            if (res != null && res.Count > 0)
            {
                List<PostInfo> list = new List<PostInfo>();
                foreach (var item in res)
                {
                    string key = RedisKeyHelper.GetPostKey(item);
                    Func<PostInfo> GetDb = () => Context.PostInfo.Find(item);
                    PostInfo info = RedisHelper.GetEntity<PostInfo>(key, GetDb);
                    list.Add(info);
                }
                return list;
            }
            return new List<PostInfo>();
        }
         
        public override async Task InsertAsync(PostInfo model, int userid)
        {
            model.PostStatus = PostStatus.发布;
            model.PostType = PostType.文章;
            model.PostCommentStatus = PostCommentStatus.打开;
            model.CommentCount = 0;
            model.CreateTime = DateTime.Now;
            model.IsDelete = false;
            model.PostAuthor = Context.UserInfo.Find(userid);
            model.PostCategoryInfo = Context.CategoryInfo.Find(model.PostCategoryInfo.Id);
            var entity = Context.PostInfo.Add(model);
            await SaveChanges();
            await base.InsertAsync(model, userid);
        }

        public override void Insert(PostInfo model, int userid)
        {
            model.PostStatus = PostStatus.发布;
            model.PostType = PostType.文章;
            model.PostCommentStatus = PostCommentStatus.打开;
            model.CommentCount = 0;
            model.CreateTime = DateTime.Now;
            model.IsDelete = false;
            model.PostAuthor = Context.UserInfo.Find(userid);
            model.PostCategoryInfo = Context.CategoryInfo.Find(model.PostCategoryInfo.Id);
            var entity = Context.PostInfo.Add(model);
            Context.SaveChanges();
            base.Insert(model, userid);
        }
        
   
        public override void Update(PostInfo model)
        {
            var entity = Context.PostInfo.Find(model.Id);
            entity.Title = model.Title;
            entity.Content = model.Content;
            entity.PostStatus = model.PostStatus;
            entity.PostCommentStatus = model.PostCommentStatus;
            entity.EditedTime = DateTime.Now;
            Context.SaveChanges();
            base.Update(model);
        }

        public override async Task UpdateAsync(PostInfo model)
        {
            var entity = Context.PostInfo.Find(model.Id);
            entity.Title = model.Title;
            entity.Content = model.Content;
            entity.PostStatus = model.PostStatus;
            entity.PostCommentStatus = model.PostCommentStatus;
            entity.EditedTime = DateTime.Now;
            string key = RedisKeyHelper.GetPostKey(model.Id);
            RedisHelper.DeleteEntity(key);
            await SaveChanges();
            await base.UpdateAsync(model);
        }
        public override async Task<int> SaveChanges()
        {
            return await Common.TaskExtensions.WithCurrentCulture<int>(this.Context.SaveChangesAsync());
        }
         
        public override string GetModelKey(int id)
        {
            return RedisKeyHelper.GetPostKey(id);
        }

        public IEnumerable<DateTime> GetPostMonthInfos()
        {
            var datetimes = Context.PostInfo.Select(x => x.CreateTime).ToList();
            return datetimes;
        }

        public Pagination<PostInfo> Query(Expression<Func<PostInfo, bool>> query, int index, int pagecount)
        {
            // System.Linq.Expressions.Expression<Func<PostInfo, bool>> ex = s => s.IsDelete == true;
            var res = Context.PostInfo.Where(query).Select(x => x.Id).OrderByDescending(x => x).ToPagedList(index, pagecount);
            if (res != null && res.Count > 0)
            {
                List<PostInfo> list = new List<PostInfo>();
                foreach (var item in res)
                {
                    string key = RedisKeyHelper.GetPostKey(item);
                    Func<PostInfo> GetDb = () => Context.PostInfo.Find(item);
                    PostInfo info = RedisHelper.GetEntityAsync<PostInfo>(key, GetDb).Result;
                    list.Add(info);
                }
                Pagination<PostInfo> pagination = new Pagination<PostInfo>()
                {
                    Items = list,
                    TotalItemCount = res.TotalItemCount,
                    PageCount = res.PageCount,
                    PageNumber = res.PageNumber,
                    PageSize = res.PageSize
                };
            }
            return new Pagination<PostInfo>()
            {
                Items = null,
                TotalItemCount = 0,
                PageCount = 0,
                PageSize = pagecount,
                PageNumber = index
            };
        }
    }
}
