using System;
using System.Collections.Generic;

namespace Blog.Common.Infrastructure.Exeptions
{
    public class ValidationApiException : BadRequestException
    {
        public ValidationApiException(IDictionary<string, string[]> errors)
            : base("One or more validation errors occurred.", errors, "https://example.com/probs/validation")
        {
        }
    }
}
