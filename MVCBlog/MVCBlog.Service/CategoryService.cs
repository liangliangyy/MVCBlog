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
    public class CategoryService : ICategoryService
    {
        private MVCBlogContext Context;

        public event EventHandler<ModelCacheEventArgs> ModelDeleteEventHandler;
        public event EventHandler<ModelCacheEventArgs> ModelCreateEventHandler;
        public event EventHandler<ModelCacheEventArgs> ModelUpdateEventHandler;

        public CategoryService(MVCBlogContext _contest)
        {
            this.Context = _contest;
        }


        public void Delete(CategoryInfo model)
        {
            var entity = Context.CategoryInfo.Find(model.Id);
            entity.IsDelete = true;
            Context.SaveChanges();
            if (ModelDeleteEventHandler != null)
            {
                ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetCategoryKey(model.Id), ID = model.Id };
                ModelDeleteEventHandler(this, e);
            }
        }

        public async Task DeleteAsync(CategoryInfo model)
        {
            var entity = Context.CategoryInfo.Find(model.Id);
            entity.IsDelete = true;
            await SaveChanges();
            if (ModelDeleteEventHandler != null)
            {
                ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetCategoryKey(model.Id), ID = model.Id };
                ModelDeleteEventHandler(this, e);
            }
        }

        public CategoryInfo GetById(int id)
        {
            var list = RedisHelper.GetEntity<List<CategoryInfo>>(ConfigInfo.GetCategoryKey(id));
            if (list != null)
            {
                return list.Find(x => x.Id == id);
            }
            return Context.CategoryInfo.Find(id);
        }

        public async Task<CategoryInfo> GetByIdAsync(int id)
        {
            Func<CategoryInfo> getitem = () =>
            {
                var list = RedisHelper.GetEntity<List<CategoryInfo>>(ConfigInfo.GetCategoryKey(id));
                if (list != null)
                {
                    return list.Find(x => x.Id == id);
                }
                return Context.CategoryInfo.Find(id);
            };
            return await Common.TaskExtensions.WithCurrentCulture<CategoryInfo>(getitem);
        }

        public List<CategoryInfo> GetCategoryList()
        {
            List<CategoryInfo> list = new List<CategoryInfo>();
             var categoryids = Context.CategoryInfo.Select(x => x.Id).ToList();
            foreach (int id in categoryids)
            {
                Func<CategoryInfo> GetDb = () => Context.CategoryInfo.Find(id);
                string key = ConfigInfo.GetCategoryKey(id);
                CategoryInfo info = RedisHelper.GetEntity<CategoryInfo>(key, GetDb);
                list.Add(info);
            }
            return list;
        }

        public async Task<List<CategoryInfo>> GetCategoryListAsync()
        {
            List<CategoryInfo> list = new List<CategoryInfo>();
            var categoryids = Context.CategoryInfo.Select(x => x.Id).ToList();
            foreach (int id in categoryids)
            {
                Func<CategoryInfo> GetDb = () => Context.CategoryInfo.Find(id);
                string key = ConfigInfo.GetCategoryKey(id);
                CategoryInfo info =await RedisHelper.GetEntityAsync<CategoryInfo>(key, GetDb);
                list.Add(info);
            }
            return list;
        }
        public void Insert(CategoryInfo model, int userid = 0)
        {
            model.CreateUser = Context.UserInfo.Find(userid == 0 ? model.CreateUser.Id : userid);
            Context.CategoryInfo.Add(model);
            Context.SaveChanges();
            if (ModelCreateEventHandler != null)
            {
                ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetCategoryKey(model.Id), ID = model.Id };
                ModelCreateEventHandler(this, e);
            }
        }

        public async Task InsertAsync(CategoryInfo model, int userid)
        {

            model.CreateUser = Context.UserInfo.Find(userid == 0 ? model.CreateUser.Id : userid);
            Context.CategoryInfo.Add(model);
            await SaveChanges();
            if (ModelCreateEventHandler != null)
            {
                ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetCategoryKey(model.Id), ID = model.Id };
                ModelCreateEventHandler(this, e);
            }
        }


        public void Update(CategoryInfo model)
        {
            var entity = Context.CategoryInfo.Find(model.Id);
            if (entity != null)
            {
                entity.CategoryName = model.CategoryName;
                entity.IsDelete = model.IsDelete;
                Context.SaveChanges();
                if (ModelUpdateEventHandler != null)
                {
                    ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetCategoryKey(model.Id), ID = model.Id };
                    ModelUpdateEventHandler(this, e);
                }
            }
        }

        public async Task UpdateAsync(CategoryInfo model)
        {
            var entity = Context.CategoryInfo.Find(model.Id);
            if (entity != null)
            {
                entity.CategoryName = model.CategoryName;
                entity.IsDelete = model.IsDelete;
                await SaveChanges();
                if (ModelUpdateEventHandler != null)
                {
                    ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetCategoryKey(model.Id), ID = model.Id };
                    ModelUpdateEventHandler(this, e);
                }
            }
        }
        public async Task<int> SaveChanges()
        {
            return await Context.SaveChangesAsync();
        }

        public CategoryInfo GetFromDB(int id)
        {
            return Context.CategoryInfo.Find(id);
        }
    }
}
