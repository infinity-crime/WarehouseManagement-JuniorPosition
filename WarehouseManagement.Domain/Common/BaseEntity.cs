using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Domain.Common
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; private set; }
    }
}