using Blog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.EntityConfigurations.Blog
{
    public class ProjectCategoryEntityConfiguration : IEntityTypeConfiguration<ProjectCategory>
    {
        public void Configure(EntityTypeBuilder<ProjectCategory> builder)
        {
            builder.ToTable("project_categories");
            
            // Composite primary key
            builder.HasKey(pc => new { pc.ProjectId, pc.CategoryId });
            
            // Foreign keys without navigation properties
            builder.Property(pc => pc.ProjectId).IsRequired();
            builder.Property(pc => pc.CategoryId).IsRequired();
            
            // Indexes
            builder.HasIndex(pc => pc.ProjectId);
            builder.HasIndex(pc => pc.CategoryId);
        }
    }
}