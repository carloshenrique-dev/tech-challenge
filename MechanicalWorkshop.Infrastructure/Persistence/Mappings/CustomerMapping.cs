using MechanicalWorkshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MechanicalWorkshop.Infrastructure.Persistence.Mappings;

public class CustomerMapping : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
        builder.Property(x => x.Document).HasColumnName("document").HasMaxLength(18).IsRequired();
        builder.Property(x => x.DocumentType).HasColumnName("document_type").HasMaxLength(4).IsRequired();
        builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(150).IsRequired();
        builder.Property(x => x.Phone).HasColumnName("phone").HasMaxLength(20).IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(x => x.Document).IsUnique();
        builder.HasIndex(x => x.Email).IsUnique();

        builder.HasMany(x => x.Vehicles)
               .WithOne(x => x.Customer)
               .HasForeignKey(x => x.CustomerId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}