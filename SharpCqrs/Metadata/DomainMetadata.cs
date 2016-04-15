using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SharpCqrs.Metadata
{
    public class DomainMetadata : 
        MetadataElement
    {
        public DomainMetadata(
            string name, 
            string description, 
            MetadataVersion version, 
            AggregateMetadata[] aggregates) 
            : base(name, description, version)
        {
            Aggregates = new ReadOnlyCollection<AggregateMetadata>(aggregates ?? new AggregateMetadata[0]);
            foreach (var aggregate in Aggregates)
                aggregate.Parent = this;
        }

        public IReadOnlyCollection<AggregateMetadata> Aggregates { get; }

        public SolutionMetadata Solution => Parent as SolutionMetadata;
    }
}