using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
namespace MVCBlog.Web.CommonHelper
{
    public class LogHelper
    {
        public static void WriteLog(LogType logtype, StringBuilder content)
        {
            WriteLog(logtype, content.ToString());
        }
        public static void WriteLog(LogType logtype, string content)
        {
            ILog log = LogManager.GetLogger(logtype.ToString());
            switch (logtype)
            {
                case LogType.API:
                    log.Warn(content);
                    break;
                case LogType.EXCEPTION:
                    log.Error(content);
                    break;
                case LogType.INFO:
                    log.Info(content);
                    break;
                default:
                    log.Info(content);
                    break;
            }

        }
    }
    public enum LogType
    {
        API,
        EXCEPTION,
        INFO
    }
}
