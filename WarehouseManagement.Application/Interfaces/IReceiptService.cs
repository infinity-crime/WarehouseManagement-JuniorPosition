using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WarehouseManagement.Application.Common;
using WarehouseManagement.Application.DTOs;
using WarehouseManagement.Application.DTOs.Commands;

namespace WarehouseManagement.Application.Interfaces
{
    /// <summary>
    /// Общий интерфейс для работы с документом поступления и ресурсом поступления (объединение логики)
    /// </summary>
    public interface IReceiptService
    {
        // Метод для основной (совмещенной) таблицы => объединение данных
        Task<Result<IEnumerable<ReceiptRecordDto>>> GetReceiptRecordsAsync(ReceiptRecordFilter filter);

        // Методы для документов поступления
        Task<Result<ReceiptDocumentDto>> GetDocumentByIdAsync(Guid documentId);
        Task<Result<Guid>> CreateDocumentAsync(CreateDocumentCommand command);
        Task<Result> UpdateDocumentAsync(UpdateDocumentCommand command);
        Task<Result> DeleteDocumentAsync(Guid id);
    }

    public class ReceiptRecordFilter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; };
        public List<string> DocumentNumbers { get; set; } = new();
        public List<Guid> ResourceIds { get; set; } = new();
        public List<Guid> UnitIds { get; set; } = new();
    }
}
