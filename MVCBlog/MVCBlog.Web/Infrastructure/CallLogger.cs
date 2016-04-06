using Autofac;
using Castle.DynamicProxy;
using MVCBlog.Service;
using MVCBlog.Service.Interfaces;
using MVCBlog.Web.CommonHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Web.Infrastructure
{
    public class CallLogger : IInterceptor
    {
        //IBase<T> baseservice;
        //public CallLogger(IBase<T> service)
        //{
        //    baseservice = service;
        //}
        public CallLogger()
        {

        }

        public void Intercept(IInvocation invocation)
        {
            string name = invocation.Method.Name;
            var attributes = invocation.Method.GetCustomAttributes(true);
            var arrguate = invocation.Arguments.Select(a => (a ?? "").ToString());
            string arr = string.Join(",", arrguate);
            string attrs = attributes != null && attributes.Count() > 0 ? string.Join(",", attributes.Select(x => x.ToString())) : string.Empty;
            log4net.LogManager.GetLogger("info").Info(string.Format("name:{0},arr:{1},att:{2}", name, arr, attrs));
            invocation.Proceed();
           
            var modifyType = GetHandlerAttribute(invocation);
            if (modifyType != null)
            {
                var interfaces = invocation.MethodInvocationTarget.ReflectedType.GetInterfaces();

                var servciceinterface = interfaces.FirstOrDefault(x => x.GetInterfaces().Select(e => e.Name).Any(s => s.Contains("IBase")));
                var service = ApplicationContainer.Container.Resolve(servciceinterface);

                var models = invocation.Arguments.FirstOrDefault(x => x.GetType().IsClass);
                //var modeltype = models.GetType();
                //var baseservice = (IBase<object>)service;
                //string key1 = baseservice.GetModelKey(models);
                //var method = invocation.MethodInvocationTarget.ReflectedType.GetMethod("GetModelKey");
                if (models != null)
                {
                    try
                    {
                        string key = service.GetType().GetMethod("GetModelKey").Invoke(service, new object[] { models }).ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }
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
    }
}
