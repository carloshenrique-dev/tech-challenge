using MechanicalWorkshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MechanicalWorkshop.Infrastructure.Persistence.Mappings;

public class VehicleMapping : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("vehicles");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property(x => x.LicensePlate).HasColumnName("license_plate").HasMaxLength(10).IsRequired();
        builder.Property(x => x.Brand).HasColumnName("brand").HasMaxLength(50).IsRequired();
        builder.Property(x => x.Model).HasColumnName("model").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Year).HasColumnName("year").IsRequired();
        builder.Property(x => x.CustomerId).HasColumnName("customer_id").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(x => x.LicensePlate).IsUnique();
    }
}