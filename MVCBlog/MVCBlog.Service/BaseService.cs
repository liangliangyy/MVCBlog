using MVCBlog.Common;
using MVCBlog.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Service
{
    public abstract class BaseService<T> : IBase<T>
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
            await Common.TaskExtensions.WithCurrentCulture(() => { Service_ModelDeleteEventHandler(model); });
        }

        public abstract T GetById(int id);

        public abstract Task<T> GetByIdAsync(int id);

        public abstract T GetFromDB(int id);
        [ModelHandlerAttribute(ModelModifyType.Create)]
        public virtual void Insert(T model, int userid = 0)
        {
            Service_ModelCreateEventHandler(model);
        }
        [ModelHandlerAttribute(ModelModifyType.Create)]
        public async virtual Task InsertAsync(T model, int userid = 0)
        {
            await Common.TaskExtensions.WithCurrentCulture(() => { Service_ModelCreateEventHandler(model); });
        }

        public abstract Task<int> SaveChanges();

        [ModelHandlerAttribute(ModelModifyType.Update)]
        public virtual void Update(T model)
        {
            Service_ModelUpdateEventHandler(model);
        }
        [ModelHandlerAttribute(ModelModifyType.Update)]
        public async virtual Task UpdateAsync(T model)
        {
            await Common.TaskExtensions.WithCurrentCulture(() => { Service_ModelUpdateEventHandler(model); });
        }

        public abstract string GetModelKey(T model);

        public virtual ModelCacheEventArgs GetEventArgs(T model)
        {
            var prop = model.GetType().GetProperty("id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            int id = prop == null ? 0 : (int)prop.GetValue(model);
            ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = GetModelKey(model), ID = id };
            return e;
        }
    }
}
