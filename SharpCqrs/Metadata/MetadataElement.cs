using System;

namespace SharpCqrs.Metadata
{
    public abstract class MetadataElement
    {
        protected MetadataElement(string name, string description, MetadataVersion version)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (version == null) throw new ArgumentNullException(nameof(version));

            Name = name;
            Description = description;
            Version = version;
        }

        public string Name { get; }
        public string Description { get; }
        public MetadataVersion Version { get; }
        protected internal MetadataElement Parent { get; internal set; }
    }
}