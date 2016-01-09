using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MVCBlog.Entities.Enums;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace MVCBlog.Entities.Models
{
    [Table("UserInfo")]
    public class UserInfo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string UserRole { get; set; }
        public UserStatus UserStatus { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? EditedTime { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public bool IsDelete { get; set; }
    }
}