using Blog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.EntityConfigurations.Analytics
{
    public class AnalyticsEntitiesConfiguration : IEntityTypeConfiguration<View>, IEntityTypeConfiguration<PostDailyMetric>
    {
        public void Configure(EntityTypeBuilder<View> builder)
        {
            builder.ToTable("views");
            builder.HasKey(x => x.ID);
            builder.HasIndex(x => new { x.PostId, x.CreatedDate });
        }

        public void Configure(EntityTypeBuilder<PostDailyMetric> builder)
        {
            builder.ToTable("post_daily_metrics");
            builder.HasKey(x => x.ID);
            builder.HasIndex(x => new { x.PostId, x.Date }).IsUnique();
        }
    }
}
