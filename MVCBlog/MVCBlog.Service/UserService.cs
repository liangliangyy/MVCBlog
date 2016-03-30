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
using MVCBlog.Common.OAuth.Models;

namespace MVCBlog.Service
{
    public class UserService : BaseService<UserInfo>, IUserService
    {
        private MVCBlogContext Context;

        public UserService(MVCBlogContext _contest)
        {
            this.Context = _contest;
        }

        public override async Task InsertAsync(UserInfo user, int userid = 0)
        {
            user.Password = AesSecret.EncryptStringToAES(user.Password);
            user.CreateTime = DateTime.Now;
            user.EditedTime = DateTime.Now;
            user.UserStatus = UserStatus.正常;
            user.UserRole = UserRole.读者.ToString();
            user.IsDelete = false;
            Context.UserInfo.Add(user);
            await SaveChanges();
            await base.InsertAsync(user, userid);
        }
        public UserInfo ValidateUser(string email, string password)
        {
            string encryptPassword = AesSecret.EncryptStringToAES(password);
            var entity = Context.UserInfo.FirstOrDefault(x => x.Email == email && x.Password == encryptPassword);
            if (entity != null)
            {
                entity.LastLoginTime = DateTime.Now;
                Context.SaveChanges();
                entity = Context.UserInfo.Find(entity.Id);
                return entity;
            }
            return null;
        }
        public async Task<UserInfo> ValidateUserAsync(string email, string password)
        {
            string encryptPassword = AesSecret.EncryptStringToAES(password);
            Func<UserInfo> finditem = () => Context.UserInfo.FirstOrDefault(x => x.Email == email && x.Password == encryptPassword);
            var entity = await Common.TaskExtensions.WithCurrentCulture<UserInfo>(finditem);
            if (entity != null)
            {
                entity.LastLoginTime = DateTime.Now;
                await SaveChanges();
                entity = await Context.UserInfo.FindAsync(entity.Id);
                return entity;
            }
            return null;
        }

        public bool CheckUserEmail(string email)
        {
            return Context.UserInfo.Any(x => x.Email == email);
        }

        public override void Update(UserInfo model)
        {
            var entity = Context.UserInfo.Find(model.Id);

            entity.EditedTime = DateTime.Now;
            entity.Name = model.Name;
            entity.Password = model.Password;
            entity.UserRole = model.UserRole;
            entity.UserStatus = model.UserStatus;
            entity.WeiBoAccessToken = model.WeiBoAccessToken;
            entity.WeiBoUid = model.WeiBoUid;
            entity.WeiBoAvator = model.WeiBoAvator;
            entity.QQAccessToken = model.QQAccessToken;
            entity.QQAvator = model.QQAvator;
            entity.QQUid = model.QQUid;
            Context.SaveChanges();
            base.Update(model);
        }

        public override async Task UpdateAsync(UserInfo model)
        {
            var entity = await Context.UserInfo.FindAsync(model.Id);
            entity.EditedTime = DateTime.Now;
            entity.Name = model.Name;
            entity.Password = model.Password;
            entity.UserRole = model.UserRole;
            entity.UserStatus = model.UserStatus;
            entity.WeiBoAccessToken = model.WeiBoAccessToken;
            entity.WeiBoUid = model.WeiBoUid;
            entity.WeiBoAvator = model.WeiBoAvator;
            entity.QQAccessToken = model.QQAccessToken;
            entity.QQAvator = model.QQAvator;
            entity.QQUid = model.QQUid;
            await SaveChanges();
            await base.UpdateAsync(model);
        }

        public override void Delete(UserInfo model)
        {
            var entity = Context.UserInfo.Find(model.Id);
            entity.IsDelete = true;
            Context.SaveChanges();
            base.Delete(model);
        }

        public override async Task DeleteAsync(UserInfo model)
        {
            var entity = await Context.UserInfo.FindAsync(model.Id);
            if (entity != null)
            {
                entity.IsDelete = true;
                await SaveChanges();
                await base.DeleteAsync(model);
            }
        }

        public override async Task<int> SaveChanges()
        {
            return await Common.TaskExtensions.WithCurrentCulture<int>(Context.SaveChangesAsync());
        }

        public override UserInfo GetById(int id)
        {
            return Context.UserInfo.Find(id);
        }

        public override async Task<UserInfo> GetByIdAsync(int id)
        {
            return await Context.UserInfo.FindAsync(id);
        }

        public UserInfo GetUserInfoByUid(string uid)
        {
            if (!string.IsNullOrEmpty(uid))
            {
                var userinfo = Context.UserInfo.FirstOrDefault(x => x.WeiBoUid == uid);
                return userinfo;
            }
            return null;
        }

        public UserInfo GetUserInfoByUid(string uid, OAuthSystemType systemtype)
        {
            switch (systemtype)
            {
                case OAuthSystemType.QQ:
                    return Context.UserInfo.FirstOrDefault(x => x.QQUid == uid);
                case OAuthSystemType.Weibo:
                    return Context.UserInfo.FirstOrDefault(x => x.WeiBoUid == uid);
                default:
                    return null;
            }
        }

        public override void Insert(UserInfo user, int userid = 0)
        {
            user.Password = AesSecret.EncryptStringToAES(user.Password);
            user.CreateTime = DateTime.Now;
            user.EditedTime = DateTime.Now;
            user.UserStatus = UserStatus.正常;
            user.UserRole = UserRole.读者.ToString();
            user.IsDelete = false;
            user = Context.UserInfo.Add(user);
            Context.SaveChanges();
            base.Insert(user, userid);
        }

        public override UserInfo GetFromDB(int id)
        {
            return Context.UserInfo.Find(id);
        }

        public override string GetModelKey(UserInfo model)
        {
            return ConfigInfo.GetUserKey(model.Id);
        }

        public UserInfo GetUserInfo(string email)
        {
            return Context.UserInfo.Single(x => x.Email == email);
        }

        public async Task<UserInfo> GetUserInfoAsync(string email)
        {
            return await Common.TaskExtensions.WithCurrentCulture<UserInfo>(() =>
            {
                return Context.UserInfo.Single(x => x.Email == email);
            });
        }
    }
}
