using System;
using Blog.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.EntityConfigurations.User;

public class UserEntityConfiguration : BaseEntityConfiguration<Blog.Api.Domain.Models.User>
{
    public override void Configure(EntityTypeBuilder<Blog.Api.Domain.Models.User> builder)
    {
        base.Configure(builder);

        builder.ToTable("user");

        builder.Property(p => p.FirstName)
               .IsRequired()
               .HasMaxLength(200);


        builder.Property(p => p.LastName)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(i => i.UserName)
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(p => p.Password)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(i => i.EmailAddress)
               .IsRequired()
               .HasMaxLength(200);

    }
}

