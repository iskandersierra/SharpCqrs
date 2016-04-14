using System;

namespace SharpCqrs
{
    public interface IItemProvider<TItem>
    {
        IObservable<TItem> Item { get; }
    }
}
