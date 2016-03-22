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
    public class UserService : IUserService
    {
        private MVCBlogContext Context;

        public event EventHandler<ModelCacheEventArgs> ModelDeleteEventHandler;
        public event EventHandler<ModelCacheEventArgs> ModelCreateEventHandler;
        public event EventHandler<ModelCacheEventArgs> ModelUpdateEventHandler;

        public UserService(MVCBlogContext _contest)
        {
            this.Context = _contest;
        }
        public void Insert(UserInfo user)
        {
            user.Password = AesSecret.EncryptStringToAES(user.Password);
            user.CreateTime = DateTime.Now;
            user.EditedTime = DateTime.Now;
            user.UserStatus = UserStatus.正常;
            user.UserRole = UserRole.读者.ToString();
            user.IsDelete = false;
            user = Context.UserInfo.Add(user);
            Context.SaveChanges();
            if (ModelCreateEventHandler != null)
            {
                ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetUserKey(user.Email), ID = user.Id };
                ModelCreateEventHandler(this, e);
            }
        }
        public async Task InsertAsync(UserInfo user, int userid = 0)
        {
            user.Password = AesSecret.EncryptStringToAES(user.Password);
            user.CreateTime = DateTime.Now;
            user.EditedTime = DateTime.Now;
            user.UserStatus = UserStatus.正常;
            user.UserRole = UserRole.读者.ToString();
            user.IsDelete = false;
            Context.UserInfo.Add(user);
            await SaveChanges();
            if (ModelCreateEventHandler != null)
            {
                ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetUserKey(user.Email), ID = user.Id };
                ModelCreateEventHandler(this, e);
            }
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

        public UserInfo GetUserInfo(string email)
        {
            string key = ConfigInfo.GetUserKey(email);
            Func<UserInfo> getitem = () => Context.UserInfo.FirstOrDefault(x => x.Email == email);
            var userinfo = RedisHelper.GetEntity<UserInfo>(key, getitem);
            return userinfo;
        }

        public async Task<UserInfo> GetUserInfoAsync(string email)
        {
            string key = ConfigInfo.GetUserKey(email);
            Func<UserInfo> getitem = () => Context.UserInfo.FirstOrDefault(x => x.Email == email);
            var userinfo = await RedisHelper.GetEntityAsync<UserInfo>(key, getitem);
            return userinfo;
        }
         

        public void Update(UserInfo model)
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

            if (ModelUpdateEventHandler != null)
            {
                ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetUserKey(model.Email), ID = model.Id };
                ModelUpdateEventHandler(this, e);
            }
        }

        public async Task UpdateAsync(UserInfo model)
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
            if (ModelUpdateEventHandler != null)
            {
                ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetUserKey(model.Email), ID = model.Id };
                ModelUpdateEventHandler(this, e);
            }
        }

        public void Delete(UserInfo model)
        {
            var entity = Context.UserInfo.Find(model.Id);
            entity.IsDelete = true;
            Context.SaveChanges();
            if (ModelDeleteEventHandler != null)
            {
                ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetUserKey(model.Email), ID = model.Id };
                ModelDeleteEventHandler(this, e);
            }
        }

        public async Task DeleteAsync(UserInfo model)
        {
            var entity = await Context.UserInfo.FindAsync(model.Id);
            if (entity != null)
            {
                entity.IsDelete = true;
                await SaveChanges();
                if (ModelDeleteEventHandler != null)
                {
                    ModelCacheEventArgs e = new ModelCacheEventArgs() { Key = ConfigInfo.GetUserKey(model.Email), ID = model.Id };
                    ModelDeleteEventHandler(this, e);
                }
            }
        }

        public async Task<int> SaveChanges()
        {
            return await Common.TaskExtensions.WithCurrentCulture<int>(Context.SaveChangesAsync());
        }

        public UserInfo GetById(int id)
        {
            return Context.UserInfo.Find(id);
        }

        public async Task<UserInfo> GetByIdAsync(int id)
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

        public void Insert(UserInfo model, int userid = 0)
        {
            Insert(model);
        }

        public UserInfo GetFromDB(int id)
        {
            return Context.UserInfo.Find(id);
        }
    }
}
