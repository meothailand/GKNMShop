using GiaiKhatNgocMai.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http;
using Newtonsoft.Json;

namespace GiaiKhatNgocMai.Infrastructure.Implementation
{
    public class SerializerHelper : ISerializer
    {
        private static ISerializer serializer;
        private SerializerHelper() { }
        public static ISerializer Get()
        {
            serializer = new SerializerHelper();
            return serializer;
        }
        public string SerializeToString<T>(T value)
        {
            var result = JsonConvert.SerializeObject(value, Formatting.Indented, 
                                                     GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings);
            return result;
        }

        public T Deserialize<T>(string value)
        {
            var result = JsonConvert.DeserializeObject<T>(value, GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings);
            return result;
        }

    }
}