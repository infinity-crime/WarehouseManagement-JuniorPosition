using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Infrastructure.Data.Configurations
{
    public class ReceiptDocumentConfiguration : IEntityTypeConfiguration<ReceiptDocument>
    {
        public void Configure(EntityTypeBuilder<ReceiptDocument> builder)
        {
            builder.ToTable("ReceiptDocuments");

            builder.HasKey(rd => rd.Id);

            builder.Property(rd => rd.Number)
                .HasConversion(number => number.Value, value => ReceiptNumber.Create(value))
                .HasColumnName("Number")
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(rd => rd.Number).IsUnique();

            builder.Property(rd => rd.Date)
                .IsRequired();
        }
    }
}
