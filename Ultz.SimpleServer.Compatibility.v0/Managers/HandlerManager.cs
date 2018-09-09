using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleServer.Handlers;
using SimpleServer.Internals;

namespace SimpleServer.Managers
{
    public class HandlerManager
    {
        private static readonly Dictionary<SimpleServer, HandlerManager> submanagers =
            new Dictionary<SimpleServer, HandlerManager>();

        private readonly List<IHandler> handlers = new List<IHandler>();

        private HandlerManager()
        {
            handlers = new List<IHandler>();
        }

        public static HandlerManager For(SimpleServer server)
        {
            if (!submanagers.ContainsKey(server)) submanagers.Add(server, new HandlerManager());

            return submanagers[server];
        }

        public HandlerManager With(SimpleServerHost host, params IHandler[] handlers)
        {
            foreach (var handler in handlers) Add(host, handler);

            return this;
        }

        public void Add(SimpleServerHost host, IHandler handler)
        {
            handlers.Add(handler);
        }

        public async Task HandleAsync(SimpleServerContext ctx)
        {
            await ctx.Request.Host.Handlers.ForEachAsync(async x =>
            {
                if (x.CanHandle(ctx.Request) && !ctx.Handled)
                    await Task.Factory.StartNew(() =>
                    {
                        x.Handle(ctx);
                        ctx.Handled = true;
                    });
            });
            if (!ctx.Handled)
                ErrorManager.Error404(ctx);
        }
    }
}