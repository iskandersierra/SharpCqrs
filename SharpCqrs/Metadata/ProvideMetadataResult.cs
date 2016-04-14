namespace SharpCqrs.Metadata
{
    public class ProvideMetadataResult
    {
        public ProvideMetadataResult(IDataTypeMetadata dataType, IDataTypeMetadata lastVersionDataType)
        {
            DataType = dataType;
            LastVersionDataType = lastVersionDataType;
        }

        public IDataTypeMetadata DataType { get; }

        public IDataTypeMetadata LastVersionDataType { get; }
    }
}