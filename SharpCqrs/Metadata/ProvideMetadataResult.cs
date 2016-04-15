namespace SharpCqrs.Metadata
{
    public class ProvideMetadataResult
    {
        public ProvideMetadataResult(DataTypeMetadata dataType, DataTypeMetadata lastVersionDataType)
        {
            DataType = dataType;
            LastVersionDataType = lastVersionDataType;
        }

        public DataTypeMetadata DataType { get; }

        public DataTypeMetadata LastVersionDataType { get; }
    }
}