using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WarehouseManagement.Domain.Common;
using WarehouseManagement.Domain.Exceptions.UnSupportedExceptions;

namespace WarehouseManagement.Domain.ValueObjects
{
    public class ReceiptNumber : ValueObject
    {
        public string Value { get; }

        private ReceiptNumber(string number) => Value = number;

        public static ReceiptNumber Create(string receiptNumber)
        {
            if (string.IsNullOrEmpty(receiptNumber))
                throw new UnSupportedReceiptNumberException(receiptNumber);

            return new ReceiptNumber(receiptNumber);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
