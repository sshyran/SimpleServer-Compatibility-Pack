using System;

namespace SimpleServer.Exceptions
{
    public class FunctionNotSupportedException : Exception
    {
        public FunctionNotSupportedException(string functionname, string functiontype) : base(
            "Function " + functionname + " had a type value of " + functiontype +
            ", a function type not present on this server. Are you missing a plugin?")
        {
        }

        public FunctionNotSupportedException(string message) : base(message)
        {
        }
    }
}