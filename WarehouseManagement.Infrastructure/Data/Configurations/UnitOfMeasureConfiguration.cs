using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Infrastructure.Data.Configurations
{
    public class UnitOfMeasureConfiguration : IEntityTypeConfiguration<UnitOfMeasure>
    {
        public void Configure(EntityTypeBuilder<UnitOfMeasure> builder)
        {
            builder.ToTable("UnitsOfMeasure");

            builder.HasKey(uom => uom.Id);

            builder.Property(uom => uom.Currency)
                .HasConversion(currency => currency.Value, value => UnitOfMeasureName.Create(value))
                .HasColumnName("Currency")
                .IsRequired()
                .HasMaxLength(10);

            builder.HasIndex(uom => uom.Currency).IsUnique();
        }
    }
}
