using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Ultz.SimpleServer.Internals.Http;

namespace SimpleServer.Internals
{
    public class SimpleServerResponse
    {
        private readonly HttpResponse _response;

        internal SimpleServerResponse(SimpleServerRequest request, HttpResponse response)
        {
            Headers = new Dictionary<string, string>();

            Request = request;
            OutputStream = new MemoryStream();

            Version = Request.Version;
            StatusCode = 200;
            ReasonPhrase = "OK";
            _response = response;
        }


        private SimpleServerRequest Request { get; }

        /// <summary>
        ///     Gets the headers of the HTTP response.
        /// </summary>
        public Dictionary<string, string> Headers { get; }

        /// <summary>
        ///     Gets the stream containing the content of this response.
        /// </summary>
        public Stream OutputStream { get; }

        /// <summary>
        ///     Gets or sets the HTTP version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        ///     Gets or sets the HTTP status code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        ///     Gets or sets the HTTP reason phrase.
        /// </summary>
        public string ReasonPhrase { get; set; }

        /// <summary>
        ///     Writes a string to OutputStream.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public Task WriteContentAsync(string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            return OutputStream.WriteAsync(buffer, 0, buffer.Length);
        }

        /// <summary>
        ///     Closes this response and sends it.
        /// </summary>
        public void Close()
        {
            var outputStream = OutputStream as MemoryStream;
            var memStream = new MemoryStream(outputStream.ToArray());
            _response.StatusCode = StatusCode;
            _response.ReasonPhrase = ReasonPhrase;
            memStream.CopyTo(_response.OutputStream);
            _response.Headers["Content-Length"] = memStream.Length.ToString();
            foreach (var h in Headers) _response.Headers.Add(h.Key,h.Value);
            _response.Close();
        }
        /// <summary>
        ///     Writes a HTTP redirect response.
        /// </summary>
        /// <param name="redirectLocation"></param>
        /// <returns></returns>
        public Task RedirectAsync(Uri redirectLocation)
        {
            StatusCode = 301;
            ReasonPhrase = "Moved permanently";
            Headers["Location"] = redirectLocation.ToString();
            Close();
            return Task.CompletedTask;
        }

        #region IDisposable

        private bool disposedValue;

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                Close();

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}