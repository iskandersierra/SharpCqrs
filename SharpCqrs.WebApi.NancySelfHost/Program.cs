using System;
using Nancy.Hosting.Self;

namespace SharpCqrs.WebApi.NancySelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var uri = "http://localhost:1239";
            HostConfiguration config = new HostConfiguration
            {
                UrlReservations = new UrlReservations { CreateAutomatically = true, },
            };
            var bootstrapper = new Bootstrapper();
            using (var host = new NancyHost(bootstrapper, config, new Uri(uri)))
            {
                host.Start();
                bootstrapper.Log.Info($"Running on {uri}");
                Console.ReadLine();
            }
        }
    }
}
