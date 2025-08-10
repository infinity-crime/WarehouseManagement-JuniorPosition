using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Infrastructure.Data.Repositories
{
    public class ResourceRepository : Repository<Resource>, IResourceRepository
    {
        public ResourceRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            var resources = await _context.Resources.ToListAsync();
            return resources.Any(r => r.Name.Value == name);
        }

        public async Task<IEnumerable<Resource>> GetAllActiveResourcesAsync()
        {
            return await _context.Resources
                .Where(r => r.ResourceState == Status.InWork)
                .ToListAsync();
        }

        public async Task<IEnumerable<Resource>> GetAllArchiveResourcesAsync()
        {
            return await _context.Resources
                .Where(r => r.ResourceState == Status.Archive)
                .ToListAsync();
        }
    }
}
