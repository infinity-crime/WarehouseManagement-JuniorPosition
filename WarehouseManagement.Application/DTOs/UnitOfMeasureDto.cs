using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagement.Domain.Enums;

namespace WarehouseManagement.Application.DTOs
{
    /// <summary>
    /// DTO для справочника единиц измерения
    /// </summary>
    public class UnitOfMeasureDto
    {
        public Guid Id { get; set; }
        public string Currency { get; set; } = default!;
        public Status State { get; set; }
    }
}
