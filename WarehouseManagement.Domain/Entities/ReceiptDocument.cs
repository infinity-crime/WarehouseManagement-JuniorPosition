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
        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            private set => _date = value.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
                : value.ToUniversalTime();
        }

        public ICollection<ReceiptResource> ReceiptResources { get; set; } = new List<ReceiptResource>();

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

        public void AddResource(ReceiptResource receiptResource)
        {
            if (ReceiptResources.Any(r => r.ResourceId == receiptResource.ResourceId))
                throw new DomainInvalidOperationException($"Такой ресурс поступления уже добавлен!");

            ReceiptResources.Add(receiptResource);
        }

        public void DeleteResource(Guid receiptResourceId)
        {
            var receiptResource = ReceiptResources.FirstOrDefault(r => r.Id == receiptResourceId);

            if(receiptResource != null)
                ReceiptResources.Remove(receiptResource);
        }

        public void ClearResources()
        {
            if(ReceiptResources.Count < 1)
                return;

            ReceiptResources.Clear();
        }

        public void ChangeNumber(string number) => Number = ReceiptNumber.Create(number);

        public void ChangeDate(DateTime date) => Date = date;
    }
}
