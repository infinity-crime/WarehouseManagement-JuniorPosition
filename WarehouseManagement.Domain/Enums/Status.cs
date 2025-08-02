using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Domain.Enums
{
    public enum Status : byte
    {
        InWork = 1,
        Archive = 2,
        Deleted = 3
    }
}
