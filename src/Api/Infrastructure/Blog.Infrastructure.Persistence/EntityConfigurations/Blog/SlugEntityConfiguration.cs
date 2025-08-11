using Blog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.EntityConfigurations.Blog
{
    public class SlugEntityConfiguration : IEntityTypeConfiguration<Slug>
    {
        public void Configure(EntityTypeBuilder<Slug> builder)
        {
            builder.ToTable("slugs");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.TargetType).HasMaxLength(20).IsRequired();
            builder.Property(x => x.SlugText).HasMaxLength(200).IsRequired();
            builder.HasIndex(x => x.SlugText).IsUnique();
        }
    }
}
