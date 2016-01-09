using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCBlog.Entities.Models
{
    [Table("CategoryRelationships")]
    public class CategoryRelationships
    {
        [Key]
        public int Id { get; set; }
        public virtual CategoryInfo CategoryInfo { get; set; }
        public virtual CategoryInfo ParentCategoryInfo { get; set; }
        public bool IsDelete { get; set; }
    }
}
