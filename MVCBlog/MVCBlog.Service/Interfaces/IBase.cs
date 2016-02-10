using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Service.Interfaces
{
    public interface IBase<T>
    {
        void Insert(T model, int userid);
        Task InsertAsync(T model, int userid); 
        void Update(T model);
        Task UpdateAsync(T model); 
        void Delete(T model);
        Task DeleteAsync(T model);
        Task<int> SaveChanges(); 
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
    }
}
