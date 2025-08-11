using Blog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.EntityConfigurations.Blog
{
    public class CommentEntityConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("comments");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.Status).HasMaxLength(20).HasDefaultValue("pending");

            builder.HasIndex(x => new { x.PostId, x.ParentId, x.CreatedDate });
        }
    }
}
