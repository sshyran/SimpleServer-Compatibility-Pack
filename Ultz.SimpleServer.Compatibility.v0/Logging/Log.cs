using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using SimpleServer.Internals;

namespace SimpleServer.Logging
{
    public class Log : ILog
    {
        public const string LogPrefix = "[%time%] [%source%%severity%]: ";

        internal string source;

        internal Log(Type type)
        {
            source = type.Name;
        }

        internal Log(string prefix)
        {
            source = prefix;
        }

        #region Statics

        internal static List<TextWriter> Writers { get; set; }

        internal static void WriteLine(string s)
        {
            Info(s);
        }

        internal static void Info(string s)
        {
            Write("", "INFO", s);
        }

        internal static void Warn(string s)
        {
            Write("", "WARN", s);
        }

        internal static void Severe(string s)
        {
            Write("", "SEVERE", s);
        }

        internal static void Error(Exception ex)
        {
            Write("", "ERROR", ex.ToString());
        }

        internal static void Error(string s)
        {
            Write("", "ERROR", s);
        }

        internal static void Write(string source, string severity, string content)
        {
            foreach (var writer in Writers)
                writer.WriteLine(LogPrefix.Replace("%time%", DateTime.Now.ToString("HH:mm:ss"))
                                     .Replace("%source%", source + (string.IsNullOrEmpty(source) ? "" : "/"))
                                     .Replace("%severity%", severity) + content);
        }

        public static ILog GetLogger()
        {
            SimpleServer.Initialize();
            var frame = new StackFrame(1);
            var method = frame.GetMethod();
            var type = method.DeclaringType;
            var name = method.Name;
            return new Log(type);
        }

        public static void AddWriter(TextWriter writer)
        {
            SimpleServer.Initialize();
            Writers.Add(writer);
        }

        internal static ILog GetLogger(SimpleServerHost host)
        {
            SimpleServer.Initialize();
            return new Log(host.FQDN);
        }

        #endregion
    }
}