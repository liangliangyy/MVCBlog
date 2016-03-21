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

        public void AddCategoryInfo(CategoryInfo info)
        {
            RedisHelper.DeleteEntity(ConfigInfo.CategoryKey);
            info.CreateUser = Context.UserInfo.Find(info.CreateUser.Id);
            Context.CategoryInfo.Add(info);
            Context.SaveChanges();
        }

        public void Delete(CategoryInfo model)
        {
            var entity = Context.CategoryInfo.Find(model.Id);
            entity.IsDelete = true;
            Context.SaveChanges();
        }

        public async Task DeleteAsync(CategoryInfo model)
        {
            var entity = Context.CategoryInfo.Find(model.Id);
            entity.IsDelete = true;
            await SaveChanges();
        }

        public CategoryInfo GetById(int id)
        {
            var list = RedisHelper.GetEntity<List<CategoryInfo>>(ConfigInfo.CategoryKey);
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
                var list = RedisHelper.GetEntity<List<CategoryInfo>>(ConfigInfo.CategoryKey);
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
            Func<List<CategoryInfo>> GetDb = () => Context.CategoryInfo.ToList();
            return RedisHelper.GetEntity<List<CategoryInfo>>(ConfigInfo.CategoryKey, GetDb);
        }

        public async Task<List<CategoryInfo>> GetCategoryListAsync()
        {
            Func<List<CategoryInfo>> GetDb = () => Context.CategoryInfo.ToList();
            return await RedisHelper.GetEntityAsync<List<CategoryInfo>>(ConfigInfo.CategoryKey, GetDb);
        }
        public void Insert(CategoryInfo model, int userid = 0)
        {
            RedisHelper.DeleteEntity(ConfigInfo.CategoryKey);
            model.CreateUser = Context.UserInfo.Find(userid == 0 ? model.CreateUser.Id : userid);
            Context.CategoryInfo.Add(model);
            Context.SaveChanges();
        }

        public async Task InsertAsync(CategoryInfo model, int userid)
        {
            RedisHelper.DeleteEntity(ConfigInfo.CategoryKey);
            model.CreateUser = Context.UserInfo.Find(userid == 0 ? model.CreateUser.Id : userid);
            Context.CategoryInfo.Add(model);
            await SaveChanges();
        }


        public void Update(CategoryInfo model)
        {
            var entity = Context.CategoryInfo.Find(model.Id);
            if (entity != null)
            {
                entity.CategoryName = model.CategoryName;
                entity.IsDelete = model.IsDelete;
                Context.SaveChanges();
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
            }
        }
        public async Task<int> SaveChanges()
        {
            return await Context.SaveChangesAsync();
        }

    }
}
