using Blog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.EntityConfigurations.Blog
{
    public class RedirectEntityConfiguration : IEntityTypeConfiguration<Redirect>
    {
        public void Configure(EntityTypeBuilder<Redirect> builder)
        {
            builder.ToTable("redirects");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.SourcePath).HasMaxLength(500).IsRequired();
            builder.HasIndex(x => x.SourcePath).IsUnique();
        }
    }
}
