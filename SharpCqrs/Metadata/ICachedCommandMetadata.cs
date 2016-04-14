using System;

namespace SharpCqrs.Metadata
{
    public interface ICachedCommandMetadata
    {
        bool IsClrTypeValidCommand(Type type);

        ProvideMetadataResult ProvideCommandMetadata(MetadataVersion version);
    }
}
