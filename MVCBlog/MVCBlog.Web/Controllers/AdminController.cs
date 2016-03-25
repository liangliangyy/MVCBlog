using MVCBlog.Common.OAuth.Models;
using MVCBlog.Entities.Models;
using MVCBlog.Service.Interfaces;
using MVCBlog.Web.CommonHelper;
using MVCBlog.Web.Models;
using Newtonsoft.Json;
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
            var userinfo = userService.ValidateUser(email, password);
            if (userinfo == null)
            {
                ModelState.AddModelError("", "您输入的帐号或密码错误");
                return View();
            }
            UserDataModel userData = new UserDataModel()
            {
                Id = userinfo.Id,
                Email = userinfo.Email,
                Name = userinfo.Name,
                SystemType = OAuthSystemType.Email,
                Uid = string.Empty,
                AccessToken = string.Empty
            };
            UserHelper.SetFormsAuthenticationTicket(email, userData, true);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> PostDeal(int postid = 0)
        {
            var category = await Helper.GetCategorySelectList();
            if (UserHelper.GetLogInUserInfo() == null)
            {
                return RedirectToAction("LogIn");
            }
            PostViewModel model = new PostViewModel();
            if (postid != 0)
            {
                var entity = await postService.GetByIdAsync(postid);
                model.Id = entity.Id;
                model.Content = entity.Content;
                model.Title = entity.Title;
                model.PostStatus = entity.PostStatus;
                model.PostCommentStatus = entity.PostCommentStatus;
                model.CategoryID = entity.PostCategoryInfo.Id;
            }
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PostDeal(PostViewModel postinfo)
        {
            if (UserHelper.GetLogInUserInfo() == null)
            {
                return RedirectToAction("LogIn");
            }
            if (ModelState.IsValid)
            {
                var entity = new PostInfo();
                entity.Id = postinfo.Id;
                entity.Title = postinfo.Title;
                entity.Content = postinfo.Content;
                entity.PostCategoryInfo = categoryService.GetCategoryList().First(x => x.Id == postinfo.CategoryID);
                if (entity.Id == 0)
                {
                    await postService.InsertAsync(entity, UserHelper.GetLogInUserInfo().Id);
                }
                else
                {
                    entity.PostCommentStatus = postinfo.PostCommentStatus;
                    entity.PostStatus = postinfo.PostStatus;
                    await postService.UpdateAsync(entity);
                }
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
        public async Task<ActionResult> AddCategoryInfo(CategoryInfo categoryinfo)
        {
            if (ModelState.IsValid)
            {
                categoryinfo.CreateUser = UserHelper.GetLogInUserInfo();
                await categoryService.InsertAsync(categoryinfo, categoryinfo.CreateUser.Id);
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