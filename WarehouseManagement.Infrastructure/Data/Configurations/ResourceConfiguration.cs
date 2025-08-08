using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Infrastructure.Data.Configurations
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.ToTable("Resources");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .HasConversion(name => name.Value, value => ResourceName.Create(value))
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(r => r.Name).IsUnique();
        }
    }
}
