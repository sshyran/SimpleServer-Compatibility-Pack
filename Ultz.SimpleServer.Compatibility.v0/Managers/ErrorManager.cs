using System;
using System.IO;
using System.Reflection;
using SimpleServer.Internals;

namespace SimpleServer.Managers
{
    public class ErrorManager
    {
        public static ErrorPage ErrorPage { get; set; } = new ErrorPage
        {
            TypeInAssembly = typeof(ErrorManager),
            NamespaceUrlOfType = "SimpleServer.Providers.ErrorPages.ErrorPage.html"
        };

        public static void Error404(SimpleServerContext context)
        {
            context.Response.StatusCode = 404;
            context.Response.ReasonPhrase = "Not Found";
            var stream = Assembly.GetAssembly(ErrorPage.TypeInAssembly)
                .GetManifestResourceStream(ErrorPage.NamespaceUrlOfType);
            var sr = new StreamReader(stream);
            var sw = new StreamWriter(context.Response.OutputStream);
            sw.WriteLine(sr.ReadToEnd().Replace("[Header]", "404 Not Found").Replace("[ErrorDetail]",
                "We couldn't handle your request because a resource doesn't exist at the requested URL. If you think this is wrong, contact the website administrator.<br><br>Your Request: " +
                context.Request.Method.Name + " " + context.Request.RawUrl));
            sw.Flush();
            context.Response.Close();
        }
        public static void Error(SimpleServerContext context,int code,string reason, string detail)
        {
            context.Response.StatusCode = code;
            context.Response.ReasonPhrase = reason;
            var stream = Assembly.GetAssembly(ErrorPage.TypeInAssembly)
                .GetManifestResourceStream(ErrorPage.NamespaceUrlOfType);
            var sr = new StreamReader(stream);
            var sw = new StreamWriter(context.Response.OutputStream);
            sw.WriteLine(sr.ReadToEnd().Replace("[Header]", code+" "+reason).Replace("[ErrorDetail]",
                detail+"<br><br>Your Request: " +
                context.Request.Method.Name + " " + context.Request.RawUrl));
            sw.Flush();
            context.Response.Close();
        }
    }

    public class ErrorPage
    {
        /// <summary>
        ///     A type in the target assembly where the error page is stored.
        /// </summary>
        public Type TypeInAssembly { get; set; }

        /// <summary>
        ///     The namespace and filename of the error page template. Example: SimpleServer.Providers.ErrorPages.ErrorPage.html
        /// </summary>
        public string NamespaceUrlOfType { get; set; }
    }
}