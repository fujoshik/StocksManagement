using System;

namespace Analyzer.API.Analyzer.Domain
{
    public class UserDataNotFoundException : Exception
    {
        public UserDataNotFoundException() : base("User data not found")
        {
        }

        public UserDataNotFoundException(string message) : base(message)
        {
        }

        public UserDataNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}