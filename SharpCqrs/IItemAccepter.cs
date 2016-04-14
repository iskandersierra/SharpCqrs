namespace SharpCqrs
{
    public interface IItemAccepter<TItem>
    {
        void Accept(TItem item);
    }
}