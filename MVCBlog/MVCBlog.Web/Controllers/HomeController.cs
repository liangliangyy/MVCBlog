using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBlog.Service.Interfaces;
using MVCBlog.Web.CommonHelper.Menu;
using Newtonsoft.Json;
using PagedList;
using MVCBlog.Entities.Models;
using MVCBlog.Common;
using MVCBlog.Entities;
using MVCBlog.Web.CommonHelper;

namespace MVCBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostService postService;
        private readonly IUserService userService;
        public HomeController(IPostService _postService, IUserService _userService)
        {
            postService = _postService;
            userService = _userService;
        }
        // GET: Home
        public ActionResult Index(int pageindex = 1)
        { 
            //List<MenuInfo> list = new List<MenuInfo>();
            //list.Add(new MenuInfo() { MenuName = "Home", MenuUrl = "/Admin/", MenuPosition = 1 });
            //list.Add(new MenuInfo() { MenuName = "创建文章", MenuUrl = "/Admin/CreatePost", MenuPosition = 2 });
            //string jsoninfo = JsonConvert.SerializeObject(list);
            //var test = postService.GetPosts();
            //return View();
            UserHelper.GetLogInUserInfo();
            Pagination<PostInfo> pagination = postService.PostPagination(pageindex, ConfigInfo.PageCount);
            return View(pagination);
        }
    }
}