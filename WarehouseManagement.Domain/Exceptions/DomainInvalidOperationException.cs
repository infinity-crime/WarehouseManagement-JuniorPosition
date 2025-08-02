using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Domain.Exceptions
{
    public class DomainInvalidOperationException : Exception
    {
        public DomainInvalidOperationException(string message) : base(message) { }
    }
}
