using MVCBlog.Entities.Models;
using MVCBlog.Service.Interfaces;
using MVCBlog.Web.CommonHelper;
using MVCBlog.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVCBlog.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IPostService postService;
        private readonly IUserService userService;
        private readonly ICategoryService categoryService;
        public AdminController(IPostService _postService, IUserService _userService, ICategoryService _categoryService)
        {
            postService = _postService;
            userService = _userService;
            categoryService = _categoryService;
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(UserViewModel userinfo)
        {
            if (userService.CheckUserEmail(userinfo.Email))
            {
                ModelState.AddModelError("Email", "该Email已经被注册");
            }
            if (ModelState.IsValid)
            {
                var entity = new UserInfo();
                entity.Name = userinfo.Name;
                entity.Password = userinfo.Password;
                entity.Email = userinfo.Email;
                userService.Insert(entity);
                return RedirectToAction("LogIn");
            }
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(string email, string password)
        {
            if (userService.ValidateUser(email, password) == null)
            {
                ModelState.AddModelError("", "您输入的帐号或密码错误");
                return View();
            }
            FormsAuthentication.SetAuthCookie(email, false);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult WritePost()
        {
            var category = Helper.GetCategorySelectList();
            if (UserHelper.GetLogInUserInfo() == null)
            {
                return RedirectToAction("LogIn");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> WritePost(PostViewModel postinfo)
        {
            if (UserHelper.GetLogInUserInfo() == null)
            {
                return RedirectToAction("LogIn");
            }
            if (ModelState.IsValid)
            {
                var entity = new PostInfo();
                entity.Title = postinfo.Title;
                entity.Content = postinfo.Content;
                entity.PostCategoryInfo = categoryService.GetCategoryList().First(x => x.Id == postinfo.CategoryID);
                await postService.InsertAsync(entity, UserHelper.GetLogInUserInfo().Id);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult AddCategoryInfo()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddCategoryInfo(CategoryInfo categoryinfo)
        {
            if (ModelState.IsValid)
            {
                categoryinfo.CreateUser = UserHelper.GetLogInUserInfo();
                categoryService.AddCategoryInfo(categoryinfo);
                return RedirectToAction("CategoryList");
            }
            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult CategoryList()
        {
            var list = categoryService.GetCategoryList();
            return View(list);
        }
    }
}