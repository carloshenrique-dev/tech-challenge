using MechanicalWorkshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MechanicalWorkshop.Infrastructure.Persistence.Mappings;

public class AdminUserMapping : IEntityTypeConfiguration<AdminUser>
{
    public void Configure(EntityTypeBuilder<AdminUser> builder)
    {
        builder.ToTable("admin_users");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
        builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(150).IsRequired();
        builder.Property(x => x.PasswordHash).HasColumnName("password_hash").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        builder.HasIndex(x => x.Email).IsUnique();
    }
}