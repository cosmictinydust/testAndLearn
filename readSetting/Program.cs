using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace readSetting
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureLogging((hostContext, config) =>
                {
                    config.AddConsole();
                    config.SetMinimumLevel(LogLevel.Warning);
                    //config.AddDebug();
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                    config.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                services.AddOptions();
                services.AddLogging();
                    services.AddDbContext<DatabaseContext>(options =>
                    {
                        var connStr = hostContext.Configuration.GetConnectionString("DefaultConnection");
                        options.UseSqlServer(connStr);
                });
                services.Configure<vaulesInSetting>(hostContext.Configuration.GetSection("mySettings"));
                    services.AddHostedService<TimerHostedService>();
                });
            await builder.RunConsoleAsync();
        }
    }
}
