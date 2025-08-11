using System;

namespace Blog.Api.Domain.Models
{
    public class PostDailyMetric : BaseEntity
    {
        public Guid PostId { get; set; }
        public DateOnly Date { get; set; }
        public int Views { get; set; }
        public int Comments { get; set; }
        public int ReactionsLike { get; set; }
        public int ReactionsLaugh { get; set; }
        public int ReactionsClap { get; set; }
    }
}
