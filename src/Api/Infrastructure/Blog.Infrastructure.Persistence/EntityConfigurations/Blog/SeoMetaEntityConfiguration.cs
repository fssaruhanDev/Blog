using Blog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.EntityConfigurations.Blog
{
    public class SeoMetaEntityConfiguration : IEntityTypeConfiguration<SeoMeta>
    {
        public void Configure(EntityTypeBuilder<SeoMeta> builder)
        {
            builder.ToTable("seo_meta");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.TargetType).HasMaxLength(20).IsRequired();
            builder.Property(x => x.MetaTitle).HasMaxLength(300);
            builder.Property(x => x.MetaDescription).HasMaxLength(500);
        }
    }
}
