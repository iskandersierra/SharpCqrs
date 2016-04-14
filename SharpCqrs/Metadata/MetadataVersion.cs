using System;

namespace SharpCqrs.Metadata
{
    public class MetadataVersion
    {
        public MetadataVersion(string domainModel, DomainVersion version, bool? isObsolete = null)
        {
            if (domainModel == null) throw new ArgumentNullException(nameof(domainModel));
            if (version == null) throw new ArgumentNullException(nameof(version));
            DomainModel = domainModel;
            Version = version;
            IsObsolete = isObsolete;
        }

        public string DomainModel { get; }
        public DomainVersion Version { get; }
        public bool? IsObsolete { get; }
    }
}