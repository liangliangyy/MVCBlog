using MVCBlog.Common;
using PagedList;
using MVCBlog.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MVCBlog.Entities;
using System.Linq.Expressions;
using MVCBlog.Repository;
using MVCBlog.CacheManager;
using System.Collections;
using MVCBlog.Entities.Models;

namespace MVCBlog.Service
{
    public abstract class BaseService<T> : IBase<T> where T : BaseModel
    {
        public event EventHandler<ModelCacheEventArgs> ModelCreateEventHandler;
        public event EventHandler<ModelCacheEventArgs> ModelDeleteEventHandler;
        public event EventHandler<ModelCacheEventArgs> ModelUpdateEventHandler;




        private void Service_ModelCreateEventHandler(T model)
        {
            if (ModelCreateEventHandler != null)
            {
                ModelCacheEventArgs e = GetEventArgs(model);
                ModelCreateEventHandler(model, e);
            }
        }
        private void Service_ModelDeleteEventHandler(T model)
        {
            if (ModelDeleteEventHandler != null)
            {
                ModelCacheEventArgs e = GetEventArgs(model);
                ModelDeleteEventHandler(model, e);
            }
        }
        private void Service_ModelUpdateEventHandler(T model)
        {
            if (ModelUpdateEventHandler != null)
            {
                ModelCacheEventArgs e = GetEventArgs(model);
                ModelUpdateEventHandler(model, e);
            }
        }
        [ModelHandlerAttribute(ModelModifyType.Delete)]
        public virtual void Delete(T model)
        {
            Service_ModelDeleteEventHandler(model);
        }
        [ModelHandlerAttribute(ModelModifyType.Delete)]
        public async virtual Task DeleteAsync(T model)
        {
            await Common.ThreadHelper.StartAsync(() => { Service_ModelDeleteEventHandler(model); });
        }

        //public virtual T GetById(int id)
        //{
        //    string key = GetModelKey(id);
        //    Func<T> GetDb = () => GetFromDB(id);
        //    return RedisHelper.GetEntity<T>(key, GetDb);
        //}


        //public virtual T GetFromDB(int id)
        //{
        //    return Context.Set<T>().Find(id);
        //}

        [ModelHandlerAttribute(ModelModifyType.Create)]
        public virtual void Insert(T model, int userid = 0)
        {
            Service_ModelCreateEventHandler(model);
        }
        [ModelHandlerAttribute(ModelModifyType.Create)]
        public async virtual Task InsertAsync(T model, int userid = 0)
        {
            await Common.ThreadHelper.StartAsync(() => { Service_ModelCreateEventHandler(model); });
        }

        //public abstract Task<int> SaveChanges();

        [ModelHandlerAttribute(ModelModifyType.Update)]
        public virtual void Update(T model)
        {
            Service_ModelUpdateEventHandler(model);
        }
        [ModelHandlerAttribute(ModelModifyType.Update)]
        public async virtual Task UpdateAsync(T model)
        {
            await Common.ThreadHelper.StartAsync(() => { Service_ModelUpdateEventHandler(model); });
        }

        public abstract string GetModelKey(int id);

        public virtual ModelCacheEventArgs GetEventArgs(T model)
        {
            var prop = model.GetType().GetProperty("id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            int id = prop == null ? 0 : (int)prop.GetValue(model);
            ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = GetModelKey(model.Id), ID = id };
            return e;
        }

        //  public abstract T GetById(int id);
        public virtual T GetFromDB(int id)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
            {
                return Context.Set<T>().Find(id);
            }
        }
        // public abstract Pagination<T> Query(int index, int pagecount, Expression<Func<T, bool>> query = null);
        //  public abstract IEnumerable<T> Query(Expression<Func<T, bool>> query = null);
        //public abstract IEnumerable<T> GetByIds(IEnumerable<int> ids);

