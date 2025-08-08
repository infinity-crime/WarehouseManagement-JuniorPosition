using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Domain.Interfaces
{
    public interface IUnitOfMeasureRepository : IRepository<UnitOfMeasure>
    {
        Task<bool> ExistsByNameAsync(string name);
    }
}
