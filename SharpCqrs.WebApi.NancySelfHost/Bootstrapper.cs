using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using SharpCqrs.Metadata;
using SharpCqrs.Nancy;
using SharpCqrs.Samples.Accounting.Nancy;
using SimpleLogging.Core;
using SimpleLogging.NLog;

namespace SharpCqrs.WebApi.NancySelfHost
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        public ILoggingService Log { get; }

        public Bootstrapper()
        {
            Log = new NLogLoggingService();
        }

        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            container.Update(builder =>
            {
                builder.RegisterInstance(Log).As<ILoggingService>();
                builder.RegisterModule<AccountingAutofacModule>();
            });

            base.ConfigureApplicationContainer(container);
        }

        protected override IEnumerable<Type> ModelBinders
        {
            get
            {
                yield return typeof (JsonSharpCqrsCommandModelBinder);
                foreach (var modelBinder in base.ModelBinders)
                    yield return modelBinder;
            }
        }
    }
}
