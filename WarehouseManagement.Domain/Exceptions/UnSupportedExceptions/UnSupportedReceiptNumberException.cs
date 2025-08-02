using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Domain.Exceptions.UnSupportedExceptions
{
    public class UnSupportedReceiptNumberException : Exception
    {
        public UnSupportedReceiptNumberException(string receiptNumber) : base(receiptNumber) { }
    }
}
