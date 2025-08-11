using Blog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.EntityConfigurations.Blog
{
    public class MediaEntityConfiguration : IEntityTypeConfiguration<Media>
    {
        public void Configure(EntityTypeBuilder<Media> builder)
        {
            builder.ToTable("media");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.FileName).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Url).HasMaxLength(1024).IsRequired();
            builder.Property(x => x.MimeType).HasMaxLength(150).IsRequired();
        }
    }
}
