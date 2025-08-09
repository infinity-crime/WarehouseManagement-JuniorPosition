using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Application.Common.Interfaces
{
    public interface IDirectoryService<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<T> CreateAsync(string name);
        Task<T> ChangeNameAsync(Guid id, string name);
        Task<T> MoveToArchiveAsync(Guid id);
        Task<T> MoveToWorkAsync(Guid id);
        Task<Result> DeleteAsync(Guid id);
    }
}
