using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Infrastructure.Data.Repositories
{
    public class ReceiptResourceRepository : Repository<ReceiptResource>, IReceiptResourceRepository
    {
        public ReceiptResourceRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> IsResourceUsedAsync(Guid resourceId)
        {
            return await _context.ReceiptResources.AnyAsync(rr => rr.ResourceId == resourceId);
        }

        public async Task<bool> IsUnitOfMeasureUsedAsync(Guid unitOfMeasureId)
        {
            return await _context.ReceiptResources.AnyAsync(rr => rr.UnitOfMeasureId == unitOfMeasureId);
        }
    }
}
