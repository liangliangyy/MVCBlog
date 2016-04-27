using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCBlog.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Web.Models
{
    public class UserInfoModel
    {
        public UserInfoModel()
        {
            CreateTime = DateTime.Now;
            UserStatus = UserStatus.正常;
            UserRole = UserRole.作者;
        }
        [Display(Name = "用户编号")]
        public int Id { get; set; }

        [Display(Name = "用户昵称")]
        [Required(ErrorMessage = "用户昵称不能为空")]
        public string Name { get; set; }

        [Required(ErrorMessage = "用户邮箱不能为空")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Display(Name ="用户状态")]
        public UserStatus UserStatus { get; set; }
        [Required]
        [Display(Name = "用户角色")]
        public UserRole UserRole { get; set; }
        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }
        [Display(Name = "最后登录时间")]
        public DateTime? LastLoginTime { get; set; }
    }
}
