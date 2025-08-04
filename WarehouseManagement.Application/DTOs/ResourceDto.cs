using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WarehouseManagement.Domain.Enums;

namespace WarehouseManagement.Application.DTOs
{
    /// <summary>
    /// DTO для справочника ресурсов
    /// </summary>
    public class ResourceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public Status State { get; set; }
    }
}
