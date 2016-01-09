using System.Collections.Generic;
using MVCBlog.Entities.Models;
namespace MVCBlog.Service.Interfaces
{
    public interface IPostService
    {
        List<PostInfo> GetPosts();
        PostInfo GetById(int id);
        void Insert(PostInfo model);
        void Update(PostInfo model);
        void Delete(PostInfo model);
    }
}
