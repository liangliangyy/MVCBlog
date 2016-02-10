using MVCBlog.Entities.Models;
using MVCBlog.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCBlog.Entities.Enums;
using MVCBlog.Service.Interfaces;
using MVCBlog.Common;
using MVCBlog.CacheManager;
namespace MVCBlog.Service
{
    public class UserService : IUserService
    {
        private MVCBlogContext Context;
        public UserService(MVCBlogContext _contest)
        {
            this.Context = _contest;
        }
        public void RegisterUserInfo(UserInfo user)
        {
            user.Password = AesSecret.EncryptStringToAES(user.Password);
            user.CreateTime = DateTime.Now;
            user.EditedTime = DateTime.Now;
            user.UserStatus = UserStatus.正常;
            user.UserRole = UserRole.读者.ToString();
            user.IsDelete = false;
            Context.UserInfo.Add(user);
            Context.SaveChanges();
        }
        public UserInfo ValidateUser(string email, string password)
        {
            if (Context.UserInfo.Any(x => x.Email == email))
            { 
                var user = Context.UserInfo.First(x => x.Email == email);
                string encryptPassword = AesSecret.EncryptStringToAES(password);
                if (user.Password == encryptPassword)
                {
                    user.LastLoginTime = DateTime.Now;
                    Context.SaveChanges();
                    user = Context.UserInfo.Find(user.Id);
                    return user;
                }
            }
            return null;
        }

        public bool CheckUserEmail(string email)
        {
            return Context.UserInfo.Any(x => x.Email == email);
        }

        public UserInfo GetUserInfo(string email)
        {
            string key = ConfigInfo.GetUserKey(email);
            Func<UserInfo> getitem = () => Context.UserInfo.FirstOrDefault(x => x.Email == email);
            var userinfo = RedisHelper.GetEntity<UserInfo>(key, getitem);
            //var userinfo = Context.UserInfo.FirstOrDefault(x => x.Email == email);
            return userinfo;
        }

    }
}
