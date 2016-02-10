using MVCBlog.Common;
using MVCBlog.Service;
using MVCBlog.Service.Interfaces;
using MVCBlog.Web.CommonHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCBlog.Web.Controllers
{
    //[Authorize]
    public class UserController : Controller
    {
        private IUserService userService = (IUserService)ResolverHelper.GetResolver<UserService>();
        private IPostService postService = (IPostService)ResolverHelper.GetResolver<PostService>();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserList()
        {
            return View();
        }


        public ActionResult UserPostInfo(int userid, int index = 1)
        {
            var postinfos = postService.GetUserPostsAsync(userid, index, ConfigInfo.PageCount).Result;
            return View(postinfos);
        }
    }
}