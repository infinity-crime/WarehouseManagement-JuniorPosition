using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Application.DTOs
{
    /// <summary>
    /// DTO для окна редактирования документа поступления
    /// </summary>
    public class ReceiptDocumentDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = default!;
        public DateTime Date { get; set; }
        public List<ReceiptResourceItemDto> Resources { get; set; } = new();
    }
}
