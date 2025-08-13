using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Application.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly IReceiptDocumentRepository _receiptDocumentRepository;
        private readonly IReceiptResourceRepository _receiptResourceRepository;
        private readonly IResourceRepository _resourceRepository;
        private readonly IUnitOfMeasureRepository _unitOfMeasureRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReceiptService(IReceiptDocumentRepository receiptDocumentRepository, IResourceRepository resourceRepository, IUnitOfMeasureRepository unitOfMeasureRepository,
            IUnitOfWork unitOfWork, IMapper mapper, IReceiptResourceRepository receiptResourceRepository)
        {
            _receiptDocumentRepository = receiptDocumentRepository;
            _resourceRepository = resourceRepository;
            _unitOfMeasureRepository = unitOfMeasureRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _receiptResourceRepository = receiptResourceRepository;
        }

        public async Task<Result<Guid>> CreateDocumentAsync(CreateDocumentCommand command)
        {
            try
            {
                var document = ReceiptDocument.Create(command.Number, command.Date);
                if(command.Resources != null)
                {
                    foreach(var resource in command.Resources)
                    {
                        var res = await _resourceRepository.GetByIdAsync(resource.ResourceId);
                        var unit = await _unitOfMeasureRepository.GetByIdAsync(resource.UnitId);

                        if (res == null || unit == null)
                            return Result<Guid>.Failure("Ресурс или единица измерения не найдены!");

                        var receiptResource = ReceiptResource.Create(res.Id, unit.Id, document.Id, resource.Amount);
                        document.AddResource(receiptResource);
                    }
                }

                await _receiptDocumentRepository.AddAsync(document);
                await _unitOfWork.CommitAsync();

                return Result<Guid>.Success(document.Id);
            }
            catch(DomainInvalidOperationException ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result<Guid>.Failure($"Введены некорректные данные для документа поступления: {ex.Message}");
            }
        }

        public async Task<Result> DeleteDocumentAsync(Guid id)
        {
            var document = await _receiptDocumentRepository.GetByIdAsync(id);
            if(document == null)
                return Result<ReceiptDocumentDto>.Failure("Такого документа не найдено!");

            await _receiptDocumentRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();

            return Result.Success();
        }

        public async Task<Result<ReceiptDocumentDto>> GetDocumentByIdAsync(Guid documentId)
        {
            var document = await _receiptDocumentRepository.GetDocumentByIdWithIncludeAsync(documentId);
            if (document is null)
                return Result<ReceiptDocumentDto>.Failure("Такого документа не найдено!");

            return Result<ReceiptDocumentDto>.Success(_mapper.Map<ReceiptDocumentDto>(document));
        }

        public async Task<Result<IEnumerable<ReceiptRecordDto>>> GetReceiptRecordsAsync(ReceiptRecordFilter filter)
        {
            var startDate = filter.StartDate.HasValue
                ? filter.StartDate.Value.ToUniversalTime()
                : (DateTime?)null;

            var endDate = filter.EndDate.HasValue
                ? filter.EndDate.Value.ToUniversalTime()
                : (DateTime?)null;

            var documents = await _receiptDocumentRepository.GetFilteredDocumentsAsync(
                startDate,
                endDate,
                filter.DocumentNumbers,
                filter.ResourceIds,
                filter.UnitIds);

            return Result<IEnumerable<ReceiptRecordDto>>.Success(_mapper.Map<IEnumerable<ReceiptRecordDto>>(documents));
        }

        public async Task<Result> UpdateDocumentAsync(UpdateDocumentCommand command)
        {
            var document = await _receiptDocumentRepository.GetDocumentByIdWithIncludeAsync(command.Id);
            if (document == null)
                return Result.Failure("Документ поступления не найден");

            document.ChangeDate(command.Date);

            try
            {
                document.ChangeNumber(command.Number);

                var existingResources = document.ReceiptResources.ToList();

                var incoming = command.Resources ?? new List<ReceiptResourceItemDto>();

                var newItemsToAdd = new List<ReceiptResourceItemDto>();
                foreach (var ir in incoming)
                {
                    if (ir.Id != Guid.Empty)
                    {
                        var exist = existingResources.FirstOrDefault(er => er.Id == ir.Id);
                        if (exist != null)
                        {
                            exist.Update(ir.ResourceId, ir.UnitId, ir.Amount);
                        }
                        else
                        {
                            newItemsToAdd.Add(ir);
                        }
                    }
                    else
                    {
                        newItemsToAdd.Add(ir);
                    }
                }

                var toDelete = existingResources
                    .Where(er => !incoming.Any(ir => ir.Id != Guid.Empty && ir.Id == er.Id))
                    .ToList();

                foreach (var del in toDelete)
                    document.DeleteResource(del.Id);

                foreach (var ir in newItemsToAdd)
                {
                    var res = await _resourceRepository.GetByIdAsync(ir.ResourceId);
                    var unit = await _unitOfMeasureRepository.GetByIdAsync(ir.UnitId);
                    if (res == null || unit == null)
                        return Result.Failure("Ресурс или единица измерения не найдены!");

                    var receiptResource = ReceiptResource.Create(res.Id, unit.Id, document.Id, ir.Amount);

                    document.AddResource(receiptResource);
                    await _receiptResourceRepository.AddAsync(receiptResource);
                }
                
            }
            catch (UnSupportedReceiptNumberException ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result.Failure($"Ошибка обновления документа: {ex.Message}");
            }
            catch (UnSupportedAmountResourceException ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result.Failure($"Ошибка обновления документа: некорректное кол-во ресурса поступления - {ex.Message}");
            }
            catch (DomainInvalidOperationException ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result.Failure($"Ошибка обновления документа: {ex.Message}");
            }

            await _unitOfWork.CommitAsync();
            return Result.Success();
        }
    }
}
