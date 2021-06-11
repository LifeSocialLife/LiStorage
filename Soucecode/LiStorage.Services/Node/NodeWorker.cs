﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;



namespace LiStorage.Services.Node
{
    public class NodeWorker : BackgroundService
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private string zzDebug { get; set; }

        private readonly ILogger<NodeWorker> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        private readonly RundataService _rundata;
        private readonly FileOperationService _fileOperation;

        public NodeWorker(ILogger<NodeWorker> logger, IHostApplicationLifetime hostappLifetime, RundataService rundataService, FileOperationService fileOperation)
        {
            this.zzDebug = "NodeWorker";

            _logger = logger;


            this._hostApplicationLifetime = hostappLifetime;
            this._rundata = rundataService;
            this._fileOperation = fileOperation;


        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _hostApplicationLifetime.ApplicationStarted.Register(OnStarted, true);
            _hostApplicationLifetime.ApplicationStopping.Register(OnStopping, true);
            _hostApplicationLifetime.ApplicationStopped.Register(OnStopped, true);


            // Get configuration file.
            this._rundata.Folders.ConfigFile = this._fileOperation.LocateFileInKnownLocations("LiStorageNode.conf");
            if (string.IsNullOrEmpty(this._rundata.Folders.ConfigFile))
            {
                if (System.Diagnostics.Debugger.IsAttached)
                    System.Diagnostics.Debugger.Break();
            }

            var ddd = this._rundata;
           


            while (!stoppingToken.IsCancellationRequested)
            {
               

                //_logger.LogInformation($"Background service running :: {stoppingToken.IsCancellationRequested}");
                await Task.Delay(2000, stoppingToken);
            }




            this.zzDebug = "sdfdsf";


        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Background service stopping : {cancellationToken.IsCancellationRequested}");


            this.zzDebug = "sdfdf";

            await Task.CompletedTask;
        }

        private void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");

            this.zzDebug = "sdfdf";
            // Perform post-startup activities here
        }

        private void OnStopping()
        {
            _logger.LogInformation("NodeWorker | OnStopping | Stop all backgrounds work.");

         

            _logger.LogInformation("NodeWorker | OnStopping | Stop all backgrounds work. | Done");
            // _logger.LogInformation("OnStopping has been called.");

            this.zzDebug = "sdfdf";
            // Perform on-stopping activities here
        }

        private void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");

            this.zzDebug = "sdfdf";
            // Perform post-stopped activities here
        }
    }
}
