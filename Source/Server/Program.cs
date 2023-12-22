namespace CrazyAboutPi.Server
{
    using System;
    using Nancy;
    using Nancy.Hosting.Self;

    class Program
    {
        static void Main(string[] args)
        {
            var uri = new Uri("http://localhost:3579");
            var hostConfig = new HostConfiguration();
            hostConfig.UrlReservations.CreateAutomatically = true;

            using (var host = new NancyHost(hostConfig, uri))
            {
                host.Start();
                Console.WriteLine("Your application is running on " + uri);
                Console.WriteLine("Press any [Enter] to close the host.");
                Console.ReadLine();
            }
        }
    }

    public static class NancyExtensions
    {
        public static void EnableCors(this NancyModule module)
        {
            module.After.AddItemToEndOfPipeline(x =>
            {
                x.Response.WithHeader("Access-Control-Allow-Origin", "*");
            });
        }
    }
}