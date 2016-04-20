using MVCBlog.CacheManager;
using MVCBlog.Entities.Models;
using MVCBlog.Repository;
using MVCBlog.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Service
{
    public class PostMetaService : BaseService<PostMetasInfo>, IPostMetaService
    {
        private MVCBlogContext Context;
        public PostMetaService(MVCBlogContext _context)
        {
            this.Context = _context;
        }
        public override void Delete(PostMetasInfo model)
        {
            model.IsDelete = true;
            Context.SaveChanges();
        }

        public async override Task DeleteAsync(PostMetasInfo model)
        {
            model.IsDelete = true;
            await SaveChanges();
        }

        public override string GetModelKey(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(IEnumerable<PostMetasInfo> infos, int postid)
        {
            throw new NotImplementedException();
        }

        public override void Insert(PostMetasInfo model, int userid = 0)
        {
            throw new NotImplementedException();
        }

        public override Task InsertAsync(PostMetasInfo model, int userid = 0)
        {
            throw new NotImplementedException();
        }

        public override Task<int> SaveChanges()
        {
            throw new NotImplementedException();
        }

        public override void Update(PostMetasInfo model)
        {
            throw new NotImplementedException();
        }

        public override Task UpdateAsync(PostMetasInfo model)
        {
            throw new NotImplementedException();
        }
    }
}
