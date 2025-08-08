using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Application.DTOs
{
    /// <summary>
    /// DTO для основной таблицы в поступлениях
    /// </summary>
    public class ReceiptRecordDto
    {
        public Guid DocumentId { get; set; }
        public string DocumentNumber { get; set; } = default!;
        public DateTime DocumentDate { get; set; }
        public List<ReceiptResourceItemDto> ReceiptResourceItemDtos { get; set; } = new();
    }
}
