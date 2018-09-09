using System.Linq;
using System.Net;
using SimpleServer.Handlers;
using SimpleServer.Internals;

namespace SimpleServer
{
    public class ServerBuilder
    {
        public SimpleServer Result { get; }

        public HostBuilder NewHost(int port)
        {
            return new HostBuilder(port, this);
        }

        public SimpleServer Build()
        {
            return Result;
        }

        public SimpleServer BuildAndStart()
        {
            Result.Start();
            return Result;
        }

        #region Statics

        public static ServerBuilder NewServer()
        {
            return new ServerBuilder();
        }

        private ServerBuilder()
        {
            Result = new SimpleServer();
        }

        #endregion
    }

    public class HostBuilder
    {
        private readonly ServerBuilder builder;

        internal HostBuilder(int port, ServerBuilder parent)
        {
            builder = parent;
            Result = new SimpleServerHost();
            Result.Endpoint = new SimpleServerEndpoint {Scope = IPAddress.Loopback, Port = port};
        }

        public SimpleServerHost Result { get; set; }

        public HostBuilder At(IPAddress scope)
        {
            Result.Endpoint.Scope = scope;
            return this;
        }

        public ServerBuilder AddToServer()
        {
            builder.Result.Hosts.Add(Result);
            return builder;
        }

        public HostBuilder For(string fqdn)
        {
            Result.FQDN = fqdn;
            return this;
        }

        public HostBuilder AlsoFor(string fqdn)
        {
            Result.AliasFQDNs.Add(fqdn);
            return this;
        }

        public HostBuilder For(params string[] fqdns)
        {
            Result.FQDN = fqdns.First();
            var aslist = fqdns.ToList();
            foreach (var fqdn in fqdns)
            {
                if (aslist.IndexOf(fqdn) == 0) continue;

                Result.AliasFQDNs.Add(fqdn);
            }

            return this;
        }

        public HostBuilder With(params IHandler[] handlers)
        {
            foreach (var handler in handlers) Result.Handlers.Add(handler);

            return this;
        }
    }
}