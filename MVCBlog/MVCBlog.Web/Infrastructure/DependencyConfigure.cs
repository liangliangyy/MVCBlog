using Autofac;
using Autofac.Core;
using Autofac.Extras.DynamicProxy2;
using Autofac.Integration.Mvc;
using Castle.DynamicProxy;
using MVCBlog.Entities.Models;
using MVCBlog.Repository;
using MVCBlog.Service;
using MVCBlog.Service.Interfaces;
using MVCBlog.Web.CommonHelper;
using MVCBlog.Web.Infrastructure;
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
            //ApplicationContainer.Container = container;
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            //DependencyResolver.SetResolver(new Dependency(container));
        }


        private static IContainer RegisterServices(ContainerBuilder builder)
        {

            builder.RegisterAssemblyTypes(
                typeof(MvcApplication).Assembly
                ).PropertiesAutowired();

          
            
            //注册PostService
            //这里通过ContainerBuilder方法RegisterType进行注册.当注册的类型在相应得到的容器中可以Resolve你的实例
            //通过AS可以通过构造函数依赖注入类型相应的接口


            builder.RegisterType<ModelCacheEventHandle>().InstancePerLifetimeScope();

            //builder.RegisterType<UserService>().As<IUserService>()
            //    //.OnActivated(InitinalServiceHandlerEvent<IUserService, UserInfo>.handler)
            //    .EnableInterfaceInterceptors().InterceptedBy(typeof(CallLogger))
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CategoryService>().As<ICategoryService>()
            //   // .OnActivated(InitinalServiceHandlerEvent<ICategoryService, CategoryInfo>.handler)
            //    .EnableInterfaceInterceptors().InterceptedBy(typeof(CallLogger))
            //    .InstancePerLifetimeScope(); ;
            //builder.RegisterType<PostService>().As<IPostService>()
            //   // .OnActivated(InitinalServiceHandlerEvent<IPostService, PostInfo>.handler)
            //    .EnableInterfaceInterceptors().InterceptedBy(typeof(CallLogger))
            //    .InstancePerLifetimeScope();
            //builder.RegisterType<CommentService>().As<ICommentService>()
            //  // .OnActivated(InitinalServiceHandlerEvent<ICommentService, CommentInfo>.handler)
            //   .EnableInterfaceInterceptors().InterceptedBy(typeof(CallLogger))
            //   .InstancePerLifetimeScope();
            //builder.RegisterType<PostMetaService>().As<IPostMetaService>()
            //// .OnActivated(InitinalServiceHandlerEvent<PostMetaService, PostMetasInfo>.handler)
            // .EnableInterfaceInterceptors().InterceptedBy(typeof(CallLogger))
            // .InstancePerLifetimeScope();

            builder.Register<IUserService>(c => new UserService())
                //  .OnActivated(InitinalServiceHandlerEvent<IUserService, UserInfo>.handler)
                .EnableInterfaceInterceptors().InterceptedBy(typeof(CallLogger)).InstancePerLifetimeScope();

            builder.Register<ICategoryService>(c => new CategoryService())
                // .OnActivated(InitinalServiceHandlerEvent<ICategoryService, CategoryInfo>.handler)
                .EnableInterfaceInterceptors().InterceptedBy(typeof(CallLogger)).InstancePerLifetimeScope();

            builder.Register<ICommentService>(c => new CommentService())
                // .OnActivated(InitinalServiceHandlerEvent<ICommentService, CommentInfo>.handler)
                .EnableInterfaceInterceptors().InterceptedBy(typeof(CallLogger)).InstancePerLifetimeScope();

            builder.Register<IPostService>(c => new PostService())
                // .OnActivated(InitinalServiceHandlerEvent<IPostService, PostInfo>.handler)
                .EnableInterfaceInterceptors().InterceptedBy(typeof(CallLogger)).InstancePerLifetimeScope();
            //   builder.RegisterType<UserService>().As<IUserService>()
            //        .WithParameter(
            //new ResolvedParameter(
            //  (pi, ctx) => pi.ParameterType == typeof(MVCBlogContext),
            //  (pi, ctx) => ctx.Resolve<MVCBlogContext>()))
            //       .EnableInterfaceInterceptors().InterceptedBy(typeof(CallLogger))
            //       .InstancePerLifetimeScope();
            //   builder.RegisterType<CategoryService>().As<ICategoryService>()
            //        .WithParameter(
            //new ResolvedParameter(
            //  (pi, ctx) => pi.ParameterType == typeof(MVCBlogContext),
            //  (pi, ctx) => ctx.Resolve<MVCBlogContext>()))
            //       .EnableInterfaceInterceptors().InterceptedBy(typeof(CallLogger))
            //       .InstancePerLifetimeScope(); ;
            //   builder.RegisterType<PostService>().As<IPostService>()
            //        .WithParameter(
            //new ResolvedParameter(
            //  (pi, ctx) => pi.ParameterType == typeof(MVCBlogContext),
            // (pi, ctx) => ctx.Resolve<MVCBlogContext>()))
            //       .EnableInterfaceInterceptors().InterceptedBy(typeof(CallLogger))
            //       .InstancePerLifetimeScope();
            //   builder.RegisterType<CommentService>().As<ICommentService>()
            //        .WithParameter(
            //new ResolvedParameter(
            //  (pi, ctx) => pi.ParameterType == typeof(MVCBlogContext),
            //  (pi, ctx) => ctx.Resolve<MVCBlogContext>()))
            //      .EnableInterfaceInterceptors().InterceptedBy(typeof(CallLogger))
            //      .InstancePerLifetimeScope();

            builder.RegisterType<CallLogger>();
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