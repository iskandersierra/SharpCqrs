using System.Collections.Generic;

namespace SharpCqrs.Metadata
{
    public interface IAggregateMetadata
    {
        IReadOnlyCollection<IDataTypeMetadata> Commands { get; }
        IReadOnlyCollection<IDataTypeMetadata> Events { get; }
        IReadOnlyCollection<IDataTypeMetadata> States { get; }
    }
}