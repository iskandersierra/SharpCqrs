using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ModelBinding;
using SharpCqrs.Metadata;

namespace SharpCqrs.Nancy
{
    public class JsonSharpCqrsCommandModelBinder : IModelBinder
    {


        public JsonSharpCqrsCommandModelBinder(ISolutionMetadata metadata)
        {
            CacheCommands(metadata.GetAllCommands());
        }

        public object Bind(NancyContext context, Type modelType, object instance, BindingConfig configuration,
            params string[] blackList)
        {
            throw new NotImplementedException();
        }

        public bool CanBind(Type modelType)
        {
            throw new NotImplementedException();
        }

        private void CacheCommands(IEnumerable<IDataTypeMetadata> getAllCommands)
        {
            throw new NotImplementedException();
        }
    }
}
