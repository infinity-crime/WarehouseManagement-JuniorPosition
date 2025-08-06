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
using WarehouseManagement.Domain.Enums;
using WarehouseManagement.Domain.Exceptions.UnSupportedExceptions;
using WarehouseManagement.Domain.Interfaces;

namespace WarehouseManagement.Application.Services
{
    public class UnitOfMeasureService : IUnitOfMeasureService
    {
        private readonly IUnitOfMeasureRepository _repository;
        private readonly IReceiptResourceRepository _receiptRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UnitOfMeasureService(IUnitOfMeasureRepository repository, IUnitOfWork unitOfWork, IMapper mapper, IReceiptResourceRepository receiptRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _receiptRepository = receiptRepository;
        }

        public async Task<Result<UnitOfMeasureDto>> CreateAsync(string name)
        {
            if(await _repository.ExistsByNameAsync(name))
                return Result<UnitOfMeasureDto>.Failure("Запись с такой единицей измерения уже существует!");

            try
            {
                var measure = UnitOfMeasure.Create(name);
                await _repository.AddAsync(measure);
                await _unitOfWork.CommitAsync();

                return Result<UnitOfMeasureDto>.Success(_mapper.Map<UnitOfMeasureDto>(measure));
            }
            catch(UnSupportedUnitOfMeasureNameException ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result<UnitOfMeasureDto>.Failure($"Некорректное имя единицы измерения: {ex.Message}");
            }
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var measure = await _repository.GetByIdAsync(id);
            if (measure is null)
                return Result.Failure("Eдиница измерения не найдена!");

            if(await _receiptRepository.IsUnitOfMeasureUsedAsync(id))
                return Result.Failure("Единица измерения уже используется в системе!");

            await _repository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();

            return Result.Success();
        }

        public async Task<Result<UnitOfMeasureDto>> ChangeNameAsync(Guid id, string name)
        {
            var measure = await _repository.GetByIdAsync(id);
            if (measure is null)
                return Result<UnitOfMeasureDto>.Failure("Записи с такой единицей измерения не существует!");

            try
            {
                measure.ChangeUnitOfMeasureName(name);

                await _repository.UpdateAsync(measure);
                await _unitOfWork.CommitAsync();

                return Result<UnitOfMeasureDto>.Success(_mapper.Map<UnitOfMeasureDto>(measure));
            }
            catch (UnSupportedUnitOfMeasureNameException ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result<UnitOfMeasureDto>.Failure($"Некорректное имя единицы измерения: {ex.Message}");
            }
        }

        public async Task<Result<UnitOfMeasureDto>> MoveToArchive(Guid id)
        {
            var measure = await _repository.GetByIdAsync(id);
            if (measure is null)
                return Result<UnitOfMeasureDto>.Failure("Невозможно перенести в архив несуществующую запись!");

            if (measure.UnitOfMeasureState == Status.Archive)
                return Result<UnitOfMeasureDto>.Failure("Единица измерения уже в архиве!");

            measure.ChangeUnitOfMeasureState(Status.Archive);
            await _repository.UpdateAsync(measure);
            await _unitOfWork.CommitAsync();

            return Result<UnitOfMeasureDto>.Success(_mapper.Map<UnitOfMeasureDto>(measure));
        }
    }
}
