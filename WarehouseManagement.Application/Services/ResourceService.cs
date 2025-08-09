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
        private readonly IReceiptResourceRepository _receiptRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ResourceService(IResourceRepository repository, IMapper mapper, IUnitOfWork unitOfWork, IReceiptResourceRepository receiptResourceRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _receiptRepository = receiptResourceRepository;
        }

        public async Task<Result<ResourceDto>> CreateAsync(string name)
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
                await _unitOfWork.RollbackAsync();
                return Result<ResourceDto>.Failure($"Некорректное имя ресурса: {ex.Message}");
            }
        }

        public async Task<Result> DeleteAsync(Guid resourceId)
        {
            var resource = await _repository.GetByIdAsync(resourceId);
            if (resource is null)
                return Result.Failure("Ресурс не найден!");

            if (await _receiptRepository.IsResourceUsedAsync(resourceId))
                return Result.Failure("Ресурс уже используется в системе!");

            await _repository.DeleteAsync(resourceId);
            await _unitOfWork.CommitAsync();

            return Result.Success();
        }

        public async Task<Result<ResourceDto>> MoveToArchive(Guid resourceId)
        {
            var resource = await _repository.GetByIdAsync(resourceId);
            if(resource is null)
                return Result<ResourceDto>.Failure("Невозможно перенести в архив несуществующую запись!");

            if(resource.ResourceState == Status.Archive)
                return Result<ResourceDto>.Failure("Ресурс уже в архиве!");

            resource.ChangeResourceState(Status.Archive);
            await _unitOfWork.CommitAsync();

            return Result<ResourceDto>.Success(_mapper.Map<ResourceDto>(resource));
        }

        public async Task<Result<ResourceDto>> ChangeNameAsync(Guid resourceId, string name)
        {
            var resource = await _repository.GetByIdAsync(resourceId);
            if (resource is null)
                return Result<ResourceDto>.Failure("Записи с таким ресурсом не существует!");

            try
            {
                resource.ChangeResourceName(name);

                await _unitOfWork.CommitAsync();

                return Result<ResourceDto>.Success(_mapper.Map<ResourceDto>(resource));
            }
            catch (UnSupportedResourceNameException ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result<ResourceDto>.Failure($"Некорректное имя ресурса: {ex.Message}");
            }
        }

        public Task<Result<ResourceDto>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ResourceDto>> MoveToArchiveAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ResourceDto>> MoveToWorkAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}