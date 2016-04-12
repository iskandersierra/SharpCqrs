using Autofac;
using Nancy;

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
            base.Load(builder);
        }
    }
}
