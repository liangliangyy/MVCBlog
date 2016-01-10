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

namespace MVCBlog.Service
{
    public class PostService : IPostService
    {
        private MVCBlogContext Context;
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
            }
        }

        public PostInfo GetById(int id)
        {
            Func<PostInfo> GetDb = () => Context.PostInfo.Find(id);
            string key = ConfigInfo.GetPostKey(id);
            PostInfo info = RedisHelper.GetEntity<PostInfo>(key, GetDb);
            return info;
        }

        public List<PostInfo> GetPosts()
        {
            return Context.PostInfo.ToList();
            //return Context.PostInfo.Where(x => !x.IsDelete && x.PostStatus == PostStatus.发布).ToList();
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

        public void Insert(PostInfo model)
        {
            model.PostStatus = PostStatus.发布;
            model.PostType = PostType.文章;
            model.PostCommentStatus = PostCommentStatus.打开;
            model.CommentCount = 0;
            model.CreateTime = DateTime.Now;
            model.IsDelete = false;
            var entity = Context.PostInfo.Add(model);
            Context.SaveChanges();
            string key = ConfigInfo.GetPostKey(entity.Id);
            RedisHelper.SetEntity<PostInfo>(key, entity);
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

        public void Update(PostInfo model)
        {
            var entity = Context.PostInfo.Find(model.Id);
            entity.Title = model.Title;
            entity.Content = model.Content;
            entity.PostStatus = model.PostStatus;
            entity.PostCommentStatus = model.PostCommentStatus;
            entity.EditedTime = DateTime.Now;
            Context.SaveChanges();
        }
    }
}
