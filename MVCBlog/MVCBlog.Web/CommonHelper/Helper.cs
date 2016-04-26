using Autofac;
using MVCBlog.Entities.Models;
using MVCBlog.Service;
using MVCBlog.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace MVCBlog.Web.CommonHelper
{
    public class Helper
    {
        public static List<SelectListItem> ParseEnumToSelectList<T>()
        {
            if (typeof(T).IsEnum)
            {
                return Enum.GetValues(typeof(T)).Cast<T>().Select(x => new SelectListItem()
                {
                    Text = x.ToString(),
                    Value = (Convert.ToInt32(x)).ToString()
                }).ToList();
            }
            else
            {
                throw new NotSupportedException("type is not enum");
            }
        }
        public static async Task<List<SelectListItem>> GetCategorySelectList()
        {
            ICategoryService service = ApplicationContainer.Container.Resolve<ICategoryService>();
            IEnumerable<CategoryInfo> list = service.Query();
            if (list.Count() == 0)
            {
                IUserService userservice = ApplicationContainer.Container.Resolve<IUserService>();
                var loginuser = UserHelper.GetLogInUserInfo();
                var defaultcategory = new CategoryInfo()
                {
                    Id = 0,
                    CategoryName = "未分类",
                    CreateTime = DateTime.Now,
                    CreateUser = loginuser,
                    IsDelete = false
                };
                service.Insert(defaultcategory, 0);
                return await GetCategorySelectList();
            }
            var res = list.Select(x => new SelectListItem() { Text = x.CategoryName, Value = x.Id.ToString() }).ToList();
            return res;
        }
    }
}
