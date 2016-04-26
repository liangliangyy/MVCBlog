using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Entities.Models
{
    public class BaseModel
    {
        public BaseModel()
        {
            IsDelete = false;
            CreateTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? EditedTime { get; set; }
    }
}
