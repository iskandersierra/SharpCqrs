using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using Nancy;
using Nancy.ModelBinding;
using SharpCqrs.Infrastructure;
using SharpCqrs.Metadata;

namespace SharpCqrs.Nancy
{
    public class SharpCqrsCommandModelBinder : IModelBinder
    {
        private readonly IPortableModelBinder[] _portableModelBinders;
        private readonly IObservable<ICachedCommandMetadata> _metadataSource;
        private readonly ReaderWriterLockSlim rwLock;
        private ICachedCommandMetadata currentMetadata;
        private static readonly Type[] DefaultModelTypes;


        public SharpCqrsCommandModelBinder(IEnumerable<IPortableModelBinder> portableModelBinders, IObservable<ICachedCommandMetadata> metadataSource)
        {
            if (portableModelBinders == null) throw new ArgumentNullException(nameof(portableModelBinders));
            if (metadataSource == null) throw new ArgumentNullException(nameof(metadataSource));

            rwLock = new ReaderWriterLockSlim();
            _portableModelBinders = portableModelBinders.ToArray();
            _metadataSource = metadataSource;
            _metadataSource.Subscribe(OnNextMetadata);
        }

        static SharpCqrsCommandModelBinder()
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
            if (context.Request.Headers.ContentLength == 0L) return null;

            // Extract information from the request
            var info = ExtractCommandInfo(context);
            if (info == null) return null;

            // Find a portable binder matching detected body format
            var portableBinder = _portableModelBinders.FirstOrDefault(b => b.Matches(info.MediaType, info.Schema, info.SchemaVersion));
            if (portableBinder == null) return null;

            ProvideMetadataResult result;
            using (EnterReadLock())
            {
                if (currentMetadata == null) return null;

                result = currentMetadata.ProvideCommandMetadata(info.Version);
            }


            if (result?.DataType == null) return null;

            if (result.DataType.Version.IsObsolete ?? false) return null;

            var type = result.DataType.ClrType;

            var ctx = new PortableBindingContext
            {
                Type = type,
                MediaType = info.MediaType,
                Schema = info.Schema,
                SchemaVersion = info.SchemaVersion,
                Encoding = info.Encoding,
                Content = context.Request.Body,
                Instance = instance,
                IgnoreErrors = configuration.IgnoreErrors,
                Overwrite = configuration.Overwrite,
                BlackList = blackList,
            };

            return portableBinder.Bind(ctx);
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

        private CommandInfo ExtractCommandInfo(NancyContext context)
        {
            var contentType = context.Request.Headers.ContentType;
            if (contentType == null) return null;
            MediaTypeHeaderValue header;
            if (!MediaTypeHeaderValue.TryParse(contentType, out header)) return null;
            var encoding = default(Encoding);
            if (header.CharSet != null)
                encoding = Encoding.GetEncoding(header.CharSet);
            var domainModel = header.Parameters.FirstOrDefault(p => p.Name == "domain-model")?.Value;
            var ver = header.Parameters.FirstOrDefault(p => p.Name == "domain-version")?.Value;
            if (domainModel == null || ver == null)
                return null;
            DomainVersion domver;
            if (!DomainVersion.TryParse(ver, out domver)) return null;
            var version = new MetadataVersion(domainModel, domver);

            return new CommandInfo(version, encoding, 
                header.MediaType, 
                header.Parameters.FirstOrDefault(p => p.Name == "schema")?.Value, 
                header.Parameters.FirstOrDefault(p => p.Name == "schema-version")?.Value);
        }

        private DisposableAction EnterReadLock()
        {
            return new DisposableAction(() => rwLock.EnterReadLock(), () => rwLock.ExitReadLock());
        }

        private DisposableAction EnterWriteLock()
        {
            return new DisposableAction(() => rwLock.EnterWriteLock(), () => rwLock.ExitWriteLock());
        }

        private class CommandInfo
        {
            public CommandInfo(MetadataVersion version, Encoding encoding, string mediaType, string schema, string schemaVersion)
            {
                if (version == null) throw new ArgumentNullException(nameof(version));
                if (mediaType == null) throw new ArgumentNullException(nameof(mediaType));
                Version = version;
                Encoding = encoding;
                MediaType = mediaType;
                Schema = schema;
                SchemaVersion = schemaVersion;
            }

            public MetadataVersion Version { get; }
            public Encoding Encoding { get; }
            public string MediaType { get; }
            public string Schema { get; }
            public string SchemaVersion { get; }
        }
    }
}
