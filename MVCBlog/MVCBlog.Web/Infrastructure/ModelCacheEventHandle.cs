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

        public void ModelDeleteEventHandler<IService, TModel>(object sender, Service.ModelCacheEventArgs e)
        {
            string key = e.Key;
            RedisHelper.DeleteEntity(key);
            log4net.LogManager.GetLogger("info").Info(string.Format("Model Delete. Service:{0}.ID:{1},KEY:{2}", typeof(IService).Name, e.ID, e.Key));
        }

        public void ModelCreateEventHandler<IService, TModel>(object sender, Service.ModelCacheEventArgs e)
        {
            string key = e.Key;
            int id = e.ID;
            IBase<TModel> service = (IBase<TModel>)ApplicationContainer.Container.Resolve<IService>();
            TModel model = service.GetFromDB(id);
            RedisHelper.SetEntity(key, model);
            log4net.LogManager.GetLogger("info").Info(string.Format("Model Create. Service:{0}.ID:{1},KEY:{2}", typeof(IService).Name, e.ID, e.Key));
        }

        public void ModelUpdateEventHandler<IService, TModel>(object sender, Service.ModelCacheEventArgs e)
        {
            string key = e.Key;
            RedisHelper.DeleteEntity(key);
            log4net.LogManager.GetLogger("info").Info(string.Format("Model Update. Service:{0}.ID:{1},KEY:{2}", typeof(IService).Name, e.ID, e.Key));
        }

    }
}
