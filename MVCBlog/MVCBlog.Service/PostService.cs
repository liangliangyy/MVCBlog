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
        //private MVCBlogContext Context;


        //public PostService(MVCBlogContext _context)
        //{
        //    this.Context = _context;
        //}

        public override void Delete(PostInfo model)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
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
        }

        public override async Task DeleteAsync(PostInfo model)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
            {
                if (model != null)
                {
                    var entity = Context.PostInfo.Find(model.Id);
                    entity.IsDelete = true;
                    entity.PostStatus = PostStatus.删除;
                    await Context.SaveChangesAsync();
                    await base.DeleteAsync(model);
                }
            }
        }


        public List<PostInfo> GetRecentPost(int count)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
            {
                var res = Context.PostInfo.OrderByDescending(x => x.Id).Take(count).Select(x => x.Id).ToList();
                if (res != null && res.Count > 0)
                {
                    List<PostInfo> list = new List<PostInfo>();
                    foreach (var item in res)
                    {
                        string key = RedisKeyHelper.GetPostKey(item);
                        Func<PostInfo> GetDb = () => GetFromDB(item);
                        PostInfo info = RedisHelper.GetEntity<PostInfo>(key, GetDb);
                        list.Add(info);
                    }
                    return list;
                }
                return new List<PostInfo>();
            }
        }

        public async Task<List<PostInfo>> GetRecentPostAsync(int count)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
            {
                Func<List<int>> GetIds = () =>
            {
                return Context.PostInfo.OrderByDescending(x => x.Id).Take(count).Select(x => x.Id).ToList();
            };
                //var res = Context.PostInfo.OrderByDescending(x => x.Id).Take(count).Select(x => x.Id).ToList();
                var res = await GetIds.StartAsync<List<int>>();
                if (res != null && res.Count > 0)
                {
                    List<PostInfo> list = new List<PostInfo>();
                    foreach (var item in res)
                    {
                        string key = RedisKeyHelper.GetPostKey(item);
                        Func<PostInfo> GetDb = () => GetFromDB(item);
                        PostInfo info = RedisHelper.GetEntity<PostInfo>(key, GetDb);
                        list.Add(info);
                    }
                    return list;
                }
                return new List<PostInfo>();
            }
        }

        public override async Task InsertAsync(PostInfo model, int userid)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
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
                PostMetaService metaservice = new PostMetaService();
                metaservice.Insert(model.PostMetasInfos, entity.Id);
                await base.InsertAsync(model, userid);
            }
        }

        public override void Insert(PostInfo model, int userid)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
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

                IPostMetaService metaservice = new PostMetaService();
                metaservice.Insert(model.PostMetasInfos, entity.Id);
                base.Insert(model, userid);
            }
        }


        public override void Update(PostInfo model)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
            {
                var entity = GetFromDB(model.Id);
                entity.CommentCount = model.CommentCount;
                entity.Title = model.Title;
                entity.Content = model.Content;
                entity.PostStatus = model.PostStatus;
                entity.PostCommentStatus = model.PostCommentStatus;
                entity.EditedTime = DateTime.Now;
                Context.SaveChanges();
                IPostMetaService metaservice = new PostMetaService();
                metaservice.Update(model.PostMetasInfos, model.Id);
                base.Update(model);
            }
        }

        public override async Task UpdateAsync(PostInfo model)
        {
            await Common.ThreadHelper.StartAsync(() =>
            {
                Update(model);
            });
            //var entity = Context.PostInfo.Find(model.Id);
            //entity.CommentCount = model.CommentCount;
            //entity.Title = model.Title;
            //entity.Content = model.Content;
            //entity.PostStatus = model.PostStatus;
            //entity.PostCommentStatus = model.PostCommentStatus;
            //entity.EditedTime = DateTime.Now;
            //string key = RedisKeyHelper.GetPostKey(model.Id);
            //RedisHelper.DeleteEntity(key);
            //await SaveChanges();
            //await base.UpdateAsync(model);
        }
      

        public override string GetModelKey(int id)
        {
            return RedisKeyHelper.GetPostKey(id);
        }

        public IEnumerable<DateTime> GetPostMonthInfos()
        {
            using (MVCBlogContext Context = new MVCBlogContext())
            {
                var datetimes = Context.PostInfo.Select(x => x.CreateTime).ToList();
                return datetimes;
            }
        }
        public override PostInfo GetFromDB(int id)
        {
            PostInfo info = base.GetFromDB(id);
            SetPostMetaInfo(info);
            return info;
        }
        public void SetPostMetaInfo(PostInfo info)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
            {
                if (info != null)
                {
                    var metarelations = Context.PostMetaRelation.Where(x => x.PostId == info.Id && !x.IsDelete).ToList();
                    if (metarelations.Count > 0)
                    {
                        var metaids = metarelations.Select(x => x.PostMetaId);
                        var metas = new PostMetaService().Query(x => metaids.Contains(x.Id) && !x.IsDelete);
                        info.PostMetasInfos = metas;
                    }
                }
            }
        }

        //public Pagination<PostInfo> Query(Expression<Func<PostInfo, bool>> query, int index, int pagecount)
        //{
        //    // System.Linq.Expressions.Expression<Func<PostInfo, bool>> ex = s => s.IsDelete == true;
        //    var res = Context.PostInfo.Where(query).Select(x => x.Id).OrderByDescending(x => x).ToPagedList(index, pagecount);
        //    if (res != null && res.Count > 0)
        //    {
        //        List<PostInfo> list = new List<PostInfo>();
        //        foreach (var item in res)
        //        {
        //            string key = RedisKeyHelper.GetPostKey(item);
        //            Func<PostInfo> GetDb = () => Context.PostInfo.Find(item);
        //            PostInfo info = RedisHelper.GetEntity<PostInfo>(key, GetDb);
        //            list.Add(info);
        //        }
        //        Pagination<PostInfo> pagination = new Pagination<PostInfo>()
        //        {
        //            Items = list,
        //            TotalItemCount = res.TotalItemCount,
        //            PageCount = res.PageCount,
        //            PageNumber = res.PageNumber,
        //            PageSize = res.PageSize
        //        };
        //    }
        //    return new Pagination<PostInfo>()
        //    {
        //        Items = null,
        //        TotalItemCount = 0,
        //        PageCount = 0,
        //        PageSize = pagecount,
        //        PageNumber = index
        //    };
        //}

        //public override PostInfo GetById(int id)
        //{
        //    string key = GetModelKey(id);
        //    Func<PostInfo> GetEntity = () => GetFromDB(id);
        //    return RedisHelper.GetEntity<PostInfo>(key, GetEntity);
        //}

        //public override PostInfo GetFromDB(int id)
        //{
        //    return Context.PostInfo.Find(id);
        //}

        //public override Pagination<PostInfo> Query(int index, int pagecount, Expression<Func<PostInfo, bool>> query = null)
        //{
        //    var ids = query != null ? Context.PostInfo.Where(query).OrderByDescending(x => x.Id).Select(x => x.Id).ToPagedList(index, pagecount) : Context.PostInfo.OrderByDescending(x => x.Id).Select(x => x.Id).ToPagedList(index, pagecount);
        //    if (ids != null && ids.Count() > 0)
        //    {

        //        Pagination<PostInfo> pagination = new Pagination<PostInfo>()
        //        {
        //            Items = GetByIds(ids),
        //            TotalItemCount = ids.TotalItemCount,
        //            PageCount = ids.PageCount,
        //            PageNumber = ids.PageNumber,
        //            PageSize = ids.PageSize
        //        };
        //        return pagination;
        //    }
        //    else
        //    {
        //        return new Pagination<PostInfo>()
        //        {
        //            Items = null,
        //            TotalItemCount = 0,
        //            PageCount = 0,
        //            PageNumber = index,
        //            PageSize = pagecount
        //        };
        //    }
        //}

        //public override IEnumerable<PostInfo> Query(Expression<Func<PostInfo, bool>> query = null)
        //{
        //    var ids = query != null ? Context.PostInfo.Where(query).OrderByDescending(x => x.Id).Select(x => x.Id).ToList() : Context.PostInfo.OrderByDescending(x => x.Id).Select(x => x.Id).ToList();
        //    if (ids != null && ids.Count > 0)
        //    {
        //        return GetByIds(ids);
        //    }
        //    else
        //    {
        //        return new List<PostInfo>();
        //    }
        //}

        //public override IEnumerable<PostInfo> GetByIds(IEnumerable<int> ids)
        //{
        //    List<PostInfo> list = new List<PostInfo>();
        //    foreach (int id in ids)
        //    {
        //        var item = GetById(id);
        //        list.Add(item);
        //    }
        //    return list;
        //}
    }
}
