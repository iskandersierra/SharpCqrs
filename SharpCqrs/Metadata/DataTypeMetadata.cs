using System;

namespace SharpCqrs.Metadata
{
    public class DataTypeMetadata : 
        MetadataElement
    {
        public DataTypeMetadata(
            string name, 
            string description, 
            MetadataVersion version, 
            Type clrType) 
            : base(name, description, version)
        {
            if (clrType == null) throw new ArgumentNullException(nameof(clrType));
            ClrType = clrType;
        }

        public Type ClrType { get; }
    }
}