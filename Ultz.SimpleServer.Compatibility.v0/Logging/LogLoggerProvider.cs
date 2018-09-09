using System;
using Microsoft.Extensions.Logging;

namespace SimpleServer.Logging
{
    public class LogLoggerProvider : ILoggerProvider
    {
        private class Logger : ILogger
        {
            private string _source;
            public Logger(string source = null)
            {
                _source = source;
            }
            
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                switch (logLevel)
                {
                    case LogLevel.Trace:
                        Logging.Log.Write(_source,"TRACE",formatter(state,exception));
                        break;
                    case LogLevel.Debug:
                        Logging.Log.Write(_source,"DEBUG",formatter(state,exception));
                        break;
                    case LogLevel.Information:
                        Logging.Log.Write(_source,"INFO",formatter(state,exception));
                        break;
                    case LogLevel.Warning:
                        Logging.Log.Write(_source,"WARN",formatter(state,exception));
                        break;
                    case LogLevel.Error:
                        Logging.Log.Write(_source,"ERROR",formatter(state,exception));
                        break;
                    case LogLevel.Critical:
                        Logging.Log.Write(_source,"SEVERE",formatter(state,exception));
                        break;
                    case LogLevel.None:
                        Logging.Log.Write(_source,"NONE",formatter(state,exception));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
                }
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return new EmptyDisposable();
            }

            private class EmptyDisposable : IDisposable
            {
                public void Dispose()
                {
                }
            }
        }

        public void Dispose()
        {
            
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new Logger();
        }
    }
}