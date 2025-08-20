using System;

namespace Blog.Common.Infrastructure.Exeptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message, string? type = "https://example.com/probs/not-found")
            : base(message, 404, "Not Found", type)
        {
        }
    }
}
