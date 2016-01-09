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
    public class CategoryInfo
    {
        public CategoryInfo()
        {
            CreateTime = DateTime.Now;
            IsDelete = false;
        }
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="分类名称不能为空")]
        public string CategoryName { get; set; }
        public UserInfo CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsDelete { get; set; }
    }
}
