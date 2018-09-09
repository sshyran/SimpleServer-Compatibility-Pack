using System.IO;
using SimpleServer;
using SimpleServer.Handlers;
using SimpleServer.Internals;

namespace TestApp
{
    public class TestHandler : IHandler
    {
        public bool CanHandle(SimpleServerRequest request)
        {
            if (request.Method == "GET" && request.FormattedUrl == "/")
                return true;
            if (request.Method == "POST" && request.FormattedUrl == "/") return true;

            return false;
        }

        public void Handle(SimpleServerContext context)
        {
            if (context.Request.FormattedUrl == "/")
                if (context.Request.Method == "GET")
                {
                    var sw = new StreamWriter(context.Response.OutputStream);
                    sw.WriteLine(
                        "<h1>Hello, World!</h1><form action=\"/\" method=\"POST\"><a>What's your name?</a><input type=\"text\" name=\"name\" value=\"Dylan\" /><input type=\"submit\" /></form>");
                    sw.Flush();
                    sw.Close();
                    context.Response.Close();
                }
                else if (context.Request.Method == "POST")
                {
                    var sr = new StreamReader(context.Request.InputStream);
                    var query = sr.ReadToEnd().AsFormParameters();
                    sr.Close();
                    var sw = new StreamWriter(context.Response.OutputStream);
                    sw.WriteLine("<h1>Hello, " + query["name"] + "</h1><a href=\"/\">< Return</a>");
                    sw.Flush();
                    sw.Close();
                    context.Response.Close();
                }
        }
    }
}