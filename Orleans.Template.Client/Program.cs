using Handler.Person;
using Microsoft.Extensions.Configuration;
using Orleans.Configuration;
using Orleans.Runtime;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Orleans.Hosting;

namespace Orleans.Template.Client
{

    public static class Program
    {
        private const int InitializeAttemptsBeforeFailing = 5;
        private static int attempt = 0;

        //public static void Main(string[] args)
        //{
        //    CreateWebHostBuilder(args).Build().Run();
        //}

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>();

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("Config/appsettings.json", false, true);
            var configuration = builder.Build();

            var client = InitialiseClient(configuration).Result;

            if (client != null)
            {
                var friend = client.GetGrain<IPersonGrain>("");

                for (int i = 0; i < 10; i++)
                {
                    var response = friend.SayHelloAsync();
                    Console.WriteLine("\n{0}", response);
                    Thread.Sleep(500);
                }
            }
            else
            {
                Console.WriteLine("Client init failed.");
            }

            Console.ReadLine();
        }

        private static async Task<IClusterClient> InitialiseClient(IConfigurationRoot configuration)
        {
            var client = new ClientBuilder()
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IPersonGrain).Assembly))
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = configuration.GetSection("ClusterId").Value;
                    options.ServiceId = configuration.GetSection("ServiceId").Value;
                })
                //.UseAdoNetClustering(options =>
                //{
                //    options.Invariant = configuration.GetSection("Invariant").Value;
                //    options.ConnectionString = configuration.GetSection("ConnectionString").Value;
                //})
                .Build();

            await client.Connect(RetryFilter);

            return client;
        }


        private static async Task<bool> RetryFilter(Exception exception)
        {
            if (exception.GetType() != typeof(SiloUnavailableException))
            {
                Console.WriteLine($"Cluster client failed to connect to cluster with unexpected error.  Exception: {exception}");
                return false;
            }

            attempt++;
            Console.WriteLine($"Cluster client attempt {attempt} of {InitializeAttemptsBeforeFailing} failed to connect to cluster.  Exception: {exception}");
            if (attempt > InitializeAttemptsBeforeFailing)
            {
                return false;
            }
            await Task.Delay(TimeSpan.FromSeconds(3));
            return true;
        }
    }
}
