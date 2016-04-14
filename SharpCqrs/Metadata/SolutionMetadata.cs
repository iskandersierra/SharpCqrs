﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SharpCqrs.Metadata
{
    public class SolutionMetadata : 
        MetadataElement
    {
        public SolutionMetadata(
            string name, 
            string description, 
            MetadataVersion version, 
            DomainMetadata[] domains) 
            : base(name, description, version)
        {
            Domains = new ReadOnlyCollection<DomainMetadata>(domains ?? new DomainMetadata[0]);
        }

        public IReadOnlyCollection<DomainMetadata> Domains { get; }
    }
}
