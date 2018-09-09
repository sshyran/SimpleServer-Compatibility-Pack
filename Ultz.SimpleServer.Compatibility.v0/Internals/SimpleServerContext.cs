namespace SimpleServer.Internals
{
    public class SimpleServerContext
    {
        internal bool Handled { get; set; } = false;
        public SimpleServerRequest Request { get; set; }
        public SimpleServerResponse Response { get; set; }
        public SimpleServerConnection Connection { get; set; }
    }
}