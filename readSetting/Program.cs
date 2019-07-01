using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace readSetting
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder=new HostBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.Configure<vaulesInSetting>(hostContext.Configuration.GetSection("mySettings"));
                })
                .Build();
            using (builder)
            {
                await builder.StartAsync();
                var test = builder.Services.GetService<IOptions<vaulesInSetting>>();
                Console.WriteLine($"Get values .Value1:{test.Value.Value1}");
                Console.ReadKey();
            }
        }
    }
}
