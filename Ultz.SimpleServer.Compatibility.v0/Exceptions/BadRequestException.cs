using System;

namespace SimpleServer.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException() : base("Bad request.")
        {
        }

        public BadRequestException(string msg) : base(msg)
        {
        }
    }
}