        //public virtual async Task<Pagination<T>> Query(int index, int pagecount, Expression<Func<T, bool>> query = null)
        //{
        //    var ids = await Common.ThreadHelper.StartAsync(() =>
        //    {
        //        return query != null ? Context.Set<T>().Where(query).OrderByDescending(x => x.Id).Select(x => x.Id).ToPagedList(index, pagecount) : Context.Set<T>().OrderByDescending(x => x.Id).Select(x => x.Id).ToPagedList(index, pagecount);
        //    });
        //    if (ids != null && ids.Count > 0)
        //    {

        //        Pagination<T> pagination = new Pagination<T>()
        //        {
        //            Items = await Common.ThreadHelper.StartAsync(() => GetByIds(ids)),
        //            TotalItemCount = ids.TotalItemCount,
        //            PageCount = ids.PageCount,
        //            PageNumber = ids.PageNumber,
        //            PageSize = ids.PageSize
        //        };
        //        return pagination;
        //    }
        //    else
        //    {
        //        return new Pagination<T>()
        //        {
        //            Items = null,
        //            TotalItemCount = 0,
        //            PageCount = 0,
        //            PageNumber = index,
        //            PageSize = pagecount
        //        };
        //    }


        //}

        //public async Task<IEnumerable<T>> Query(Expression<Func<T, bool>> query = null)
        //{
        //    var ids = await Common.ThreadHelper.StartAsync(() =>
        //    {
        //        return query != null ? Context.Set<T>().Where(query).OrderByDescending(x => x.Id).Select(x => x.Id).ToList() : Context.Set<T>().OrderByDescending(x => x.Id).Select(x => x.Id).ToList();
        //    });
        //    if (ids != null && ids.Count > 0)
        //    {
        //        return await Common.ThreadHelper.StartAsync(() => GetByIds(ids));
        //    }
        //    else
        //    {
        //        return new List<T>();
        //    }

        //}
        public T GetById(int id)
        {
            string key = GetModelKey(id);
            Func<T> GetDb = () => GetFromDB(id);
            return RedisHelper.GetEntity<T>(key, GetDb);
        }

        public IEnumerable<T> GetByIds(IEnumerable<int> ids)
        {
            foreach (int id in ids)
            {
                yield return GetById(id);
            }
            //List<T> list = new List<T>();
            //foreach (int id in ids)
            //{
            //    var item = GetById(id);
            //    list.Add(item);
            //}
            //return list;
        }

        public Pagination<T> Query(int index, int pagecount, Expression<Func<T, bool>> query)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
            {
                var ids = query != null ? Context.Set<T>().Where(query).OrderByDescending(x => x.Id).Select(x => x.Id).ToPagedList(index, pagecount) : Context.Set<T>().OrderByDescending(x => x.Id).Select(x => x.Id).ToPagedList(index, pagecount);
                if (ids != null && ids.Count > 0)
                {

                    Pagination<T> pagination = new Pagination<T>()
                    {
                        Items = GetByIds(ids),
                        TotalItemCount = ids.TotalItemCount,
                        PageCount = ids.PageCount,
                        PageNumber = ids.PageNumber,
                        PageSize = ids.PageSize
                    };
                    return pagination;
                }
                else
                {
                    return new Pagination<T>()
                    {
                        Items = null,
                        TotalItemCount = 0,
                        PageCount = 0,
                        PageNumber = index,
                        PageSize = pagecount
                    };
                }
            }
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> query)
        {
            using (MVCBlogContext Context = new MVCBlogContext())
            {
                var ids = query != null ? Context.Set<T>().Where(query).OrderByDescending(x => x.Id).Select(x => x.Id).ToList() : Context.Set<T>().OrderByDescending(x => x.Id).Select(x => x.Id).ToList();
                if (ids != null && ids.Count > 0)
                {
                    return GetByIds(ids);
                }
                else
                {
                    return new List<T>();
                }
            }
        }
    }
}
