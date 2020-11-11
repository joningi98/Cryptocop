using System;

namespace Cryptocop.Software.API.Models.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(): base("Model is not properly formatted.") {}
        public ConflictException(string message): base(message) {}
        public ConflictException(string message, Exception inner) : base(message, inner) {}
        
    }
}