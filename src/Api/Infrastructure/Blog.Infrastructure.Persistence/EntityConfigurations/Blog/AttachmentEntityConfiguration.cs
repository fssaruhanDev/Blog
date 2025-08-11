using Blog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.EntityConfigurations.Blog
{
    public class AttachmentEntityConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            builder.ToTable("attachments");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.TargetType).HasMaxLength(20).IsRequired();
            builder.Property(x => x.Purpose).HasMaxLength(20);
            builder.HasIndex(x => new { x.TargetType, x.TargetId, x.SortOrder });
        }
    }
}
