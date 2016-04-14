using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SharpCqrs.Metadata
{
    public class SolutionMetadata : ISolutionMetadata
    {
        public SolutionMetadata(params IDomainMetadata[] domains)
        {
            Domains = new ReadOnlyCollection<IDomainMetadata>(domains ?? new IDomainMetadata[0]);
        }

        public IReadOnlyCollection<IDomainMetadata> Domains { get; }

        public IEnumerable<IDataTypeMetadata> GetAllCommands()
        {
            yield break;
        }
    }
}
