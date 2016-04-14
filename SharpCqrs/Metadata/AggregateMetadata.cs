using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SharpCqrs.Metadata
{
    public class AggregateMetadata : 
        MetadataElement
    {
        public AggregateMetadata(
            string name, 
            string description, 
            MetadataVersion version,
            DataTypeMetadata[] commands,
            DataTypeMetadata[] events,
            DataTypeMetadata[] states) 
            : base(name, description, version)
        {
            Commands = new ReadOnlyCollection<DataTypeMetadata>(commands ?? new DataTypeMetadata[0]);
            Events = new ReadOnlyCollection<DataTypeMetadata>(events ?? new DataTypeMetadata[0]);
            States = new ReadOnlyCollection<DataTypeMetadata>(states ?? new DataTypeMetadata[0]);
        }

        public IReadOnlyCollection<DataTypeMetadata> Commands { get; }
        public IReadOnlyCollection<DataTypeMetadata> Events { get; }
        public IReadOnlyCollection<DataTypeMetadata> States { get; }
    }
}