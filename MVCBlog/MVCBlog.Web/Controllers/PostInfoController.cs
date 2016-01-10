using MVCBlog.Common;
using MVCBlog.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCBlog.Web.Controllers
{
    public class PostInfoController : Controller
    {
        private readonly IPostService postService;
        private readonly IUserService userService;
        private readonly ICategoryService categoryService;
        public PostInfoController(IPostService _postService, IUserService _userService, ICategoryService _categoryService)
        {
            postService = _postService;
            userService = _userService;
            categoryService = _categoryService;
        }
        // GET: PostInfo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RecentPost()
        {
            var recentpost = postService.GetRecentPost(ConfigInfo.RecentPostCount);
            return PartialView(recentpost);
        }

        public ActionResult PostCategoryInfo()
        {
            var categorylist = categoryService.GetCategoryList();
            return PartialView(categorylist);
        }

        public ActionResult PostInfo(int id)
        {
            var entity = postService.GetById(id);
            return View(entity);
        }
    }
}