using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MVCBlog.Entities.Enums;

namespace MVCBlog.Entities.Models
{
    [Table("CommentInfo")]
    public class CommentInfo : BaseModel
    {
        public CommentInfo()
        {
            this.CreateTime = DateTime.Now;
            this.IsDelete = false;
        }
      
        [Required]
        public virtual UserInfo CommentUser { get; set; }
    
        [Required]
        public int PostID { get; set; }
        
        [Required]
        public string CommentTitle { get; set; }
        [Required]
        public string CommentContent { get; set; }
      
    }
}
