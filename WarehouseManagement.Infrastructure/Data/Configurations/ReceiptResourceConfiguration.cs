using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Infrastructure.Data.Configurations
{
    public class ReceiptResourceConfiguration : IEntityTypeConfiguration<ReceiptResource>
    {
        public void Configure(EntityTypeBuilder<ReceiptResource> builder)
        {
            builder.ToTable("ReceiptResources");

            builder.HasKey(rr => rr.Id);

            builder.Property(rr => rr.Amount)
                .HasConversion(amount => amount.Value, value => AmountResource.Create(value))
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Amount")
                .IsRequired();

            builder.HasOne(rr => rr.Document)
                .WithMany(rd => rd.ReceiptResources)
                .HasForeignKey(rr => rr.ReceiptDocumentId)
                .OnDelete(DeleteBehavior.Cascade); // при удалении документа - удаляются используемые в нем ресурсы поступления

            builder.HasOne(rr => rr.Resource)
                .WithMany()
                .HasForeignKey(rr => rr.ResourceId)
                .OnDelete(DeleteBehavior.Restrict); // запрет удаления ресурса (на уровне БД), если он где-то используется

            builder.HasOne(rr => rr.UnitOfMeasure)
                .WithMany()
                .HasForeignKey(rr => rr.UnitOfMeasureId)
                .OnDelete(DeleteBehavior.Restrict); // запрет удаления ед. изм. (на уровне БД), если они где-то используются
        }
    }
}
