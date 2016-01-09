using Autofac;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVCBlog.Web.Infrastructure
{
    public class Dependency : IDependencyResolver
    {
        public readonly IContainer container;

        public Dependency(IContainer container)
        {
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            return
                container.IsRegistered(serviceType)
                    ? container.Resolve(serviceType)
                    : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {

            Type enumerableServiceType =
                typeof (IEnumerable<>).MakeGenericType(serviceType);

            object instance =
                container.Resolve(enumerableServiceType);

            return ((IEnumerable) instance).Cast<object>();
        }
    }
}