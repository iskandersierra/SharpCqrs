namespace SharpCqrs.Infrastructure
{
    public interface IPortableModelBinder
    {
        bool Matches(string mediaType, string schema, string schemaVersion);
        object Bind(PortableBindingContext context);
    }
}
