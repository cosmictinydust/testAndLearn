using System;
using System.Linq;
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
        private readonly DatabaseContext _databaseContext;
        private Timer _timer;

        public TimerHostedService(IOptionsSnapshot<vaulesInSetting> settings, ILogger<TimerHostedService> logger,DatabaseContext databaseContext)
        {
            _settings = settings.Value;
            _logger = logger;
            _databaseContext = databaseContext;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogWarning($"From appsettings.json value1:{_settings.Interval}");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_settings.Interval));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var query = _databaseContext.OtherSet.AsQueryable();
            string newIp = "";
            OtherSet currentSet = (OtherSet)query.Where(x => x.itemName == "gzxf").FirstOrDefault();
            DateTime lastUpdate = currentSet.updateTime;
            TimeSpan minDiff = DateTime.Now - lastUpdate;
            if ((int)minDiff.TotalMinutes < 10)
                newIp = currentSet.cItemSet;
            _logger.LogInformation($"Program is running in {DateTime.Now}, the newIP:{newIp}");
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