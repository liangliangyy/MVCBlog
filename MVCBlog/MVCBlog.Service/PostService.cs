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

namespace MVCBlog.Service
{
    public class PostService : IPostService
    {
        private MVCBlogContext Context;

        public event EventHandler<ModelCacheEventArgs> ModelDeleteEventHandler;
        public event EventHandler<ModelCacheEventArgs> ModelCreateEventHandler;
        public event EventHandler<ModelCacheEventArgs> ModelUpdateEventHandler;

        public PostService(MVCBlogContext _contest)
        {
            this.Context = _contest;
        }

        public void Delete(PostInfo model)
        {
            if (model != null)
            {
                var entity = Context.PostInfo.Find(model.Id);
                entity.IsDelete = true;
                entity.PostStatus = PostStatus.删除;
                Context.SaveChanges();
                if (ModelDeleteEventHandler != null)
                {
                    ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetPostKey(model.Id), ID = model.Id };
                    ModelDeleteEventHandler(this, e);
                }
            }
        }

        public async Task DeleteAsync(PostInfo model)
        {
            if (model != null)
            {
                var entity = Context.PostInfo.Find(model.Id);
                entity.IsDelete = true;
                entity.PostStatus = PostStatus.删除;
                await SaveChanges();
                if (ModelDeleteEventHandler != null)
                {
                    ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetPostKey(model.Id), ID = model.Id };
                    ModelDeleteEventHandler(this, e);
                }
            }
        }
        public PostInfo GetById(int id)
        {
            Func<PostInfo> GetDb = () => Context.PostInfo.Find(id);
            string key = ConfigInfo.GetPostKey(id);
            PostInfo info = RedisHelper.GetEntity<PostInfo>(key, GetDb);
            return info;
        }
        public async Task<PostInfo> GetByIdAsync(int id)
        {
            Func<PostInfo> GetDb = () => Context.PostInfo.Find(id);
            string key = ConfigInfo.GetPostKey(id);
            return await RedisHelper.GetEntityAsync<PostInfo>(key, GetDb);
        }

        public List<PostInfo> GetPosts()
        {
            return Context.PostInfo.ToList();
            //return Context.PostInfo.Where(x => !x.IsDelete && x.PostStatus == PostStatus.发布).ToList();
        }
        public async Task<List<PostInfo>> GetPostsAsync()
        {
            Func<List<PostInfo>> GetItems = () => { return Context.PostInfo.ToList(); };
            return await Common.TaskExtensions.WithCurrentCulture<List<PostInfo>>(GetItems);
        }

        public List<PostInfo> GetRecentPost(int count)
        {
            var res = Context.PostInfo.OrderByDescending(x => x.Id).Take(count).Select(x => x.Id).ToList();
            if (res != null && res.Count > 0)
            {
                List<PostInfo> list = new List<PostInfo>();
                foreach (var item in res)
                {
                    string key = ConfigInfo.GetPostKey(item);
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
                    string key = ConfigInfo.GetPostKey(item);
                    Func<PostInfo> GetDb = () => Context.PostInfo.Find(item);
                    PostInfo info = RedisHelper.GetEntity<PostInfo>(key, GetDb);
                    list.Add(info);
                }
                return list;
            }
            return new List<PostInfo>();
        }

        public async Task<Pagination<PostInfo>> GetUserPostsAsync(int authorid, int index, int pagecount)
        {
            Func<IPagedList<int>> GetIds = () =>
            {
                return Context.PostInfo
             .Where(x => x.PostAuthor.Id == authorid)
             .Select(x => x.Id).OrderByDescending(x => x)
             .ToPagedList(index, pagecount);
            };
            IPagedList<int> postids = await Common.TaskExtensions.WithCurrentCulture<IPagedList<int>>(GetIds);

            // var postids = Common.TaskExtensions.GetItemAsync(GetIds).GetAwaiter().GetResult();
            if (postids != null && postids.Count > 0)
            {
                List<PostInfo> list = new List<PostInfo>();
                foreach (var item in postids)
                {
                    string key = ConfigInfo.GetPostKey(item);
                    Func<PostInfo> GetDb = () => Context.PostInfo.Find(item);

                    //PostInfo info = await Common.TaskExtensions.WithCurrentCulture<PostInfo>(GetDb);
                    PostInfo info = await RedisHelper.GetEntityAsync<PostInfo>(key, GetDb);
                    list.Add(info);
                }
                Pagination<PostInfo> pagination = new Pagination<PostInfo>()
                {
                    Items = list,
                    TotalItemCount = postids.TotalItemCount,
                    PageCount = postids.PageCount,
                    PageNumber = postids.PageNumber,
                    PageSize = postids.PageSize
                };
                return pagination;
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

        public Pagination<PostInfo> GetUserPosts(int authorid, int index, int pagecount)
        {
            var postids = Context.PostInfo
                .Where(x => x.PostAuthor.Id == authorid)
                .Select(x => x.Id).OrderByDescending(x => x)
                .ToPagedList(index, pagecount);
            if (postids != null && postids.Count > 0)
            {
                List<PostInfo> list = new List<PostInfo>();
                foreach (var item in postids)
                {
                    string key = ConfigInfo.GetPostKey(item);
                    Func<PostInfo> GetDb = () => Context.PostInfo.Find(item);
                    PostInfo info = RedisHelper.GetEntity<PostInfo>(key, GetDb);
                    list.Add(info);
                }
                Pagination<PostInfo> pagination = new Pagination<PostInfo>()
                {
                    Items = list,
                    TotalItemCount = postids.TotalItemCount,
                    PageCount = postids.PageCount,
                    PageNumber = postids.PageNumber,
                    PageSize = postids.PageSize
                };
                return pagination;
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


        public async Task InsertAsync(PostInfo model, int userid)
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
            //string key = ConfigInfo.GetPostKey(entity.Id);
            //RedisHelper.SetEntity<PostInfo>(key, entity);
            if (ModelCreateEventHandler != null)
            {
                ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetPostKey(model.Id), ID = model.Id };
                ModelCreateEventHandler(this, e);
            }
        }

        public void Insert(PostInfo model, int userid)
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

            if (ModelCreateEventHandler != null)
            {
                ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetPostKey(model.Id), ID = model.Id };
                ModelCreateEventHandler(this, e);
            }
        }

        public Pagination<PostInfo> PostPagination(int index, int pagecount)
        {
            var res = Context.PostInfo.OrderByDescending(x => x.Id).Select(x => x.Id).ToPagedList(index, pagecount);
            if (res != null && res.Count > 0)
            {
                List<PostInfo> list = new List<PostInfo>();
                foreach (var item in res)
                {
                    string key = ConfigInfo.GetPostKey(item);
                    Func<PostInfo> GetDb = () => Context.PostInfo.Find(item);
                    PostInfo info = RedisHelper.GetEntity<PostInfo>(key, GetDb);
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
                return pagination;
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
        public async Task<Pagination<PostInfo>> PostPaginationAsync(int index, int pagecount)
        {
            Func<IPagedList<int>> getids = () =>
            {
                return Context.PostInfo.OrderByDescending(x => x.Id).Select(x => x.Id).ToPagedList(index, pagecount);
            };
            var res = await Common.TaskExtensions.WithCurrentCulture<IPagedList<int>>(getids);
            if (res != null && res.Count > 0)
            {
                List<PostInfo> list = new List<PostInfo>();
                foreach (var item in res)
                {
                    string key = ConfigInfo.GetPostKey(item);
                    Func<PostInfo> GetDb = () => Context.PostInfo.Find(item);
                    PostInfo info = await RedisHelper.GetEntityAsync<PostInfo>(key, GetDb);
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
                return pagination;
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
        public void Update(PostInfo model)
        {
            var entity = Context.PostInfo.Find(model.Id);
            entity.Title = model.Title;
            entity.Content = model.Content;
            entity.PostStatus = model.PostStatus;
            entity.PostCommentStatus = model.PostCommentStatus;
            entity.EditedTime = DateTime.Now;
            Context.SaveChanges();
            if (ModelUpdateEventHandler != null)
            {
                ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetPostKey(model.Id), ID = model.Id };
                ModelUpdateEventHandler(this, e);
            }
        }

        public async Task UpdateAsync(PostInfo model)
        {
            var entity = Context.PostInfo.Find(model.Id);
            entity.Title = model.Title;
            entity.Content = model.Content;
            entity.PostStatus = model.PostStatus;
            entity.PostCommentStatus = model.PostCommentStatus;
            entity.EditedTime = DateTime.Now;
            string key = ConfigInfo.GetPostKey(model.Id);
            RedisHelper.DeleteEntity(key);
            await SaveChanges();
            if (ModelUpdateEventHandler != null)
            {
                ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetPostKey(model.Id), ID = model.Id };
                ModelUpdateEventHandler(this, e);
            }
        }
        public async Task<int> SaveChanges()
        {
            return await Common.TaskExtensions.WithCurrentCulture<int>(this.Context.SaveChangesAsync());
        }

        public PostInfo GetFromDB(int id)
        {
            return Context.PostInfo.Find(id);
        }
    }
}
