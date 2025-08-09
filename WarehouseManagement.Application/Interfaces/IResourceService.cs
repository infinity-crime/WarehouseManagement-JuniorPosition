using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WarehouseManagement.Application.Common;
using WarehouseManagement.Application.Common.Interfaces;
using WarehouseManagement.Application.DTOs;
using WarehouseManagement.Domain.Entities;

namespace WarehouseManagement.Application.Interfaces
{
    public interface IResourceService : IDirectoryService<Result<ResourceDto>>
    {
        Task<Result<IEnumerable<ResourceDto>>> GetAllActiveResourcesAsync();
        Task<Result<IEnumerable<ResourceDto>>> GetAllArchiveResourcesAsync();
    }
}
