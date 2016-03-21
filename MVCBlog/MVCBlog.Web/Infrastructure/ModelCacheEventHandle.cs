using MVCBlog.CacheManager;
using MVCBlog.Entities.Models;
using MVCBlog.Service.Interfaces;
using MVCBlog.Web.CommonHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
namespace MVCBlog.Web.Infrastructure
{
    public class ModelCacheEventHandle
    {
   
        public void ModelDeleteEventHandler(object sender, Service.ModelCacheEventArgs e)
        {
            string key = e.Key;
            RedisHelper.DeleteEntity(key);
        }

        public void ModelCreateEventHandler<IService,TModel>(object sender, Service.ModelCacheEventArgs e)
        {
            string key = e.Key;
            int id = e.ID;
            IBase<TModel> service = (IBase<TModel>)ApplicationContainer.Container.Resolve<IService>();
            TModel model = service.GetByIdAsync(id).Result;
            RedisHelper.SetEntity(key, model);
        }

    }
}
