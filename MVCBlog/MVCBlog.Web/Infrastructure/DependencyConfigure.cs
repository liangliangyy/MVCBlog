using Autofac;
using Autofac.Integration.Mvc;
using MVCBlog.Entities.Models;
using MVCBlog.Repository;
using MVCBlog.Service;
using MVCBlog.Service.Interfaces;
using MVCBlog.Web.CommonHelper;
using System.Web.Mvc;
namespace MVCBlog.Web.Infrastructure
{
    internal class DependencyConfigure
    {
        public static void Initialize()
        {
            var builder = new ContainerBuilder();
            IContainer container = RegisterServices(builder);
            builder.RegisterType<IDependencyResolver>().As<Dependency>();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            ApplicationContainer.Container = container;
            // DependencyResolver.SetResolver(new Dependency(container));
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {

            builder.RegisterAssemblyTypes(
                typeof(MvcApplication).Assembly
                ).PropertiesAutowired();

            //注册上下文.每次都会创建不同的实例
            //builder.RegisterType<MVCBlogContext>().As<MVCBlogContext>().InstancePerRequest();
            builder.RegisterType<MVCBlogContext>().InstancePerLifetimeScope();
            //注册PostService
            //这里通过ContainerBuilder方法RegisterType进行注册.当注册的类型在相应得到的容器中可以Resolve你的实例
            //通过AS可以通过构造函数依赖注入类型相应的接口
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<PostService>().As<IPostService>();

            builder.RegisterType<CategoryService>().As<ICategoryService>();
            //Build()方法生成一个对应的Container实例,这样,就可以通过Resolve解析到注册的类型实例
            return builder.Build();
        }


    }
}