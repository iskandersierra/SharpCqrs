using Autofac;
using Nancy;
using SharpCqrs.Metadata;

namespace SharpCqrs.Samples.Accounting.Nancy
{
    public class AccountingAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AccountingNancyModule>()
                .As<INancyModule>()
                .PropertiesAutowired(PropertyWiringOptions.PreserveSetValues)
                .SingleInstance();

            var solutionMetadata = new SolutionMetadata();
            builder.RegisterInstance(solutionMetadata).AsImplementedInterfaces();

            base.Load(builder);
        }
    }
}
