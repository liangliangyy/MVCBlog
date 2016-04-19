using MVCBlog.Common;
using MVCBlog.Entities.Models;
using MVCBlog.Service;
using MVCBlog.Service.Interfaces;
using MVCBlog.Web.CommonHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVCBlog.Web.Controllers
{
    //[Authorize]
    public class UserController : Controller
    {
        private IUserService userService;
        private IPostService postService;
        public UserController(IUserService _userService, IPostService _postService)
        {
            userService = _userService;
            postService = _postService;
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserList()
        {
            return View();
        }

        public ActionResult UserWightInfo()
        {
            UserInfo userinfo = UserHelper.GetLogInUserInfo();
            if (userinfo != null)
            {
                return PartialView(userinfo);
            }
            return PartialView();
        }

        public ActionResult UserPostInfo(int userid, int index = 1)
        {
            var postinfos = postService.Query(index, ConfigInfo.PageCount, x => x.PostAuthor.Id == userid);
            return View(postinfos);
        }
    }
}