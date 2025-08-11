using Blog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.EntityConfigurations.Blog
{
    public class JoinEntitiesConfiguration : IEntityTypeConfiguration<PostCategory>, IEntityTypeConfiguration<PostTag>
    {
        public void Configure(EntityTypeBuilder<PostCategory> builder)
        {
            builder.ToTable("post_categories");
            builder.HasKey(x => new { x.PostId, x.CategoryId });
            builder.HasIndex(x => x.CategoryId);
        }

        public void Configure(EntityTypeBuilder<PostTag> builder)
        {
            builder.ToTable("post_tags");
            builder.HasKey(x => new { x.PostId, x.TagId });
            builder.HasIndex(x => x.TagId);
        }
    }
}
