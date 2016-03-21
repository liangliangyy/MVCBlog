using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Service
{
    public class ModelCacheEventArgs : EventArgs
    {
        public string Key { get; set; }
        public int ID { get; set; }
    }
}
