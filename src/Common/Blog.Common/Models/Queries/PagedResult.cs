using System;

namespace Blog.Common.Models.Queries
{
    public class PagedResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public T[] Items { get; set; } = Array.Empty<T>();
    }
}
