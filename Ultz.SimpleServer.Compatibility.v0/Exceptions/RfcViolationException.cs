using System;

namespace SimpleServer.Exceptions
{
    public class RfcViolationException : Exception
    {
        public RfcViolationException(string msg) : base(msg)
        {
        }
    }
}