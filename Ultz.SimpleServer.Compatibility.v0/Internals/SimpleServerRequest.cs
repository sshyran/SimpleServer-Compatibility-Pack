using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Http2.Hpack;
using Ultz.SimpleServer.Internals;
using Ultz.SimpleServer.Internals.Http;

namespace SimpleServer.Internals
{
    public class SimpleServerRequest
    {
        private SimpleServerConnection client;
        internal bool empty;

        internal SimpleServerRequest()
        {
            empty = true;
        }

        internal SimpleServerRequest(IRequest request, SimpleServerConnection connection, SimpleServerHost host)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (host == null)
                throw new ArgumentNullException(nameof(host));
            client = connection;
            empty = false;
            var req = request as HttpRequest;
            LocalEndpoint = connection?.LocalEndPoint;
            RemoteEndpoint = connection?.RemoteEndPoint;
            RequestUri = req?.Url;
            Method = host.Methods.FirstOrDefault(x => x.Name == req?.RawMethod);
            Headers =
                ((IEnumerable<HeaderField>) req?.Headers ?? throw new NullReferenceException("Headers are null"))
                .ToDictionary(x => x.Name, x => x.Value);
            InputStream = req.InputStream;
            Version = req.Protocol;
            RawUrl = req.RawUrl;
            Host = host;
        }

        public static SimpleServerRequest Empty => new SimpleServerRequest();

        /// <summary>
        ///     Gets the endpoint of the listener that received the request.
        /// </summary>
        public IPEndPoint LocalEndpoint { get; internal set; }

        /// <summary>
        ///     Gets the endpoint that sent the request.
        /// </summary>
        public IPEndPoint RemoteEndpoint { get; internal set; }

        /// <summary>
        ///     Gets the URI send with the request.
        /// </summary>
        public Uri RequestUri { get; internal set; }

        /// <summary>
        ///     Gets the HTTP method.
        /// </summary>
        public SimpleServerMethod Method { get; internal set; }

        /// <summary>
        ///     Gets the headers of the HTTP request.
        /// </summary>
        public Dictionary<string, string> Headers { get; internal set; }

        /// <summary>
        ///     Gets the stream containing the content sent with the request.
        /// </summary>
        public Stream InputStream { get; internal set; }

        /// <summary>
        ///     Gets the HTTP version.
        /// </summary>
        public string Version { get; internal set; }

        /// <summary>
        ///     Gets a value indicating whether the request was sent locally or not.
        /// </summary>
        public bool IsLocal => RemoteEndpoint.Address.Equals(LocalEndpoint.Address);

        public SimpleServerConnection Connection { get; internal set; }
        public string RawUrl { get; internal set; }

        public string FormattedUrl => RawUrl.UrlFormat();


        public SimpleServerHost Host { get; set; }

        public static bool IsNullOrEmpty(SimpleServerRequest request)
        {
            return request == null || request.empty;
        }

        /// <summary>
        ///     Reads the content of the request as a string.
        /// </summary>
        /// <returns></returns>
        public async Task<string> ReadContentAsStringAsync()
        {
            var length = InputStream.Length;
            var buffer = new byte[length];
            await InputStream.ReadAsync(buffer, 0, (int) length);
            return Encoding.UTF8.GetString(buffer);
        }
    }
}