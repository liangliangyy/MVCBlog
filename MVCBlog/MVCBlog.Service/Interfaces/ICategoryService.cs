using MVCBlog.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Service.Interfaces
{
    public interface ICategoryService : IBase<CategoryInfo>
    {
        List<CategoryInfo> GetCategoryList();
        Task<List<CategoryInfo>> GetCategoryListAsync();
        //void AddCategoryInfo(CategoryInfo info);
    }
}
