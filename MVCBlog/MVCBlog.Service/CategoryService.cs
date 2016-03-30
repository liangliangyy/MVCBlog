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
    public class CategoryService : BaseService<CategoryInfo>, ICategoryService
    {
        private MVCBlogContext Context;

        public CategoryService(MVCBlogContext _contest)
        {
            this.Context = _contest;
        }


        public override void Delete(CategoryInfo model)
        {
            var entity = Context.CategoryInfo.Find(model.Id);
            entity.IsDelete = true;
            Context.SaveChanges();
            base.Delete(model);
        }

        public override async Task DeleteAsync(CategoryInfo model)
        {
            var entity = Context.CategoryInfo.Find(model.Id);
            entity.IsDelete = true;
            await SaveChanges();
            await base.DeleteAsync(model);
        }

        public override CategoryInfo GetById(int id)
        {
            var list = RedisHelper.GetEntity<List<CategoryInfo>>(ConfigInfo.GetCategoryKey(id));
            if (list != null)
            {
                return list.Find(x => x.Id == id);
            }
            return Context.CategoryInfo.Find(id);
        }

        public override async Task<CategoryInfo> GetByIdAsync(int id)
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
                CategoryInfo info = await RedisHelper.GetEntityAsync<CategoryInfo>(key, GetDb);
                list.Add(info);
            }
            return list;
        }
        public override void Insert(CategoryInfo model, int userid = 0)
        {
            model.CreateUser = Context.UserInfo.Find(userid == 0 ? model.CreateUser.Id : userid);
            Context.CategoryInfo.Add(model);
            Context.SaveChanges();
            base.Insert(model, userid);
        }

        public override async Task InsertAsync(CategoryInfo model, int userid)
        {

            model.CreateUser = Context.UserInfo.Find(userid == 0 ? model.CreateUser.Id : userid);
            Context.CategoryInfo.Add(model);
            await SaveChanges();
            await base.InsertAsync(model, userid);
        }


        public override void Update(CategoryInfo model)
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

        public override async Task UpdateAsync(CategoryInfo model)
        {
            var entity = Context.CategoryInfo.Find(model.Id);
            if (entity != null)
            {
                entity.CategoryName = model.CategoryName;
                entity.IsDelete = model.IsDelete;
                await SaveChanges();
                await base.UpdateAsync(model);
            }
        }
        public override async Task<int> SaveChanges()
        {
            return await Context.SaveChangesAsync();
        }

        public override CategoryInfo GetFromDB(int id)
        {
            return Context.CategoryInfo.Find(id);
        }

        public override string GetModelKey(CategoryInfo model)
        {
            return ConfigInfo.GetCategoryKey(model.Id);
        }


    }
}
