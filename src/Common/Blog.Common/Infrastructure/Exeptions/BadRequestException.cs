using System;
using System.Collections.Generic;

namespace Blog.Common.Infrastructure.Exeptions
{
    public class BadRequestException : ApiException
    {
        public BadRequestException(string message, IDictionary<string, string[]>? errors = null, string? type = "https://example.com/probs/bad-request")
            : base(message, 400, "Bad Request", type, errors)
        {
        }
    }
}
