using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using MVCBlog.Entities.Models;
using MVCBlog.Repository;
using MVCBlog.Service;
using MVCBlog.Service.Interfaces;
using MVCBlog.Web.CommonHelper;
using System;
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

            builder.RegisterType<ModelCacheEventHandle>().InstancePerLifetimeScope();

            builder.RegisterType<UserService>().As<IUserService>()
                .OnActivated(InitinalServiceHandlerEvent<IUserService, UserInfo>.handler)
                .InstancePerLifetimeScope();
            builder.RegisterType<CategoryService>().As<ICategoryService>()
                .OnActivated(InitinalServiceHandlerEvent<ICategoryService, CategoryInfo>.handler)
                .InstancePerLifetimeScope(); ;
            builder.RegisterType<PostService>().As<IPostService>()
                .OnActivated(InitinalServiceHandlerEvent<IPostService, PostInfo>.handler)
                .InstancePerLifetimeScope();

            builder.RegisterType<CommentService>().As<ICommentService>()
               .OnActivated(InitinalServiceHandlerEvent<ICommentService, CommentInfo>.handler)
               .InstancePerLifetimeScope();

            //Build()方法生成一个对应的Container实例,这样,就可以通过Resolve解析到注册的类型实例
            return builder.Build();
        }


    }

    public static class InitinalServiceHandlerEvent<TIService, TModel> where TIService : Service.Interfaces.IBase<TModel> where TModel : class
    {
        public static Action<IActivatedEventArgs<TIService>> handler = act =>
        {
            var instance = act.Context.Resolve<ModelCacheEventHandle>();
            act.Instance.ModelCreateEventHandler += instance.ModelCreateEventHandler<TIService, TModel>;
            act.Instance.ModelDeleteEventHandler += instance.ModelDeleteEventHandler<TIService, TModel>;
            act.Instance.ModelUpdateEventHandler += instance.ModelUpdateEventHandler<TIService, TModel>;
        };
    }
}