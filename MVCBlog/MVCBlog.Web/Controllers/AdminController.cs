using MVCBlog.Entities.Models;
using MVCBlog.Service.Interfaces;
using MVCBlog.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public AdminController(IPostService _postService,IUserService _userService)
        {
            postService = _postService;
            userService = _userService;
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserViewModel userinfo)
        {
            if(userService.CheckUserEmail(userinfo.Email))
            {
                ModelState.AddModelError("Email", "该Email已经被注册");
            }
            if (ModelState.IsValid)
            {
                var entity = new UserInfo();
                entity.Name = userinfo.Name;
                entity.Password = userinfo.Password;
                entity.Email = userinfo.Email;
                userService.RegisterUserInfo(entity);
                return RedirectToAction("LogIn");
            }
            return View();
        }
        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(string email,string password)
        {
            if(userService.ValidateUser(email,password)==null)
            {
                ModelState.AddModelError("", "您输入的帐号或密码错误");
                return View();
            }
            FormsAuthentication.SetAuthCookie(email, false);
            return RedirectToAction("Index");
        }
        public ActionResult WritePost()
        {
            return View();
        }
    }
}