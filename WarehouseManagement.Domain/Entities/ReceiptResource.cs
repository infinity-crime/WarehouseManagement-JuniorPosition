using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using WarehouseManagement.Domain.Common;
using WarehouseManagement.Domain.ValueObjects;

namespace WarehouseManagement.Domain.Entities
{
    public class ReceiptResource : BaseEntity<Guid>
    {
        public Guid ResourceId { get; private set; }
        public Guid UnitOfMeasureId { get; private set; }
        public Guid ReceiptDocumentId { get; private set; }
        
        public Resource Resource { get; private set; }
        public UnitOfMeasure UnitOfMeasure { get; private set; }
        public ReceiptDocument Document { get; private set; }

        public AmountResource Amount { get; private set; }

        private ReceiptResource() { }

        public static ReceiptResource Create(Guid resourceId, Guid unitOfMeasureId, Guid receiptDocumentId, decimal amount)
        {
            return new ReceiptResource
            {
                Id = Guid.NewGuid(),
                ResourceId = resourceId,
                UnitOfMeasureId = unitOfMeasureId,
                ReceiptDocumentId = receiptDocumentId,
                Amount = AmountResource.Create(amount)
            };
        }
    }
}
