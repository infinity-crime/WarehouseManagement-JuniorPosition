using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Result<ResourceDto>> MoveToArchiveAsync(Guid resourceId)
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

        public async Task<Result<ResourceDto>> GetByIdAsync(Guid id)
        {
            var resource = await _repository.GetByIdAsync(id);
            return Result<ResourceDto>.Success(_mapper.Map<ResourceDto>(resource));
        }

        public async Task<Result<ResourceDto>> MoveToWorkAsync(Guid id)
        {
            var resource = await _repository.GetByIdAsync(id);
            if (resource is null)
                return Result<ResourceDto>.Failure("Невозможно перенести в работу несуществующую запись!");

            if (resource.ResourceState == Status.InWork)
                return Result<ResourceDto>.Failure("Ресурс уже в работе!");

            resource.ChangeResourceState(Status.InWork);
            await _unitOfWork.CommitAsync();

            return Result<ResourceDto>.Success(_mapper.Map<ResourceDto>(resource));
        }

        public async Task<Result<IEnumerable<ResourceDto>>> GetAllActiveResourcesAsync()
        {
            var resources = await _repository.GetAllActiveResourcesAsync();
            return Result <IEnumerable<ResourceDto>>.Success(_mapper.Map<IEnumerable<ResourceDto>>(resources));
        }

        public async Task<Result<IEnumerable<ResourceDto>>> GetAllArchiveResourcesAsync()
        {
            var resources = await _repository.GetAllArchiveResourcesAsync();
            return Result<IEnumerable<ResourceDto>>.Success(_mapper.Map<IEnumerable<ResourceDto>>(resources));
        }

        public async Task<Result<IEnumerable<ResourceDto>>> GetAllResourcesAsync()
        {
            var resources = await _repository.GetAllAsync();
            return Result<IEnumerable<ResourceDto>>.Success(_mapper.Map<IEnumerable<ResourceDto>>(resources));
        }
    }
}