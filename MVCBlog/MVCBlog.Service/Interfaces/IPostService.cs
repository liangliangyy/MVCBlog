using System.Collections.Generic;
using MVCBlog.Entities.Models;
using PagedList;
using MVCBlog.Entities;

namespace MVCBlog.Service.Interfaces
{
    public interface IPostService
    {
        List<PostInfo> GetPosts();
        PostInfo GetById(int id);
        void Insert(PostInfo model,int userid);
        void Update(PostInfo model);
        void Delete(PostInfo model);
        Pagination<PostInfo> PostPagination(int index,int pagecount);

        List<PostInfo> GetRecentPost(int count);
    }
}
