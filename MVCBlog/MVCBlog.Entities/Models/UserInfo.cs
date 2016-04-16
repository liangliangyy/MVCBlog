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

    public class UserInfo : BaseModel
    {
        public UserInfo()
        {
            this.UserRole = string.Empty;
            this.UserStatus = MVCBlog.Entities.Enums.UserStatus.正常;
            this.CreateTime = DateTime.Now;

        }
     

        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string UserRole { get; set; }
        public UserStatus UserStatus { get; set; }
      
        public DateTime? LastLoginTime { get; set; }
    
        public string WeiBoAccessToken { get; set; }
        public string WeiBoUid { get; set; }
        public string WeiBoAvator { get; set; }

        public string QQAccessToken { get; set; }
        public string QQUid { get; set; }
        public string QQAvator { get; set; }

        public string WeiXinAccessToken { get; set; }
        public string WeiXinUid { get; set; }
        public string WeiXinAvator { get; set; }
    }
}