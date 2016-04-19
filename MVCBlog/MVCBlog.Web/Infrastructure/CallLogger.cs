using Autofac;
using Newtonsoft.Json;
using Castle.DynamicProxy;
using MVCBlog.Service;
using MVCBlog.Service.Interfaces;
using MVCBlog.Web.CommonHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MVCBlog.CacheManager;
using MVCBlog.Entities.Models;
using Autofac.Core;
using MVCBlog.Repository;
using Autofac.Integration.Mvc;

namespace MVCBlog.Web.Infrastructure
{
    public class CallLogger : IInterceptor
    {

        private ModelModifyType? GetHandlerAttribute(IInvocation invocation)
        {
            var attrs = invocation.MethodInvocationTarget.GetCustomAttributes(true);
            object custormeAttr = attrs.FirstOrDefault(x => x.GetType() == typeof(ModelHandlerAttribute));
            if (custormeAttr != null)
            {
                var attrgetAttribte = (ModelHandlerAttribute)custormeAttr;
                return attrgetAttribte.modifyType;
            }
            return null;
        }
        public IEnumerable<KeyValuePair<string, object>> MapParameters(object[] arguments, ParameterInfo[] getParameters)
        {
            for (int i = 0; i < arguments.Length; i++)
            {
                yield return new KeyValuePair<string, object>(getParameters[i] == null ? string.Empty : getParameters[i].Name, arguments[i] == null ? new object() : arguments[i]);
            }
        }

        public void Intercept(IInvocation invocation)
        {
            string name = invocation.Method.Name;

            var mappedParameters = MapParameters(invocation.Arguments, invocation.Method.GetParameters())
            .ToDictionary(x => x.Key, x => x.Value.ToString());
            string parameters = JsonConvert.SerializeObject(mappedParameters);
            LogHelper.WriteLog(LogType.INFO, string.Format("Before Method Call. Name:{0},Parameters:{1}", name, parameters));

            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(LogType.EXCEPTION, string.Format("Method Call Have Exception. Name:{0},Exception:{1}", name, ex.ToString()));
                throw ex;
            }

            Common.ThreadHelper.StartAsync(() =>
           {

               string returnValue = JsonConvert.SerializeObject(invocation.ReturnValue);
               LogHelper.WriteLog(LogType.INFO, string.Format("After Method Call. Name:{0},Rerutn Value:{1}", name, returnValue));


               var attributes = invocation.Method.GetCustomAttributes(true);
               string attrs = attributes != null && attributes.Count() > 0 ? string.Join(",", attributes.Select(x => x.ToString())) : string.Empty;
               var modifyType = GetHandlerAttribute(invocation);
               if (modifyType != null)
               {
                   var interfaces = invocation.MethodInvocationTarget.ReflectedType.GetInterfaces();

                   var servciceinterface = interfaces.FirstOrDefault(x => x.GetInterfaces().Select(e => e.Name).Any(s => s.Contains("IBase")));

                   Action<string, object> HandlerModelUpdateCreateCache = (k, m) =>
                   {
                       using (var scope2 = ApplicationContainer.Container.BeginLifetimeScope())
                       {
                           var service2 = scope2.Resolve(servciceinterface, new TypedParameter(typeof(MVCBlogContext), new MVCBlogContext()));
                           RedisHelper.DeleteEntity(k);
                           var idProperties = m.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals("id", StringComparison.OrdinalIgnoreCase));
                           int id = (int)idProperties.GetValue(m);
                           var entity = service2.GetType().GetMethod("GetFromDB").Invoke(service2, new object[] { id });
                           RedisHelper.SetEntity(k, entity);
                           LogHelper.WriteLog(LogType.INFO, string.Format("Model {0}. Before:{1} After:{2}", modifyType.ToString(), JsonConvert.SerializeObject(m), JsonConvert.SerializeObject(entity)));
                       }
                   };
                   using (var scope = ApplicationContainer.Container.BeginLifetimeScope())
                   // AutofacDependencyResolver.Current.ApplicationContainer.BeginLifetimeScope())
                   {
                       var service = scope.Resolve(servciceinterface, new TypedParameter(typeof(MVCBlogContext), new MVCBlogContext()));

                       var model = invocation.Arguments.FirstOrDefault(x => typeof(BaseModel).IsInstanceOfType(x.GetType()));
                       if (model != null)
                       {
                           try
                           {
                               string key = service.GetType().GetMethod("GetModelKey").Invoke(service, new object[] { ((BaseModel)model).Id }).ToString();
                               switch (modifyType)
                               {
                                   case ModelModifyType.Create:
                                       {
                                           HandlerModelUpdateCreateCache(key, model);
                                           break;
                                       }
                                   case ModelModifyType.Delete:
                                       {
                                           RedisHelper.DeleteEntity(key);
                                           LogHelper.WriteLog(LogType.INFO, string.Format("Model {0}. Before:{1}", modifyType.ToString()));
                                           break;
                                       }
                                   case ModelModifyType.Update:
                                       {
                                           HandlerModelUpdateCreateCache(key, model);
                                           break;
                                       }
                                   default:
                                       break;
                               }

                           }
                           catch (Exception ex)
                           {
                               LogHelper.WriteLog(LogType.EXCEPTION, string.Format("Intercept Exception.{0}.ex:{1}", invocation.ToString(), ex.ToString()));
                               throw ex;
                           }
                       }
                   }
               }
           });
        }
    }
}
