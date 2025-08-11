using Blog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.EntityConfigurations.Blog
{
    public class ReactionEntityConfiguration : IEntityTypeConfiguration<Reaction>
    {
        public void Configure(EntityTypeBuilder<Reaction> builder)
        {
            builder.ToTable("reactions");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.TargetType).HasMaxLength(20).IsRequired();
            builder.Property(x => x.Type).HasMaxLength(20).IsRequired();
            builder.HasIndex(x => new { x.TargetType, x.TargetId });
            builder.HasIndex(x => new { x.TargetType, x.TargetId, x.UserId, x.Type }).IsUnique();
        }
    }
}
