using MVCBlog.Entities.Models;
using MVCBlog.Service;
using MVCBlog.Web.CommonHelper;
using MVCBlog.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCBlog.Web.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment
        private CommentService commentService = (CommentService)ResolverHelper.GetResolver<CommentService>();
        private PostService postService = (PostService)ResolverHelper.GetResolver<PostService>();
        private UserService userService = (UserService)ResolverHelper.GetResolver<UserService>();
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
        public ActionResult CommentInfo(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {

                var userinfo = userService.GetUserInfo(model.UserEmail);
                var postinfo = postService.GetById(model.PostID);
                var commentinfo = new CommentInfo()
                {
                    CommentUser = userinfo,
                    CommentPost = postinfo,
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