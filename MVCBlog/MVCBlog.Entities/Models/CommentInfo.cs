using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MVCBlog.Entities.Enums;

namespace MVCBlog.Entities.Models
{
    [Table("CommentInfo")]
    public class CommentInfo
    {
        public CommentInfo()
        {
            this.CreateTime = DateTime.Now;
            this.IsDelete = false;
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public virtual UserInfo CommentUser { get; set; }
        public virtual DateTime CreateTime { get; set; }
        public virtual PostInfo CommentPost { get; set; }
        public virtual CommentInfo ParentCommentInfo { get; set; }

        [Required]
        public string CommentTitle { get; set; }
        [Required]
        public string CommentContent { get; set; }
        public bool IsDelete { get; set; }
    }
}
