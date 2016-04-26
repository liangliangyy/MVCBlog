using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCBlog.Entities.Models
{
    [Table("CategoryInfo")]
    public class CategoryInfo : BaseModel
    {
        public CategoryInfo()
        {
            CreateTime = DateTime.Now;
            IsDelete = false;
        }

        [Required(ErrorMessage = "分类名称不能为空")]
        public string CategoryName { get; set; }
        public virtual UserInfo CreateUser { get; set; }

    }
}
