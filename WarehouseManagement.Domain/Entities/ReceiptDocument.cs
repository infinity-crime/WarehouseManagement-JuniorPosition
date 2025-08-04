using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WarehouseManagement.Domain.Common;
using WarehouseManagement.Domain.Exceptions;
using WarehouseManagement.Domain.ValueObjects;

namespace WarehouseManagement.Domain.Entities
{
    public class ReceiptDocument : BaseEntity<Guid>
    {
        public ReceiptNumber Number { get; private set; }
        public DateTime Date { get; private set; }

        private readonly List<ReceiptResource> _receiptResources = new();
        public IReadOnlyCollection<ReceiptResource> ReceiptResources => _receiptResources;

#pragma warning disable CS8618
        private ReceiptDocument() { }
#pragma warning restore CS8618

        public static ReceiptDocument Create(string number, DateTime date)
        {
            return new ReceiptDocument
            {
                Id = Guid.NewGuid(),
                Number = ReceiptNumber.Create(number),
                Date = date
            };
        }

        public void AddResource(Resource resource, UnitOfMeasure unit, decimal amount)
        {
            if (_receiptResources.Any(r => r.ResourceId == resource.Id))
                throw new DomainInvalidOperationException($"Такой ресурс поступления уже добавлен!");

            var receiptResource = ReceiptResource.Create(resource.Id, unit.Id, this.Id, amount);

            _receiptResources.Add(receiptResource);
        }

        public void DeleteResource(Guid receiptResourceId)
        {
            var receiptResource = _receiptResources.FirstOrDefault(r => r.Id == receiptResourceId);

            if(receiptResource != null)
                _receiptResources.Remove(receiptResource);
        }

        public void ChangeNumber(string number) => Number = ReceiptNumber.Create(number);

        public void ChangeDate(DateTime date) => Date = date;
    }
}
