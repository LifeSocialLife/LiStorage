using LiLog.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LiLog.Test.Client
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly LiLogService _liLog;

        public Worker(ILogger<Worker> logger, LiLogService liLogService)
        {
            _logger = logger;
            this._liLog = liLogService;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            this._liLog.Init();

            while (!stoppingToken.IsCancellationRequested)
            {
                string test = $"Worker running at: {DateTime.UtcNow}";

                this._liLog.send(test);

                // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                try
                {
                    await Task.Delay(1000, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }
    }
}
