using MechanicalWorkshop.Domain.Entities;
using MechanicalWorkshop.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MechanicalWorkshop.Infrastructure.Persistence.Mappings;

public class ServiceOrderMapping : IEntityTypeConfiguration<ServiceOrder>
{
    public void Configure(EntityTypeBuilder<ServiceOrder> builder)
    {
        builder.ToTable("service_orders");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property(x => x.OrderNumber).HasColumnName("order_number").HasMaxLength(30).IsRequired();
        builder.Property(x => x.CustomerId).HasColumnName("customer_id").IsRequired();
        builder.Property(x => x.VehicleId).HasColumnName("vehicle_id").IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasConversion<string>().IsRequired();
        builder.Property(x => x.Notes).HasColumnName("notes").HasMaxLength(1000);
        builder.Property(x => x.TotalAmount).HasColumnName("total_amount").HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        builder.Property(x => x.StartedAt).HasColumnName("started_at");
        builder.Property(x => x.FinishedAt).HasColumnName("finished_at");
        builder.Property(x => x.DeliveredAt).HasColumnName("delivered_at");

        builder.HasIndex(x => x.OrderNumber).IsUnique();

        builder.HasOne(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.CustomerId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Vehicle)
               .WithMany()
               .HasForeignKey(x => x.VehicleId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Items)
               .WithOne()
               .HasForeignKey(x => x.ServiceOrderId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Items)
               .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}