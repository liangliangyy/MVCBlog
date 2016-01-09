using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBlog.Service.Interfaces;
using MVCBlog.Web.CommonHelper.Menu;
using Newtonsoft.Json;
namespace MVCBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostService _postService;

        public HomeController(IPostService postService)
        {
            _postService = postService;
        }
        // GET: Home
        public ActionResult Index()
        {
            List<MenuInfo> list = new List<MenuInfo>();
            list.Add(new MenuInfo() { MenuName = "Home", MenuUrl = "/Admin/", MenuPosition = 1 });
            list.Add(new MenuInfo() { MenuName = "创建文章", MenuUrl = "/Admin/CreatePost", MenuPosition = 2 });
            string jsoninfo = JsonConvert.SerializeObject(list);
            var test = _postService.GetPosts();
            return View();
        }
    }
}