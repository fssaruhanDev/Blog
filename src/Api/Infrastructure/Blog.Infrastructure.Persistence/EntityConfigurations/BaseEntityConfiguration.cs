using System;
using Blog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(i => i.ID);

        builder.Property(i => i.ID).ValueGeneratedOnAdd();

        builder.Property(e => e.CreatedDate)
               .HasColumnType("datetime2")
               .IsRequired();

        builder.Property(e => e.UpdatedDate)
               .HasColumnType("datetime2")
               .IsRequired(false);
    }
}