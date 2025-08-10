using System;
using System.Collections.Generic;
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

        public ReceiptService(IReceiptDocumentRepository receiptDocumentRepository, IReceiptResourceRepository receiptResourceRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _receiptDocumentRepository = receiptDocumentRepository;
            _receiptResourceRepository = receiptResourceRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

                        document.AddResource(res, unit, resource.Amount);
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
            var document = await _receiptDocumentRepository.GetByIdAsync(documentId);
            if (document is null)
                return Result<ReceiptDocumentDto>.Failure("Такого документа не найдено!");

            return Result<ReceiptDocumentDto>.Success(_mapper.Map<ReceiptDocumentDto>(document));
        }

        public async Task<Result<IEnumerable<ReceiptRecordDto>>> GetReceiptRecordsAsync(ReceiptRecordFilter filter)
        {
            var documents = await _receiptDocumentRepository.GetFilteredDocumentsAsync(
                filter.StartDate,
                filter.EndDate,
                filter.DocumentNumbers,
                filter.ResourceIds,
                filter.UnitIds);

            return Result<IEnumerable<ReceiptRecordDto>>.Success(_mapper.Map<IEnumerable<ReceiptRecordDto>>(documents));
        }

        public async Task<Result> UpdateDocumentAsync(UpdateDocumentCommand command)
        {
            var document = await _receiptDocumentRepository.GetByIdAsync(command.Id);
            if (document == null)
                return Result.Failure("Документ поступления не найден");

            document.ChangeDate(command.Date);

            try
            {
                document.ChangeNumber(command.Number);

                document.ClearResources();

                if (command.Resources != null)
                {
                    foreach (var resource in command.Resources)
                    {
                        var res = await _resourceRepository.GetByIdAsync(resource.ResourceId);
                        var unit = await _unitOfMeasureRepository.GetByIdAsync(resource.UnitId);

                        if (res == null || unit == null)
                            return Result<Guid>.Failure("Ресурс или единица измерения не найдены!");

                        document.AddResource(res, unit, resource.Amount);
                    }
                }
            }
            catch(UnSupportedReceiptNumberException ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result.Failure($"Ошибка обновления документа: {ex.Message}");
            }
            catch(DomainInvalidOperationException ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result.Failure($"Ошибка обновления документа: {ex.Message}");
            }

            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
    }
}
