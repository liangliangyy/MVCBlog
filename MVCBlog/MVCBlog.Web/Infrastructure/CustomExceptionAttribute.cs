using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVCBlog.Web.Infrastructure
{
    public class CustomExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled == true)
            {
                return;
            }
            //filterContext.ExceptionHandled = true;
            Exception exception = filterContext.Exception;

            HttpException httpException = new HttpException(null, exception);

            log4net.LogManager.GetLogger("exception").Error(filterContext.Exception.ToString());

            //filterContext.ExceptionHandled = true;
            //filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            
        }
    }
}
