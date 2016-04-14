using System.Collections.Generic;

namespace SharpCqrs.Metadata
{
    public interface ISolutionMetadata
    {
        IReadOnlyCollection<IDomainMetadata> Domains { get; }

        IEnumerable<IDataTypeMetadata> GetAllCommands();
    }
}