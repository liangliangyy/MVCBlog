using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Service
{

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class ModelHandlerAttribute : Attribute
    {
        public ModelModifyType modifyType { get; private set; }
        public ModelHandlerAttribute(ModelModifyType _type)
        {
            modifyType = _type;
        }
    }
    public enum ModelModifyType
    {
        Delete,
        Create,
        Update
    }
}
