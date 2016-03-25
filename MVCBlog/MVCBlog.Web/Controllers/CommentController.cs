using MVCBlog.Common;
using MVCBlog.Entities.Models;
using MVCBlog.Service;
using MVCBlog.Service.Interfaces;
using MVCBlog.Web.CommonHelper;
using MVCBlog.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVCBlog.Web.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment
        private ICommentService commentService ;
        private IPostService postService;
        private IUserService userService;
        public CommentController(ICommentService _commentService, IPostService _postService, IUserService _userService)
        {
            commentService = _commentService;
            postService = _postService;
            userService = _userService;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetCommentInfo(int postid)
        {
            var res = commentService.CommentList(postid);
            return PartialView(res);
        }
        [HttpGet]
        public ActionResult CommentInfo(int postid)
        {
            var model = new CommentViewModel();
            var loginuser = UserHelper.GetLogInUserInfo();
            model.PostID = postid;
            if (loginuser != null)
            {
                model.UserEmail = loginuser.Email;
                model.UserName = loginuser.Name;
            }
            return PartialView(model);
        }

        [HttpPost]
        public async Task<ActionResult> CommentInfo(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {

                var userinfo = userService.GetUserInfo(model.UserEmail);
                if (userinfo == null)
                {
                    UserInfo u = new UserInfo()
                    {
                        Email = model.UserEmail,
                        Name = model.UserName,
                        Password = ConfigInfo.UserDefaultPassword
                    };
                    await userService.InsertAsync(u, 0);
                    userinfo = await userService.GetUserInfoAsync(model.UserEmail);
                }
                var postinfo = postService.GetById(model.PostID);
                var commentinfo = new CommentInfo()
                {
                    CommentUser = userinfo,
                    PostID = model.PostID,
                    CommentContent = model.CommentContent,
                    CommentTitle = model.CommentTitle
                };
                commentService.AddCommentInfo(commentinfo);
                return RedirectToAction("PostInfo", "PostInfo", new { id = model.PostID });
            }
            return View();
        }
    }
}