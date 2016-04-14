using System;

namespace SharpCqrs.Metadata
{
    public interface IDataTypeMetadata
    {
        Type ClrType { get; }

        string DomainModel { get; }

        string Version { get; }

        bool IsObsolete { get; }
    }
}