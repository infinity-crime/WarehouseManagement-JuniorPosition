using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Infrastructure.Data.Repositories
{
    public class UnitOfMeasureRepository : Repository<UnitOfMeasure>, IUnitOfMeasureRepository
    {
        public UnitOfMeasureRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            var units =  await _context.UnitsOfMeasure.ToListAsync();
            return units.Any(uom => uom.Currency.Value == name);
        }

        public async Task<IEnumerable<UnitOfMeasure>> GetAllActiveUnitsAsync()
        {
            return await _context.UnitsOfMeasure
                .Where(uom => uom.UnitOfMeasureState == Domain.Enums.Status.InWork)
                .ToListAsync();
        }

        public async Task<IEnumerable<UnitOfMeasure>> GetAllArchiveUnitsAsync()
        {
            return await _context.UnitsOfMeasure
                .Where(uom => uom.UnitOfMeasureState == Domain.Enums.Status.Archive)
                .ToListAsync();
        }
    }
}
