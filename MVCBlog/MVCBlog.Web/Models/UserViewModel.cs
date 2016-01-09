using MVCBlog.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Web.Models
{
    public class UserViewModel
    {
        [DisplayName("中文姓名")]
        [Required(ErrorMessage = "请输入中文姓名")]
        [Description("暂时不考虑外国人英文姓名注册")]
        public string Name { get; set; }

        [DisplayName("会员帐号")]
        [Required(ErrorMessage = "请输入Email帐号")]
        [Description("我们直接以Email帐号作为会员登录帐号")]
        [MaxLength(250, ErrorMessage = "Email地址长度不能超过250个字符")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [DisplayName("会员密码")]
        [Required(ErrorMessage = "请输入密码")]
        [Description("密码将会以SHA1进行哈希运算,通过SHA1哈希运算后的结果转为HEX表示法的字符串长度皆为40个字符")]
        [DataType(DataType.Password)]
        public string Password { get; set; }



        [DisplayName("请再次输入会员密码")]
        [Required(ErrorMessage = "再次输请输入密码")]
        [Description("密码将会以SHA1进行哈希运算,通过SHA1哈希运算后的结果转为HEX表示法的字符串长度皆为40个字符")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "密码不相符")]
        public string RepeatPassword { get; set; }
    }
}
