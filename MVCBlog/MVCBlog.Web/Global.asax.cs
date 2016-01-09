using MVCBlog.Repository;
using MVCBlog.Service.Interfaces;
using MVCBlog.Web.CommonHelper;
using MVCBlog.Web.Infrastructure;
using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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
            IIdentity id = Context.User.Identity;
            if (id.IsAuthenticated)
            {

                var customerService = System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IUserService)) as IUserService;
                var userinfo = customerService.GetUserInfo(id.Name);

                ResolverHelper.GetResolver<MVCBlog.Service.PostService>();

            }
        }

        protected void Application_Start()
        {
            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());

            Database.SetInitializer<MVCBlogContext>(new DropCreateDatabaseIfModelChanges<MVCBlogContext>());


            AreaRegistration.RegisterAllAreas();
            DependencyConfigure.Initialize();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
