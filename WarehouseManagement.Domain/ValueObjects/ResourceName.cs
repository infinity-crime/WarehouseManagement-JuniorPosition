using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagement.Domain.Common;
using WarehouseManagement.Domain.Exceptions.UnSupportedExceptions;

namespace WarehouseManagement.Domain.ValueObjects
{
    public sealed class ResourceName : ValueObject
    {
        public string Value { get; }

        private ResourceName(string name) => Value = name;

        public static ResourceName Create(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName) || resourceName.Length > 255)
                throw new UnSupportedResourceNameException(resourceName);

            return new ResourceName(resourceName);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
