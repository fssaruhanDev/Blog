using System;
using System.Runtime.Serialization;

namespace Blog.Infrastructure.Persistence.Exeptions;

    public class DatabaseValidationException : Exception
    {
        public int StatusCode { get; } // HTTP Durum Kodu
        public string ErrorMessage { get; } // Hata Mesajı

        public DatabaseValidationException(string message, int statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
            ErrorMessage = message;
        }

        public DatabaseValidationException(string message, Exception innerException, int statusCode = 400)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ErrorMessage = message;
        }

        // Removed obsolete serialization constructor.
    }

