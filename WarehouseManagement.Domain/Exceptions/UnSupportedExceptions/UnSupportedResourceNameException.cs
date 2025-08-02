using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Domain.Exceptions.UnSupportedExceptions
{
    public class UnSupportedResourceNameException : Exception
    {
        public UnSupportedResourceNameException(string resourceName) : base(resourceName) { }
    }
}
