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
            using (var host = new NancyHost(new Bootstrapper(), config, new Uri(uri)))
            {
                host.Start();
                Console.WriteLine($"Running on {uri}");
                Console.ReadLine();
            }
        }
    }
}
