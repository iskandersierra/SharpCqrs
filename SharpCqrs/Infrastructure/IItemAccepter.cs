namespace SharpCqrs.Infrastructure
{
    public interface IItemAccepter<in TItem>
    {
        void Accept(TItem item);
    }
}