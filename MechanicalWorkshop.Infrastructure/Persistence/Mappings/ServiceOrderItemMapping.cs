using MechanicalWorkshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MechanicalWorkshop.Infrastructure.Persistence.Mappings;

public class ServiceOrderItemMapping : IEntityTypeConfiguration<ServiceOrderItem>
{
    public void Configure(EntityTypeBuilder<ServiceOrderItem> builder)
    {
        builder.ToTable("service_order_items");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property(x => x.ServiceOrderId).HasColumnName("service_order_id").IsRequired();
        builder.Property(x => x.ServiceId).HasColumnName("service_id");
        builder.Property(x => x.PartId).HasColumnName("part_id");
        builder.Property(x => x.Description).HasColumnName("description").HasMaxLength(200).IsRequired();
        builder.Property(x => x.Quantity).HasColumnName("quantity").IsRequired();
        builder.Property(x => x.UnitPrice).HasColumnName("unit_price").HasPrecision(10, 2).IsRequired();

        builder.Ignore(x => x.TotalPrice);

        builder.HasOne(x => x.Service)
               .WithMany()
               .HasForeignKey(x => x.ServiceId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Part)
               .WithMany()
               .HasForeignKey(x => x.PartId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}