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
using MVCBlog.Entities;
using System.Linq.Expressions;
using PagedList;

namespace MVCBlog.Service
{
    public class CommentService : BaseService<CommentInfo>, ICommentService
    {
        private MVCBlogContext Context;
        public CommentService(MVCBlogContext _context) 
        {
            this.Context = _context;
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

        //public override CommentInfo GetById(int id)
        //{
        //    string key = GetModelKey(id);
        //    Func<CommentInfo> GetEntity = () => GetFromDB(id);
        //    return RedisHelper.GetEntity<CommentInfo>(key, GetEntity);
        //}

        //public override IEnumerable<CommentInfo> GetByIds(IEnumerable<int> ids)
        //{
        //    foreach (int id in ids)
        //    {
        //        yield return GetById(id);
        //    }
        //}

        //public override CommentInfo GetFromDB(int id)
        //{
        //    return Context.CommentInfo.Find(id);
        //}

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

        //public override IEnumerable<CommentInfo> Query(Expression<Func<CommentInfo, bool>> query = null)
        //{
        //    var ids = query != null ? Context.CommentInfo.Where(query).OrderByDescending(x => x.Id).Select(x => x.Id).ToList() : Context.CommentInfo.OrderByDescending(x => x.Id).Select(x => x.Id).ToList();
        //    if (ids != null && ids.Count > 0)
        //    {
        //        return GetByIds(ids);
        //    }
        //    else
        //    {
        //        return new List<CommentInfo>();
        //    }
        //}

        //public override Pagination<CommentInfo> Query(int index, int pagecount, Expression<Func<CommentInfo, bool>> query = null)
        //{
        //    var ids = query != null ? Context.CommentInfo.Where(query).OrderByDescending(x => x.Id).Select(x => x.Id).ToPagedList(index, pagecount) : Context.CommentInfo.OrderByDescending(x => x.Id).Select(x => x.Id).ToPagedList(index, pagecount);
        //    if (ids != null && ids.Count() > 0)
        //    {

        //        Pagination<CommentInfo> pagination = new Pagination<CommentInfo>()
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
        //        return new Pagination<CommentInfo>()
        //        {
        //            Items = null,
        //            TotalItemCount = 0,
        //            PageCount = 0,
        //            PageNumber = index,
        //            PageSize = pagecount
        //        };
        //    }
        //}

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
