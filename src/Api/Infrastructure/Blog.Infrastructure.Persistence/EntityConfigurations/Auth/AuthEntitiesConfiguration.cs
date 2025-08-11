using Blog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Persistence.EntityConfigurations.Auth
{
    public class AuthEntitiesConfiguration : IEntityTypeConfiguration<Role>,
                                             IEntityTypeConfiguration<Permission>,
                                             IEntityTypeConfiguration<UserRole>,
                                             IEntityTypeConfiguration<RolePermission>,
                                             IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        }

        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("permissions");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Key).HasMaxLength(150).IsRequired();
            builder.HasIndex(x => x.Key).IsUnique();
        }

        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("user_roles");
            builder.HasKey(x => new { x.UserId, x.RoleId });
            builder.HasIndex(x => x.RoleId);
        }

        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("role_permissions");
            builder.HasKey(x => new { x.RoleId, x.PermissionId });
            builder.HasIndex(x => x.PermissionId);
        }

        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_tokens");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Token).HasMaxLength(500).IsRequired();
            builder.HasIndex(x => new { x.UserId, x.Token }).IsUnique();
        }
    }
}
