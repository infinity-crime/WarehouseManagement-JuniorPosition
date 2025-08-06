using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Application.DTOs
{
    /// <summary>
    /// DTO для окна редактирования самих ресурсов поступления
    /// </summary>
    public class ReceiptResourceItemDto
    {
        public Guid Id { get; set; }
        public Guid ReceiptDocumentId { get; set; }
        public Guid ResourceId { get; set; }
        public string ResourceName { get; set; } = default!;
        public Guid UnitId { get; set; }
        public string UnitName { get; set; } = default!;
        public decimal Amount { get; set; }
    }
}
