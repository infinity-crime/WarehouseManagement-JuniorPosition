using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Application.DTOs.Commands
{
    public class UpdateDocumentCommand
    {
        public string OtherNumber { get; set; } = default!;
        public DateTime OtherDate { get; set; }
    }
}
