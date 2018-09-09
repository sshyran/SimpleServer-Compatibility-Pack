using SimpleServer.Internals;

namespace SimpleServer.Handlers
{
    public interface IHandler
    {
        bool CanHandle(SimpleServerRequest request);
        void Handle(SimpleServerContext context);
    }
}