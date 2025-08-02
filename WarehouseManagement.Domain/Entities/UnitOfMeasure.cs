using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WarehouseManagement.Domain.Common;
using WarehouseManagement.Domain.Enums;
using WarehouseManagement.Domain.ValueObjects;

namespace WarehouseManagement.Domain.Entities
{
    public class UnitOfMeasure : BaseEntity<Guid>
    {
        public UnitOfMeasureName Currency { get; private set; }
        public Status UnitOfMeasureState { get; private set; }

        private UnitOfMeasure() { }

        public UnitOfMeasure(string currency)
        {
            Currency = UnitOfMeasureName.Create(currency);
            UnitOfMeasureState = Status.InWork;
        }

        public void ChangeUnitOfMeasureState(Status status) => UnitOfMeasureState = status;
    }
}
