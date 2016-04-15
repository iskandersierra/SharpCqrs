using System.IO;
using Newtonsoft.Json;

namespace SharpCqrs.Infrastructure
{
    public class JsonPortableModelBinder : IPortableModelBinder
    {
        private static readonly JsonSerializerSettings DefaultSettings = new JsonSerializerSettings();
        public JsonSerializerSettings Settings { get; set; }

        public bool Matches(string mediaType, string schema, string schemaVersion)
        {
            if (mediaType != "application/json") return false;

            return true;
        }

        public object Bind(PortableBindingContext context)
        {
            using (var reader = GetReaderFrom(context))
            {
                var serializer = new JsonSerializer();
                var instance = serializer.Deserialize(reader, context.Type);
                return instance;
            }
        }

        private JsonReader GetReaderFrom(PortableBindingContext context)
        {
            TextReader reader = new StreamReader(context.Content, context.Encoding);
            return new JsonTextReader(reader);
        }
    }
}