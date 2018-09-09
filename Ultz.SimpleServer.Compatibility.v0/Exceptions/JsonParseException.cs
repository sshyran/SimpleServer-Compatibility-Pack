using System;

namespace SimpleServer.Exceptions
{
    public class JsonParseException : Exception
    {
        public JsonParseException(string message) : base(message)
        {
        }
    }
}