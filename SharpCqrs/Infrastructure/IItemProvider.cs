using System;

namespace SharpCqrs.Infrastructure
{
    public interface IItemProvider<out TItem>
    {
        IObservable<TItem> Item { get; }
    }
}
