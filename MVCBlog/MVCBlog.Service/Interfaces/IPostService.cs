using System.Collections.Generic;
using MVCBlog.Entities.Models;
using PagedList;
using MVCBlog.Entities;
using System.Threading.Tasks;

namespace MVCBlog.Service.Interfaces
{
    public interface IPostService : IBase<PostInfo>
    {
        Pagination<PostInfo> PostPagination(int index, int pagecount);
        Task<Pagination<PostInfo>> PostPaginationAsync(int index, int pagecount);
        List<PostInfo> GetRecentPost(int count);
        Task<List<PostInfo>> GetRecentPostAsync(int count);
        Pagination<PostInfo> GetUserPosts(int authorid, int index, int pagecount);
        Task<Pagination<PostInfo>> GetUserPostsAsync(int authorid, int index, int pagecount);
    }
}
