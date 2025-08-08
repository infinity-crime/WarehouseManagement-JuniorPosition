using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagement.Domain.Interfaces;
using WarehouseManagement.Infrastructure.Data.Repositories.Common;

namespace WarehouseManagement.Infrastructure.Data.Repositories
{
    public class UnitOfMeasureRepository : Repository<UnitOfMeasure>, IUnitOfMeasureRepository
    {
        public UnitOfMeasureRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.UnitsOfMeasure.AnyAsync(uom => uom.Currency.Value == name);
        }
    }
}
