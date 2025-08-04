using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Domain.Interfaces
{
    public interface IReceiptResourceRepository : IRepository<ReceiptResource>
    {
        Task<bool> IsResourceUsedAsync(Guid resourceId);
        Task<bool> IsUnitOfMeasureUsedAsync(Guid unitOfMeasureId);
    }
}
