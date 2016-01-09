using System;
using System.Collections.Generic;
using System.Linq;
using MVCBlog.Entities.Models;
using MVCBlog.Service.Interfaces;
using MVCBlog.Repository;
using MVCBlog.Entities.Enums;
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
            if (model!=null)
            {
                var entity = Context.PostInfo.Find(model.Id);
                entity.IsDelete = true;
                entity.PostStatus = PostStatus.删除;
                Context.SaveChanges();
            }
        }

        public PostInfo GetById(int id)
        {
            var entity = Context.PostInfo.Find(id);
            return entity;
        }

        public List<PostInfo> GetPosts()
        {
            return Context.PostInfo.ToList();
            //return Context.PostInfo.Where(x => !x.IsDelete && x.PostStatus == PostStatus.发布).ToList();
        }

        public void Insert(PostInfo model)
        {
            Context.PostInfo.Add(model);
            Context.SaveChanges();
        }

        public void Update(PostInfo model)
        {
            var entity = Context.PostInfo.Find(model.Id);
            entity.Title = model.Title;
            entity.Content = model.Content;
            entity.PostStatus = model.PostStatus;
            entity.CommentStatus = model.CommentStatus;
            entity.EditedTime = DateTime.Now;
            Context.SaveChanges();
        }
    }
}
