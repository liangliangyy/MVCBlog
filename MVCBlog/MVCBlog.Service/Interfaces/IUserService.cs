using MVCBlog.Entities.Enums;
using MVCBlog.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Service.Interfaces
{
    public interface IUserService
    {
        void RegisterUserInfo(UserInfo user);
        bool CheckUserEmail(string email);
        UserInfo ValidateUser(string email, string password);
        UserInfo GetUserInfo(string email);
    }
}
