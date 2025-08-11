using Blog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.EntityConfigurations.Blog
{
    public class PostEntityConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("posts");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Title).HasMaxLength(300).IsRequired();
            builder.Property(x => x.Status).HasMaxLength(20).HasDefaultValue("draft");

            builder.HasIndex(x => x.PublishedAt);
            builder.Property(x => x.Excerpt).HasMaxLength(500);
        }
    }
}
