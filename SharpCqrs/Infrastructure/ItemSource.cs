using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace SharpCqrs.Infrastructure
{
    public class ItemSource<TItem> :
        IItemProvider<TItem>
    {
        private readonly Subject<TItem> subject;

        public ItemSource(IObservable<TItem> source, CancellationToken ct = default(CancellationToken))
        {
            source.Subscribe(
                item => subject.OnNext(item),
                ex => subject.OnError(ex),
                () => subject.OnCompleted(),
                ct);
            subject = new Subject<TItem>();
        }

        public IObservable<TItem> Item => subject.Select(e => e);
    }
}