using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SimpleServer.Handlers;
using SimpleServer.Logging;
using SimpleServer.Managers;
using Ultz.SimpleServer;
using Ultz.SimpleServer.Common;
using Ultz.SimpleServer.Internals;
using Ultz.SimpleServer.Internals.Http;

namespace SimpleServer.Internals
{
    public class SimpleServerHost : Ultz.SimpleServer.Handlers.IHandler
    {
        public SimpleServerHost()
        {
            Handlers = new List<IHandler>();
            FQDN = "*";
            AliasFQDNs = new List<string>();
            Endpoint = new SimpleServerEndpoint();
            Methods = new List<SimpleServerMethod>(SimpleServerMethod.DefaultMethods);
        }

        public List<IHandler> Handlers { get; set; }
        public string FQDN { get; set; }
        public List<string> AliasFQDNs { get; set; }
        public SimpleServerEndpoint Endpoint { get; set; }
        public List<SimpleServerMethod> Methods { get; set; }
        internal string RequestRegex { get; set; }

        public bool CanHandle(IRequest request)
        {
            var req = new SimpleServerRequest(request, null, this);
            return Handlers.Any(x => x.CanHandle(req));
        }

        public void Handle(IContext context)
        {
            var req = new SimpleServerRequest(context.Request, new SimpleServerConnection(context.Connection), this);
            var res = new SimpleServerResponse(req, ((HttpContext) context).Response);
            var ctx = new SimpleServerContext()
                {Request = req, Response = res, Connection = req.Connection, Handled = false};
            var handler = Handlers.FirstOrDefault(x => x.CanHandle(req));
            if (handler == null)
                ErrorManager.Error404(ctx);
            else
                handler.Handle(ctx);
        }

        internal MinimalServer ToServer()
        {
            var srv = new MinimalServer(Http.Create());
            srv.Endpoints.Add(new IPEndPoint(Endpoint.Scope, Endpoint.Port));
            srv.Handlers.Add(this);
            srv.LoggerProvider = new LogLoggerProvider();
            srv.OnError += SrvOnOnError;
            return srv;
        }

        private void SrvOnOnError(object sender, ErrorEventArgs e)
        {
            Log.Error("" + (int) e.Type + " " + e.Type + ": " + e.CurrentError);
            var req = new SimpleServerRequest(e.Context.Request, new SimpleServerConnection(e.Context.Connection),
                this);
            var res = new SimpleServerResponse(req, ((HttpContext) e.Context).Response);
            var ctx = new SimpleServerContext()
                {Request = req, Response = res, Connection = req.Connection, Handled = false};
            if (e.Type == ErrorType.HandlerNotFound)
                ErrorManager.Error404(ctx);
            else
                ErrorManager.Error(ctx, (int) e.Type, e.Type.ToString(), e.CurrentError.ToString());
        }
    }
}