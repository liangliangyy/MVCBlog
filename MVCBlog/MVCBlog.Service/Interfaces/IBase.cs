using MVCBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Service.Interfaces
{
    public interface IBase<T>
    {
        void Insert(T model, int userid = 0);
        Task InsertAsync(T model, int userid = 0);
        void Update(T model);
        Task UpdateAsync(T model);
        void Delete(T model);
        Task DeleteAsync(T model);
        Task<int> SaveChanges();
        T GetById(int id);
        T GetFromDB(int id);
        Task<T> GetByIdAsync(int id);
        string GetModelKey(int id);
        ModelCacheEventArgs GetEventArgs(T model);
        Task<Pagination<T>> Query(int index, int pagecount, Expression<Func<T, bool>> query=null);
        Task<IEnumerable<T>> Query(Expression<Func<T, bool>> query = null);
        IEnumerable<T> GetByIds(IEnumerable<int> ids);
        event EventHandler<ModelCacheEventArgs> ModelDeleteEventHandler;
        event EventHandler<ModelCacheEventArgs> ModelCreateEventHandler;
        event EventHandler<ModelCacheEventArgs> ModelUpdateEventHandler;
    }
}
