using Autofac;
using System;
using System.Linq;
namespace MVCBlog.Web.CommonHelper
{
    //public class ResolverHelper
    //{
    //    public static object GetResolver<TService>()
    //    {
    //        var iTypes = typeof(TService).GetInterfaces();
    //        if(iTypes.Count()==0)
    //        {
    //            throw new NotImplementedException("依赖注入的类型没有实现接口");
    //        }
    //        Type iType = iTypes[0];
    //        if ( ApplicationContainer.Container.IsRegistered(iType))
    //        {
    //            return ApplicationContainer.Container.Resolve(iType);
    //        }

    //        var builder = new ContainerBuilder();
           
    //        RegisterServices<TService>(builder, iType);
    //        builder.Update(ApplicationContainer.Container);
    //        return ApplicationContainer.Container.Resolve(iType);
            
    //    }
    //    private static void RegisterServices<TService>(ContainerBuilder builder,Type iType)
    //    {
    //        builder.RegisterType<TService>().As(new Type[] { iType });
    //    }
    //}
}
