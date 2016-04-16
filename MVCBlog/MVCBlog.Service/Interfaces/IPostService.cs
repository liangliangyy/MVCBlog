using System.Collections.Generic;
using System;
using MVCBlog.Entities.Models;
using PagedList;
using MVCBlog.Entities;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace MVCBlog.Service.Interfaces
{
    public interface IPostService : IBase<PostInfo>
    {
        List<PostInfo> GetRecentPost(int count);
        Task<List<PostInfo>> GetRecentPostAsync(int count);
        IEnumerable<DateTime> GetPostMonthInfos();
    }
}
