using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Application.Interfaces
{
    public interface IUnitOfMeasureService : IDirectoryService<Result<UnitOfMeasureDto>>
    {
        Task<Result<IEnumerable<UnitOfMeasureDto>>> GetAllUnitsAsync();
        Task<Result<IEnumerable<UnitOfMeasureDto>>> GetAllActiveUnitsAsync();
        Task<Result<IEnumerable<UnitOfMeasureDto>>> GetAllArchiveUnitsAsync();
    }
}
