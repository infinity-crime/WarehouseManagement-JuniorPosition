using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Domain.Interfaces
{
    public interface IReceiptDocumentRepository : IRepository<ReceiptDocument>
    {
        Task<ReceiptDocument?> GetDocumentByIdWithIncludeAsync(Guid id);
        Task<IEnumerable<ReceiptDocument>> GetFilteredDocumentsAsync(DateTime? startDate, DateTime? endDate, 
            List<string> docNumbers, List<Guid> resourceIds, List<Guid> unitIds);
    }
}
