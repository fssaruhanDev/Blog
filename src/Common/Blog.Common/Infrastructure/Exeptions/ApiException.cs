using System;
using System.Collections.Generic;

namespace Blog.Common.Infrastructure.Exeptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public string? Title { get; }
        public string? Type { get; }
        public IDictionary<string, string[]>? Errors { get; }

        public ApiException(string message, int statusCode = 500, string? title = null, string? type = null, IDictionary<string, string[]>? errors = null)
            : base(message)
        {
            StatusCode = statusCode;
            Title = title;
            Type = type;
            Errors = errors;
        }
    }
}
