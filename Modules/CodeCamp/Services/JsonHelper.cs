
using System;
using System.IO;
using System.Web.Script.Serialization;

namespace WillStrohl.Modules.CodeCamp.Services
{
    public static class JsonHelper
    {
        private static int MAX_LENGTH = Int32.MaxValue;

        public static string ObjectToJson(this object target)
        {
            var ser = new JavaScriptSerializer();

            ser.MaxJsonLength = MAX_LENGTH;
            
            return ser.Serialize(target);
        }

        public static T ObjectFromJson<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
                return default(T);

            var ser = new JavaScriptSerializer();

            ser.MaxJsonLength = MAX_LENGTH;
            
            return ser.Deserialize<T>(json);
        }

        public static T ObjectFromJson<T>(Stream stream)
        {
            var rdr = new StreamReader(stream);
            
            return ObjectFromJson<T>(rdr.ReadToEnd());
        }
    }
}