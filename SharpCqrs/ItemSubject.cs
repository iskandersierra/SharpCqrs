using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace SharpCqrs
{
    public class ItemSubject<TItem> :
        IItemProvider<TItem>,
        IItemAccepter<TItem>
    {
        private readonly Subject<TItem> subject;

        public ItemSubject()
        {
            subject = new Subject<TItem>();
        }

        public IObservable<TItem> Item => subject.Select(e => e);

        public void Accept(TItem item)
        {
            subject.OnNext(item);
        }
    }
}