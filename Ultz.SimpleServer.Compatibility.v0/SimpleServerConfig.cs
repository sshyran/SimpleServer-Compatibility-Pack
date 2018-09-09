namespace SimpleServer
{
    public class SimpleServerConfig
    {
        /// <summary>
        ///     Declines all requests from HTTP 1.0, accepting only HTTP 1.1 requests and HTTP 2 if enabled.
        /// </summary>
        public static bool Http11Only { get; set; }

        /// <summary>
        ///     Creates an experimental instance of SimpleServerEngine that runs HTTP 2 along with the traditional HTTP 1.X engines
        /// </summary>
        public static bool Http2Subsystem { get; set; }

        /// <summary>
        ///     This will treat send exceptions as warnings rather than errors, keeping the log relatively clutter free.
        /// </summary>
        public static bool IgnoreSendExceptions { get; set; }

        /// <summary>
        ///     When true, exceptions will not be thrown, but logged.
        /// </summary>
        public static bool LogOnly { get; set; }

        internal static void InitializeDefault()
        {
        }
    }
}