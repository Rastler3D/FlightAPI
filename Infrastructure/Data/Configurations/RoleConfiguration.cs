using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Code)
            .IsRequired()
            .HasMaxLength(256);
            
        builder.HasIndex(r => r.Code)
            .IsUnique();
            
        builder.HasData(
            new { Id = 1, Code = Roles.User },
            new { Id = 2, Code = Roles.Moderator }
        );
    }
}
