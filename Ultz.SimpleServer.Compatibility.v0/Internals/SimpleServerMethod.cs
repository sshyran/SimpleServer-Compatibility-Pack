using System.Collections.Generic;

namespace SimpleServer.Internals
{
    public class SimpleServerMethod
    {
        public static readonly ICollection<SimpleServerMethod> DefaultMethods =
            new List<SimpleServerMethod> {Get, Head, Post, Put, Delete, Options, Trace, Patch, Connect}.AsReadOnly();

        public static SimpleServerMethod Get => new SimpleServerMethod {Name = "GET", HasInputStream = false};

        public static SimpleServerMethod Post => new SimpleServerMethod {Name = "POST", HasInputStream = true};

        public static SimpleServerMethod Put => new SimpleServerMethod {Name = "PUT", HasInputStream = true};

        public static SimpleServerMethod Patch => new SimpleServerMethod {Name = "PATCH", HasInputStream = true};

        public static SimpleServerMethod Delete => new SimpleServerMethod {Name = "DELETE", HasInputStream = false};

        public static SimpleServerMethod Head => new SimpleServerMethod {Name = "HEAD", HasInputStream = false};

        public static SimpleServerMethod Options => new SimpleServerMethod {Name = "OPTIONS", HasInputStream = false};

        public static SimpleServerMethod Trace => new SimpleServerMethod {Name = "TRACE", HasInputStream = false};

        public static SimpleServerMethod Connect => new SimpleServerMethod {Name = "CONNECT", HasInputStream = false};

        public string Name { get; set; }
        public bool HasInputStream { get; set; }

        public static bool operator ==(SimpleServerMethod ssmethod, string smethod)
        {
            return ssmethod.Name.ToUpper() == smethod;
        }

        public static bool operator !=(SimpleServerMethod ssmethod, string smethod)
        {
            return ssmethod.Name.ToUpper() != smethod;
        }
    }
}