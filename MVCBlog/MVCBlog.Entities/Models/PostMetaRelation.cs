using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Entities.Models
{
    public class PostMetaRelation : BaseModel
    {
        public int PostId { get; set; }
        public int PostMetaId { get; set; }
    }
}
