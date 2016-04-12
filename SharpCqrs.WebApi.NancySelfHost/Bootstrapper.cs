using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using SharpCqrs.Samples.Accounting.Nancy;
using SimpleLogging.Core;
using SimpleLogging.NLog;

namespace SharpCqrs.WebApi.NancySelfHost
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            container.Update(builder =>
            {
                builder.RegisterType<NLogLoggingService>().As<ILoggingService>().SingleInstance();
                builder.RegisterModule<AccountingAutofacModule>();
            });

            base.ConfigureApplicationContainer(container);
        }
    }
}
