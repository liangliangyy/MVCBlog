using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Entities.Models
{
    public class PostMetasInfo
    {
        public int Id { get; set; }
        public PostInfo PostInfo { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsDelete { get; set; }
    }
}
