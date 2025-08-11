using Blog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.EntityConfigurations.Collections
{
    public class CollectionsEntitiesConfiguration : IEntityTypeConfiguration<Bookmark>, IEntityTypeConfiguration<ReadingList>, IEntityTypeConfiguration<ReadingListItem>
    {
        public void Configure(EntityTypeBuilder<Bookmark> builder)
        {
            builder.ToTable("bookmarks");
            builder.HasKey(x => new { x.UserId, x.PostId });
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        }

        public void Configure(EntityTypeBuilder<ReadingList> builder)
        {
            builder.ToTable("reading_lists");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
        }

        public void Configure(EntityTypeBuilder<ReadingListItem> builder)
        {
            builder.ToTable("reading_list_items");
            builder.HasKey(x => new { x.ReadingListId, x.PostId });
            builder.HasIndex(x => new { x.ReadingListId, x.SortOrder });
        }
    }
}
