using System.Collections.Generic;

namespace SharpCqrs.Metadata
{
    public interface IDomainMetadata
    {
        IReadOnlyCollection<IAggregateMetadata> Aggregates { get; }

        IEnumerable<IDataTypeMetadata> GetAllCommands();
    }
}