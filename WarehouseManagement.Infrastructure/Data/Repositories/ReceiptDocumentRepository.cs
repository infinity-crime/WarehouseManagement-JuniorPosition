using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Infrastructure.Data.Repositories
{
    public class ReceiptDocumentRepository : Repository<ReceiptDocument>, IReceiptDocumentRepository
    {
        public ReceiptDocumentRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<ReceiptDocument>> GetFilteredDocumentsAsync(
            DateTime? startDate, 
            DateTime? endDate, 
            List<string> docNumbers, 
            List<Guid> resourceIds, 
            List<Guid> unitIds)
        {
            var query = _context.ReceiptDocuments
                .Include(d => d.ReceiptResources)
                    .ThenInclude(rr => rr.Resource)
                .Include(d => d.ReceiptResources)
                    .ThenInclude(rr => rr.UnitOfMeasure)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(d => d.Date >= startDate.Value.Date);

            if (endDate.HasValue)
                query = query.Where(d => d.Date <= endDate.Value.Date.AddDays(1).AddTicks(-1));

            if (docNumbers != null && docNumbers.Any())
                query = query.Where(d => docNumbers.Contains(d.Number.Value));

            if (resourceIds != null && resourceIds.Any())
                query = query.Where(d => d.ReceiptResources.Any(rr => resourceIds.Contains(rr.ResourceId)));

            if (unitIds != null && unitIds.Any())
                query = query.Where(d => d.ReceiptResources.Any(rr => unitIds.Contains(rr.UnitOfMeasureId)));

            return await query.ToListAsync();
        }
    }
}
