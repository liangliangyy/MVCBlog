using MVCBlog.Common;
using MVCBlog.Entities.Models;
using MVCBlog.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVCBlog.Web.Controllers
{
    [AllowAnonymous]
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
            var categorylist = categoryService.Query();
            return PartialView(categorylist);
        }

        public ActionResult PostInfo(int id)
        {
            var entity = postService.GetById(id);
            return View(entity);
        }

        public ActionResult PostMonthList()
        {
            var posttimes = postService.GetPostMonthInfos();
            var times = posttimes.Select(x => x.ToString("yyyy-MM")).Distinct();
            return PartialView(times);
        }
        public ActionResult PostMonthInfo(int year, int month, int index = 1)
        {
            var postinfo = postService.Query(index, ConfigInfo.PageCount, x => x.CreateTime.Year == year && x.CreateTime.Month == month);
            return View(postinfo);
        }

        public ActionResult PostMetaInfo(PostInfo info)
        {
            return PartialView(info);
        }
    }
}