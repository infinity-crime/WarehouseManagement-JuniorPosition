using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagement.Application.Common;
using WarehouseManagement.Application.Common.Interfaces;
using WarehouseManagement.Application.DTOs;

namespace WarehouseManagement.Application.Interfaces
{
    public interface IUnitOfMeasureService : IDirectoryService<Result<UnitOfMeasureDto>> { }
}
