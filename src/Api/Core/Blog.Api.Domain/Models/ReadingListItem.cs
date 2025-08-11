using System;

namespace Blog.Api.Domain.Models
{
    public class ReadingListItem
    {
        public Guid ReadingListId { get; set; }
        public Guid PostId { get; set; }
        public int? SortOrder { get; set; }
    }
}
