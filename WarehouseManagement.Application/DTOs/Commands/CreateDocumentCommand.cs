using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Application.DTOs.Commands
{
    public class CreateDocumentCommand
    {
        public string Number { get; set; } = default!;
        public DateTime Date { get; set; }
        public List<ReceiptResourceItemDto>? Resources { get; set; }
    }
}
