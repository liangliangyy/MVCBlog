using MVCBlog.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCBlog.Entities.Models
{
    [Table("PostInfo")]
    public class PostInfo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public PostStatus PostStatus { get; set; }
        public PostType PostType { get; set; }
        public CommentStatus CommentStatus { get; set; }
        public int CommentCount { get; set; }
        public virtual UserInfo PostAuthor { get; set; }
        public virtual ICollection<CommentInfo> CommentInfos { get; set; }
        public virtual ICollection<PostMetasInfo> PostMetasInfos { get; set; }
        public DateTime? EditedTime { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsDelete { get; set; }
    }
}
