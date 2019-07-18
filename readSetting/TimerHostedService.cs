using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            var query = _databaseContext.OtherSet.AsQueryable().AsNoTracking();
            string newIp = "";
            OtherSet currentSet = (OtherSet)query.Where(x => x.itemName == "gzxf").FirstOrDefault();
            DateTime lastUpdate = currentSet.updateTime;
            TimeSpan minDiff = DateTime.Now - lastUpdate;
            if ((int)minDiff.TotalMinutes < 10)
                newIp = currentSet.cItemSet;
            _logger.LogWarning($"数据库最后更新时间:{lastUpdate}, the newIP:{newIp}");
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Timer is stopping!");
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