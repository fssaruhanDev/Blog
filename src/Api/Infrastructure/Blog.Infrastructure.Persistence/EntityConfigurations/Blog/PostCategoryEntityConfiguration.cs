using Blog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.EntityConfigurations.Blog
{
    public class PostCategoryEntityConfiguration : IEntityTypeConfiguration<PostCategory>
    {
        public void Configure(EntityTypeBuilder<PostCategory> builder)
        {
            builder.ToTable("post_categories");
            
            // Composite primary key
            builder.HasKey(pc => new { pc.PostId, pc.CategoryId });
            
            // Foreign keys without navigation properties
            builder.Property(pc => pc.PostId).IsRequired();
            builder.Property(pc => pc.CategoryId).IsRequired();
            
            // Indexes
            builder.HasIndex(pc => pc.PostId);
            builder.HasIndex(pc => pc.CategoryId);
        }
    }
}