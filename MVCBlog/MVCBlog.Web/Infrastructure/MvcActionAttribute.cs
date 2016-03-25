using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
namespace MVCBlog.Web.Infrastructure
{
    public class MvcActionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string control = filterContext.RouteData.Values["controller"].ToString();
            string action = filterContext.RouteData.Values["action"].ToString();

            var request = filterContext.RequestContext.HttpContext.Request;
            if (request.HttpMethod.Equals("post", StringComparison.CurrentCultureIgnoreCase))
            {
                try
                {
                    var requestList = new SortedList<string, string>();
                    foreach (var x in request.Params.AllKeys)
                    {
                        requestList.Add(x, request.Params[x]);
                    }
                    string content = JsonConvert.SerializeObject(requestList);
                    log4net.LogManager.GetLogger("api").Info(string.Format("{0}:{1}:{2}", control, action, content));
                }
                catch
                {

                }
            }
            base.OnActionExecuted(filterContext);
        }
    }
}
