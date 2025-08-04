using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WarehouseManagement.Application.Common;
using WarehouseManagement.Application.DTOs;
using WarehouseManagement.Application.Interfaces;
using WarehouseManagement.Domain.Entities;
using WarehouseManagement.Domain.Interfaces;
using WarehouseManagement.Domain.Enums;
using WarehouseManagement.Domain.Exceptions;
using WarehouseManagement.Domain.Exceptions.UnSupportedExceptions;
using WarehouseManagement.Domain.ValueObjects;

namespace WarehouseManagement.Application.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IResourceRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ResourceService(IResourceRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ResourceDto>> AddResourceAsync(string name)
        {
            if (await _repository.ExistsByNameAsync(name))
                return Result<ResourceDto>.Failure("Запись с таким ресурсом уже существует!");

            try
            {
                var resource = Resource.Create(name);
                await _repository.AddAsync(resource);
                await _unitOfWork.CommitAsync();

                return Result<ResourceDto>.Success(_mapper.Map<ResourceDto>(resource));
            }
            catch(UnSupportedResourceNameException ex)
            {
                return Result<ResourceDto>.Failure($"Неккоректное имя ресурса: {ex.Message}");
            }
        }

        public async Task<Result> DeleteResourceAsync(Guid resourceId)
        {
            var resource = await _repository.GetByIdAsync(resourceId);
            if (resource is null)
                return Result.Failure("Ресурс не найден!");

            await _repository.DeleteAsync(resourceId);
            await _unitOfWork.CommitAsync();

            return Result<ResourceDto>.Success();
        }

        public async Task<Result<ResourceDto>> MoveToArchive(Guid resourceId)
        {
            var resource = await _repository.GetByIdAsync(resourceId);
            if(resource is null)
                return Result<ResourceDto>.Failure("Невозможно перенести в архив несуществующую запись!");

            if(resource.ResourceState == Status.Archive)
                return Result<ResourceDto>.Failure("Ресурс уже в архиве!");

            resource.ChangeResourceState(Status.Archive);
            await _repository.UpdateAsync(resource);
            await _unitOfWork.CommitAsync();

            return Result<ResourceDto>.Success(_mapper.Map<ResourceDto>(resource));
        }

        public async Task<Result<ResourceDto>> UpdateResourceAsync(Guid resourceId, string name)
        {
            var resource = await _repository.GetByIdAsync(resourceId);
            if (resource is null)
                return Result<ResourceDto>.Failure("Записи с таким ресурсом не существует!");

            try
            {
                var resourceName = ResourceName.Create(name);

                resource.ChangeResourceName(resourceName);

                await _repository.UpdateAsync(resource);
                await _unitOfWork.CommitAsync();

                return Result<ResourceDto>.Success(_mapper.Map<ResourceDto>(resource));
            }
            catch (UnSupportedResourceNameException ex)
            {
                return Result<ResourceDto>.Failure($"Неккоректное имя ресурса: {ex.Message}");
            }
        }
    }
}