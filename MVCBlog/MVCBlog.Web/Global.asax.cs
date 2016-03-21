using MVCBlog.Repository;
using MVCBlog.Service.Interfaces;
using MVCBlog.Web.App_Start;
using MVCBlog.Web.CommonHelper;
using MVCBlog.Web.Infrastructure;
using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace MVCBlog.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public MvcApplication()
        {
            AuthorizeRequest += new EventHandler(MvcApplication_AuthorizeRequest);
 
        }

        void MvcApplication_AuthorizeRequest(object sender, EventArgs e)
        {
            //IIdentity id = Context.User.Identity;
            //if (id.IsAuthenticated)
            //{
            //    var customerService = System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IUserService)) as IUserService;
            //    var userinfo = customerService.GetUserInfo(id.Name);

            //    ResolverHelper.GetResolver<MVCBlog.Service.PostService>();

            //}
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var formsIdetity = HttpContext.Current.User.Identity as FormsIdentity;
                MVCBlogIdentity mvcblogIdentity = new MVCBlogIdentity(formsIdetity.Ticket.UserData)
                {
                    AuthenticationType = formsIdetity.AuthenticationType,
                    IsAuthenticated = formsIdetity.IsAuthenticated,
                    Name = formsIdetity.Name,
                    Ticket = formsIdetity.Ticket,
                };
                MVCBlogPrincipal mvcblogPrincipal = new MVCBlogPrincipal() { Identity = mvcblogIdentity, };
                HttpContext.Current.User = mvcblogPrincipal;
            }
        }

        protected void Application_Start()
        {
            //配置log4net日志
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("log4net.config")));
            CacheManager.RedisHelper.DeleteAllKeys();

            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());

            Database.SetInitializer<MVCBlogContext>(new DropCreateDatabaseIfModelChanges<MVCBlogContext>());

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            AreaRegistration.RegisterAllAreas();
            DependencyConfigure.Initialize();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
