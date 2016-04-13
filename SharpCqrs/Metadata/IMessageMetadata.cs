using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SharpCqrs.Metadata
{
    public interface ISolutionMetadata
    {
        IReadOnlyCollection<IDomainMetadata> Domains { get; }

        IEnumerable<IDataTypeMetadata> GetAllCommands();
    }

    public interface IDomainMetadata
    {
        IReadOnlyCollection<IAggregateMetadata> Aggregates { get; }

        IEnumerable<IDataTypeMetadata> GetAllCommands();
    }

    public interface IAggregateMetadata
    {
        IReadOnlyCollection<IDataTypeMetadata> Commands { get; }
        IReadOnlyCollection<IDataTypeMetadata> Events { get; }
        IReadOnlyCollection<IDataTypeMetadata> States { get; }
    }

    public interface IDataTypeMetadata
    {
        Type ClrType { get; }

        string DomainModel { get; }

        string Version { get; }

        bool IsObsolete { get; }
    }

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
