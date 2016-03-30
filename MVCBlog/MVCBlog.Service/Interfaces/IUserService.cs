using MVCBlog.Common.OAuth.Models;
using MVCBlog.Entities.Enums;
using MVCBlog.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Service.Interfaces
{
    public interface IUserService : IBase<UserInfo>
    {
        bool CheckUserEmail(string email);
        UserInfo ValidateUser(string email, string password);
        Task<UserInfo> ValidateUserAsync(string email, string password);
        UserInfo GetUserInfo(string email);
        Task<UserInfo> GetUserInfoAsync(string email); 
        UserInfo GetUserInfoByUid(string uid);
        UserInfo GetUserInfoByUid(string uid, OAuthSystemType systemtype);
    }
}
