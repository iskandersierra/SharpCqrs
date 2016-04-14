using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using Nancy;
using Nancy.ModelBinding;
using SharpCqrs.Metadata;

namespace SharpCqrs.Nancy
{
    public class JsonSharpCqrsCommandModelBinder : IModelBinder
    {
        private readonly IObservable<ICachedCommandMetadata> _metadataSource;
        private readonly ReaderWriterLockSlim rwLock;
        private ICachedCommandMetadata currentMetadata;
        private static readonly Type[] DefaultModelTypes;


        public JsonSharpCqrsCommandModelBinder(IObservable<ICachedCommandMetadata> metadataSource)
        {
            rwLock = new ReaderWriterLockSlim();
            _metadataSource = metadataSource;
            _metadataSource.Subscribe(OnNextMetadata);
        }

        static JsonSharpCqrsCommandModelBinder()
        {
            DefaultModelTypes = new []
            {
                typeof(object),
                typeof(IDynamicMetaObjectProvider),
                typeof(IDictionary<string, object>),
                typeof(ExpandoObject),
            };
        }

        private void OnNextMetadata(ICachedCommandMetadata cachedCommandMetadata)
        {
            using (EnterWriteLock())
            {
                currentMetadata = cachedCommandMetadata;
            }
        }

        public object Bind(
            NancyContext context, 
            Type modelType, 
            object instance, 
            BindingConfig configuration,
            params string[] blackList)
        {
            using (EnterReadLock())
            {
                var version = ExtractCommandVersion(context);
            }
        }

        public bool CanBind(Type modelType)
        {
            if (DefaultModelTypes.Contains(modelType))
                return true;

            using (EnterReadLock())
            {
                if (currentMetadata != null)
                    return currentMetadata.IsClrTypeValidCommand(modelType);
                return false;
            }
        }

        private DisposableAction EnterReadLock()
        {
            return new DisposableAction(() => rwLock.EnterReadLock(), () => rwLock.ExitReadLock());
        }

        private DisposableAction EnterWriteLock()
        {
            return new DisposableAction(() => rwLock.EnterWriteLock(), () => rwLock.ExitWriteLock());
        }
    }
}
