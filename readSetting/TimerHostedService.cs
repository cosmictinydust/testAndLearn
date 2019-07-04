using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace readSetting
{
    class TimerHostedService:BackgroundService
    {
        private readonly vaulesInSetting _settings;
        private ILogger _logger;
        private Timer _timer;

        public TimerHostedService(IOptionsSnapshot<vaulesInSetting> settings, ILogger<TimerHostedService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogWarning($"From appsettings.json value1:{_settings.Value1}");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation($"Program is running . {DateTime.Now},value2:{_settings.Value2}");
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timer is stopping!");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }
    }
}