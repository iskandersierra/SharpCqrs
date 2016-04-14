using System.Collections.Generic;
using System.Linq;

namespace SharpCqrs.Metadata
{
    public static class SolutionMetadataExtensions
    {

        public static IEnumerable<DataTypeMetadata> GetAllCommands(this SolutionMetadata self)
        {
            return self.Domains.SelectMany(d => GetAllCommands((DomainMetadata) d));
        }
        public static IEnumerable<DataTypeMetadata> GetAllCommands(this DomainMetadata self)
        {
            return self.Aggregates.SelectMany(d => d.Commands);
        }
    }
}