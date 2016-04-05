using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.ConsoleTest.ProxyTest
{
    public class Person: MarshalByRefObject
    {
        public string Say()
        {
            const string str = "Person's say is called";
            Console.WriteLine(str);
            return str;
        }
    }
}
