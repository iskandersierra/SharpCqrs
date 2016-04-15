using System;
using System.IO;
using System.Text;

namespace SharpCqrs.Infrastructure
{
    public class PortableBindingContext
    {
        public Type Type { get; set; }
        public string MediaType { get; set; }
        public string Schema { get; set; }
        public string SchemaVersion { get; set; }
        public Encoding Encoding { get; set; }
        public Stream Content { get; set; }
        public object Instance { get; set; }
        public bool IgnoreErrors { get; set; }
        public bool Overwrite { get; set; }
        public string[] BlackList { get; set; }
    }
}