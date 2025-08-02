using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WarehouseManagement.Domain.Exceptions;

namespace WarehouseManagement.Domain.ValueObjects
{
    public sealed class ResourceName
    {
        public string Value { get; }

        private ResourceName(string name)
        {
            Value = name;
        }

        public static ResourceName Create(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName))
                throw new UnSupportedResourceNameException(resourceName);

            return new ResourceName(resourceName);
        }
    }
}
