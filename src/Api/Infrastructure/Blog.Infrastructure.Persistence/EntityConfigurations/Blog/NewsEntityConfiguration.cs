using Blog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.EntityConfigurations.Blog
{
    public class NewsEntityConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.ToTable("news");
            builder.HasKey(x => x.ID);

            builder.Property(x => x.Title).HasMaxLength(300).IsRequired();
            builder.Property(x => x.Summary).HasMaxLength(500);
            builder.Property(x => x.SourceName).HasMaxLength(200);
            builder.Property(x => x.SourceUrl).HasMaxLength(1000);
            builder.Property(x => x.Status).HasMaxLength(20).HasDefaultValue("draft");
            // Tags basit metin alanı, uzun olabilir
            builder.Property(x => x.Tags).HasColumnType("nvarchar(max)");

            builder.HasIndex(x => x.PublishedAt);
            builder.HasIndex(x => new { x.Status, x.PublishedAt });
            builder.HasIndex(x => x.SourceUrl)
                   .IsUnique()
                   .HasFilter("[SourceUrl] IS NOT NULL");
        }
    }
}
