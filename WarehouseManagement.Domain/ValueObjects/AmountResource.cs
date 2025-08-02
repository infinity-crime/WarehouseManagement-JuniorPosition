using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WarehouseManagement.Domain.Common;
using WarehouseManagement.Domain.Exceptions.UnSupportedExceptions;

namespace WarehouseManagement.Domain.ValueObjects
{
    public class AmountResource : ValueObject
    {
        public decimal Value { get; }

        private AmountResource(decimal amount) => Value = amount;

        public static AmountResource Create(decimal amountResource)
        {
            if(amountResource <= 0)
                throw new UnSupportedAmountResourceException(amountResource.ToString());

            return new AmountResource(amountResource);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
