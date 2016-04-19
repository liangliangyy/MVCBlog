using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MVCBlog.Common
{

    public static class ThreadHelper
    {

        public static Task StartAsync(this Action todo)
        {
            return Task.Run(todo);
            //return Task.Factory.StartNew(todo);
        }
        public static Task<T> StartAsync<T>(this Func<T> todo)
        {
            return Task.Run<T>(todo);
            //return Task.Factory.StartNew<T>(todo);
        }
    }
}
