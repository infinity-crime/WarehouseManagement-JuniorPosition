using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WarehouseManagement.Domain.Common;
using WarehouseManagement.Domain.Entities;
using WarehouseManagement.Domain.Exceptions.UnSupportedExceptions;

namespace WarehouseManagement.Domain.ValueObjects
{
    public class UnitOfMeasureName : ValueObject
    {
        public string Value { get; }

        private UnitOfMeasureName(string name) => Value = name;

        public static UnitOfMeasureName Create(string unitOfMeasureName)
        {
            if (string.IsNullOrEmpty(unitOfMeasureName) || unitOfMeasureName.Length != 3)
                throw new UnSupportedUnitOfMeasureNameException(unitOfMeasureName);

            return new UnitOfMeasureName(unitOfMeasureName);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
