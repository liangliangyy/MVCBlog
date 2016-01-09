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
        public CategoryService(MVCBlogContext _contest)
        {
            this.Context = _contest;
        }

        public void AddCategoryInfo(CategoryInfo info)
        {
            RedisHelper.DeleteEntity(ConfigInfo.GetCategoryKey);
            Context.CategoryInfo.Add(info);
            Context.SaveChanges();
        }

        public List<CategoryInfo> GetCategoryList()
        {
            Func<List<CategoryInfo>> GetDb = () => Context.CategoryInfo.ToList();
            return RedisHelper.GetEntity<List<CategoryInfo>>(ConfigInfo.GetCategoryKey, GetDb);
        }
    }
}
