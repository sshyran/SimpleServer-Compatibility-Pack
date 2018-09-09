using System.Collections.Generic;
using SimpleServer.Functions;

namespace SimpleServer.Managers
{
    public class FunctionManager
    {
        private static readonly Dictionary<string, Function> types = new Dictionary<string, Function>();

        public static void RegisterType(string typeid, Function dummyInstance)
        {
            types.Add(typeid, dummyInstance);
        }

        public static Function Deserialize(string s)
        {
            // TODO: Function system
            // TODO: Function system
            // TODO: Function system
            // TODO: Function system
            // TODO: Function system
            // TODO: Function system
            //JObject obj = JObject.Parse(s);
            //if (!obj.ContainsKey("Type"))
            //{
            //    throw new JsonParseException("Attempted to parse a function without a type key, the JSON file may be invalid");
            //}
            //if (!obj.ContainsKey("Name"))
            //{
            //    throw new JsonParseException("Attempted to parse a function without a name key, the JSON file may be invalid");
            //}
            //if (!types.ContainsKey(obj["Type"].Value<string>()))
            //{
            //    throw new FunctionNotSupportedException(obj["Name"].Value<string>(), obj["Type"].Value<string>());
            //}
            // TODO: Function system
            return null;
        }
    }
}