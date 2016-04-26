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
    public class CategoryService : BaseService<CategoryInfo>, ICategoryService
    {
        //private MVCBlogContext Context;

        //public CategoryService(MVCBlogContext _context)
        //{
        //    this.Context = _context;
        //}


        public override void Delete(CategoryInfo model)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
            {
                var entity = Context.CategoryInfo.Find(model.Id);
                entity.IsDelete = true;
                Context.SaveChanges();
                base.Delete(model);
            }
        }

        public override async Task DeleteAsync(CategoryInfo model)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
            {
                var entity = Context.CategoryInfo.Find(model.Id);
                entity.IsDelete = true;
                await Context.SaveChangesAsync(); 
                await base.DeleteAsync(model);
            }
        }


        //public override async Task<CategoryInfo> GetByIdAsync(int id)
        //{
        //    Func<CategoryInfo> getitem = () =>
        //    {
        //        var list = RedisHelper.GetEntity<List<CategoryInfo>>(RedisKeyHelper.GetCategoryKey(id));
        //        if (list != null)
        //        {
        //            return list.Find(x => x.Id == id);
        //        }
        //        return Context.CategoryInfo.Find(id);
        //    };
        //    return await Common.TaskExtensions.WithCurrentCulture<CategoryInfo>(getitem);
        //}


        public override void Insert(CategoryInfo model, int userid = 0)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
            {
                model.CreateUser = Context.UserInfo.Find(userid == 0 ? model.CreateUser.Id : userid);
                Context.CategoryInfo.Add(model);
                Context.SaveChanges();
                base.Insert(model, userid);
            }
        }

        public override async Task InsertAsync(CategoryInfo model, int userid)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
            {
                model.CreateUser = Context.UserInfo.Find(userid == 0 ? model.CreateUser.Id : userid);
                Context.CategoryInfo.Add(model);
                await Context.SaveChangesAsync();
                await base.InsertAsync(model, userid);
            }
        }


        public override void Update(CategoryInfo model)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
            {
                var entity = Context.CategoryInfo.Find(model.Id);
                if (entity != null)
                {
                    entity.CategoryName = model.CategoryName;
                    entity.IsDelete = model.IsDelete;
                    Context.SaveChanges();
                    base.Update(model);
                }
            }
        }

        public override async Task UpdateAsync(CategoryInfo model)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
            {
                var entity = Context.CategoryInfo.Find(model.Id);
                if (entity != null)
                {
                    entity.CategoryName = model.CategoryName;
                    entity.IsDelete = model.IsDelete;
                    await Context.SaveChangesAsync();
                    await base.UpdateAsync(model);
                }
            }
        }
       

        public override string GetModelKey(int id)
        {
            return RedisKeyHelper.GetCategoryKey(id);
        }

        //public override CategoryInfo GetById(int id)
        //{
        //    string key = GetModelKey(id);
        //    Func<CategoryInfo> GetEntity = () => GetFromDB(id);
        //    return RedisHelper.GetEntity<CategoryInfo>(key, GetEntity);
        //}

        //public override CategoryInfo GetFromDB(int id)
        //{
        //    return Context.CategoryInfo.Find(id);
        //}

        //public override Pagination<CategoryInfo> Query(int index, int pagecount, Expression<Func<CategoryInfo, bool>> query = null)
        //{
        //    var ids = query != null ? Context.CategoryInfo.Where(query).OrderByDescending(x => x.Id).Select(x => x.Id).ToPagedList(index, pagecount) : Context.CategoryInfo.OrderByDescending(x => x.Id).Select(x => x.Id).ToPagedList(index, pagecount);
        //    if (ids != null && ids.Count() > 0)
        //    {

        //        Pagination<CategoryInfo> pagination = new Pagination<CategoryInfo>()
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
        //        return new Pagination<CategoryInfo>()
        //        {
        //            Items = null,
        //            TotalItemCount = 0,
        //            PageCount = 0,
        //            PageNumber = index,
        //            PageSize = pagecount
        //        };
        //    }
        //}

        //public override IEnumerable<CategoryInfo> Query(Expression<Func<CategoryInfo, bool>> query = null)
        //{
        //    var ids = query != null ? Context.CategoryInfo.Where(query).OrderByDescending(x => x.Id).Select(x => x.Id).ToList() : Context.CategoryInfo.OrderByDescending(x => x.Id).Select(x => x.Id).ToList();
        //    if (ids != null && ids.Count > 0)
        //    {
        //        return GetByIds(ids);
        //    }
        //    else
        //    {
        //        return new List<CategoryInfo>();
        //    }
        //}

        //public override IEnumerable<CategoryInfo> GetByIds(IEnumerable<int> ids)
        //{
        //    foreach (int id in ids)
        //    {
        //        yield return GetById(id);
        //    }
        //}
    }
}
