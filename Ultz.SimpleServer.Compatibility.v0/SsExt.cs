using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleServer.Internals;

namespace SimpleServer
{
    public static class SsExt
    {
        public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> func)
        {
            foreach (var value in list) await func(value);
        }

        public static string UrlFormat(this string s)
        {
            if (s.Contains("?"))
                return s.Split('?')[0].Split('/').Last().Contains(".") ? s : s.EndsWith("/") ? s : s + "/";
            return s.Split('/').Last().Contains(".") ? s : s.EndsWith("/") ? s : s + "/";
        }

        public static Dictionary<string, string> AsFormParameters(this string s)
        {
            string[] parts;
            parts = s.Contains("&") ? s.Split('&') : new string[1] {s};

            return parts.Select(part => part.Split('=')).ToDictionary(keyval => keyval[0], keyval => keyval[1]);
        }

        public static SimpleServerMethod Get(this IEnumerable<SimpleServerMethod> list, string method)
        {
            return list.FirstOrDefault(mthd => mthd == method);
        }
    }
}