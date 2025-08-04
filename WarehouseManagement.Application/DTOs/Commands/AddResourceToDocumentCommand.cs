using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Application.DTOs.Commands
{
    public class AddResourceToDocumentCommand
    {
        public Guid ResourceId { get; set; }
        public Guid UnitId { get; set; }
        public decimal Amount { get; set; }
    }
}
