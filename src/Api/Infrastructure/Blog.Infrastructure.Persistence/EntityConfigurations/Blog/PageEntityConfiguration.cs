using Blog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.EntityConfigurations.Blog
{
    public class PageEntityConfiguration : IEntityTypeConfiguration<Page>
    {
        public void Configure(EntityTypeBuilder<Page> builder)
        {
            builder.ToTable("pages");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Slug).IsRequired().HasMaxLength(180);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(300);
            builder.Property(x => x.Excerpt).HasMaxLength(500);
            builder.Property(x => x.Status).IsRequired().HasMaxLength(20).HasDefaultValue("draft");
            builder.Property(x => x.MetaTitle).HasMaxLength(300);
            builder.Property(x => x.MetaDescription).HasMaxLength(500);
            builder.HasIndex(x => x.Slug).IsUnique();
            builder.HasIndex(x => x.PublishedAt);
            builder.HasIndex(x => new { x.Status, x.PublishedAt });
        }
    }
}
