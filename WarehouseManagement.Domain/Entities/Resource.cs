using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WarehouseManagement.Domain.Common;
using WarehouseManagement.Domain.Enums;
using WarehouseManagement.Domain.ValueObjects;

namespace WarehouseManagement.Domain.Entities
{
    public class Resource : BaseEntity<Guid>
    {
        public ResourceName Name { get; private set; }
        public Status ResourceState { get; private set; }

        private Resource() { }

        public Resource(string name)
        {
            Name = ResourceName.Create(name);
            ResourceState = Status.InWork;
        }
    }
}
