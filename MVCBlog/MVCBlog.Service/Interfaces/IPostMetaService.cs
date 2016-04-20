using MVCBlog.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Service.Interfaces
{
    public interface IPostMetaService : IBase<PostMetasInfo>
    {
        void Insert(IEnumerable<PostMetasInfo> infos,int postid);
    }
}
