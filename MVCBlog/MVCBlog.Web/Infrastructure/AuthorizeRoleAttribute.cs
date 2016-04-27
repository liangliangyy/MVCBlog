using MVCBlog.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVCBlog.Web.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        private readonly UserRole[] roles;
        public AuthorizeRoleAttribute(params UserRole[] roles)
        {
            this.roles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (base.AuthorizeCore(httpContext))
            {
                var formsIdetity = httpContext.User.Identity as MVCBlogIdentity;
                if (formsIdetity != null)
                {
                    //用户的最大权限大于等于要求的最小权限
                    int maxUserPermission = formsIdetity.UserData.UserRoles.Select(x => (int)x).Max();
                    int minCheckPermission = roles.Select(x => (int)x).Min();
                    return maxUserPermission >= minCheckPermission;
                }
            }
            return false;
        }

        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    string cookieName = FormsAuthentication.FormsCookieName;

        //    if (!filterContext.HttpContext.User.Identity.IsAuthenticated ||
        //        filterContext.HttpContext.Request.Cookies == null ||
        //        filterContext.HttpContext.Request.Cookies[cookieName] == null
        //    )
        //    {
        //        HandleUnauthorizedRequest(filterContext);
        //        return;
        //    }
        //    var formsIdetity = filterContext.HttpContext.User.Identity as MVCBlogIdentity;
        //    MVCBlogIdentity mvcblogIdentity = new MVCBlogIdentity(formsIdetity.Ticket.UserData)
        //    {
        //        AuthenticationType = formsIdetity.AuthenticationType,
        //        IsAuthenticated = formsIdetity.IsAuthenticated,
        //        Name = formsIdetity.Name,
        //        Ticket = formsIdetity.Ticket
        //    };
        //    //MVCBlogPrincipal mvcblogPrincipal = new MVCBlogPrincipal() { Identity = mvcblogIdentity, };
        //    UserRole[] roles = mvcblogIdentity.UserData.UserRoles == null ? new UserRole[] { UserRole.作者 } :
        //        mvcblogIdentity.UserData.UserRoles.ToArray();

        //    MVCBlogPrincipal mvcblogPrincipal = new MVCBlogPrincipal(mvcblogIdentity, roles);



        //    filterContext.HttpContext.User = mvcblogPrincipal;
        //    base.OnAuthorization(filterContext);

        //}
    }
}
