using MVCBlog.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Service.Interfaces
{
   public interface ICommentService
    {
        List<CommentInfo> CommentList(int postid);
        void AddCommentInfo(CommentInfo entity);
    }
}
