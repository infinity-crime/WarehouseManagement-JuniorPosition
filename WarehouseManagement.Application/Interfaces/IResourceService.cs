using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WarehouseManagement.Application.Common;
using WarehouseManagement.Application.DTOs;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Application.Interfaces
{
    public interface IResourceService
    {
        Task<Result<ResourceDto>> AddResourceAsync(string name);
        Task<Result<ResourceDto>> UpdateResourceAsync(Guid resourceId, string name);
        Task<Result> DeleteResourceAsync(Guid resourceId);
        Task<Result<ResourceDto>> MoveToArchive(Guid resourceId);
    }
}
