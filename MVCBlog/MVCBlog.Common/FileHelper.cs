using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Common
{
    public class FileHelper
    {
        public static string GetFileContent(string path)
        {
            try
            {
                string content = File.ReadAllText(path);
                return content;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
