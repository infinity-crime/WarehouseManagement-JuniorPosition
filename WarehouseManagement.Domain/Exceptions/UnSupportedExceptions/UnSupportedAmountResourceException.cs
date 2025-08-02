using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Domain.Exceptions.UnSupportedExceptions
{
    public class UnSupportedAmountResourceException : Exception
    {
        public UnSupportedAmountResourceException(string amountResource) : base(amountResource) { }
    }
}
