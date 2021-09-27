// <summary>
// Main handler for all background work, node and master.
// </summary>
// <copyright file="Worker.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>

namespace LiStorage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using LiStorage.Models.StoragePool;
    using LiStorage.Services;
    using LiStorage.Services.Node;
    using LiTools.Helpers.Organize;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Listorage background worker.
    /// </summary>
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly RundataService _rundata;
        private readonly IConfiguration _configuration;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly RundataNodeService _node;
        private readonly FileOperationService _fileOperation;
        private readonly CollectionService _collections;
        private readonly StoragePoolService _storagepool;
        private readonly BlockStorageService _objectStorage;
        private readonly NodeHttpService _httpserver;
        private readonly ConfigFileService _configFile;
        private readonly TaskService _task;

        /// <summary>
        /// Initializes a new instance of the <see cref="Worker"/> class.
        /// </summary>
        /// <param name="logger">ILogger.</param>
        /// <param name="configuration">IConfiguration.</param>
        /// <param name="rundataService">RundataService.</param>
        /// <param name="hostappLifetime">IHostApplicationLifetime.</param>
        /// <param name="fileOperation">FileOperationService.</param>
        /// <param name="rundataNode">RundataNodeService.</param>
        /// <param name="storagePoolService">StoragePoolService.</param>
        /// <param name="fileStorageService">FileStorageService.</param>
        /// <param name="objectStorageService">ObjectStorageService.</param>
        /// <param name="nodeHttpService">NodeHttpService.</param>
        /// <param name="collectionService">CollectionService.</param>
        /// <param name="configFileService">ConfigFileService.</param>
        /// <param name="taskService">TaskService.</param>
        public Worker(ILogger<Worker> logger, IConfiguration configuration, RundataService rundataService, IHostApplicationLifetime hostappLifetime, FileOperationService fileOperation, RundataNodeService rundataNode, StoragePoolService storagePoolService, BlockStorageService objectStorageService, NodeHttpService nodeHttpService, CollectionService collectionService, ConfigFileService configFileService, TaskService taskService)
        {
            this.zzDebug = "Worker";

            this._logger = logger;
            this._task = taskService;
            this._configuration = configuration;
            this._rundata = rundataService;
            this._hostApplicationLifetime = hostappLifetime;
            this._node = rundataNode;
            this._fileOperation = fileOperation;
            this._storagepool = storagePoolService;
            this._objectStorage = objectStorageService;
            this._httpserver = nodeHttpService;
            this._collections = collectionService;
            this._configFile = configFileService;
        }

        [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Reviewed.")]
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        private string zzDebug { get; set; }

        /// <inheritdoc/>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            this._logger.LogInformation($"Background service stopping : {cancellationToken.IsCancellationRequested}");

            this.zzDebug = "sdfdf";

            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }

            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            #region Sett paths and other data

            this._hostApplicationLifetime.ApplicationStarted.Register(this.OnStarted, true);
            this._hostApplicationLifetime.ApplicationStopping.Register(this.OnStopping, true);
            this._hostApplicationLifetime.ApplicationStopped.Register(this.OnStopped, true);

            var dd = WebHostDefaults.ContentRootKey;
            var d2 = this._configuration.GetValue<string>(WebHostDefaults.ContentRootKey);

            this._rundata.Folders.PathRuntimes = this._configuration.GetValue<string>(WebHostDefaults.ContentRootKey);

            #endregion

            this.zzDebug = "dfdsf";

            #region DEV - Create configfile for dev node

            /*
            var gg = LiStorage.Helpers.Configuration.ConfigFileNodeHelper.CreateTemplate();
            var a1 = Helpers.CommonHelper.SerializeJson(gg, true);
            this.zzDebug = "sdfdf";
            this._fileOperation.WriteFile(this._rundata.Folders.ConfigFile, a1, false);
            */

            #endregion

            this.zzDebug = "sdfdsf";

            // Set background status to shod be running.
            this._storagepool.BackgroundTaskShodbeRunning = true;

            while (!stoppingToken.IsCancellationRequested)
            {
                #region Locatea and read configfile

                if (this._node.StartUpStatus.ConfigFile == Models.Rundata.NodeStartUpStatusEnum.Notstarted)
                {
                    // Locate configfile.
                    this._rundata.Folders.ConfigFile = this._fileOperation.LocateFileInKnownLocations("LiStorageNode.conf");

                    // Configfile exist?
                    if (string.IsNullOrEmpty(this._rundata.Folders.ConfigFile))
                    {
                        this._node.StartUpStatus.ConfigFileExist = false;
                        this._node.StartUpStatus.ConfigFile = Models.Rundata.NodeStartUpStatusEnum.Error;
                    }
                    else
                    {
                        this._node.StartUpStatus.ConfigFile = Models.Rundata.NodeStartUpStatusEnum.Running;
                        this._node.StartUpStatus.ConfigFileExist = true;

                        // TODO . Turn this into task.
                        this._configFile.ReadNodeConfigFile();
                        this.zzDebug = "sfdf";
                    }
                }

                #endregion

                #region If configfile is reading. Stop and restart while.

                if (this._node.StartUpStatus.ConfigFile == Models.Rundata.NodeStartUpStatusEnum.Running)
                {
                    await Task.Delay(3000, stoppingToken);
                    continue;
                }

                #endregion

                this.zzDebug = "sdfdsf";

                this._task.BackgroundTaskChecker();
                this._configFile.BackgroundTaskChecker();
                this._storagepool.BackgroundTaskChecker();

                this._logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                try
                {
                    await Task.Delay(1000, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }

            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
            
            LiTools.Helpers.Organize.ParallelTask.Token.Cancel();

        }

        private void OnStarted()
        {
            // Perform post-startup activities here
            this._logger.LogInformation("OnStarted has been called.");

            this.zzDebug = "sdfdf";

            //if (Debugger.IsAttached)
            //{
            //    Debugger.Break();
            //}
        }

        private void OnStopping()
        {
            // Perform on-stopping activities here
            this._logger.LogInformation("NodeWorker | OnStopping | Stop all backgrounds work.");
            this._logger.LogInformation("NodeWorker | OnStopping | Stop all backgrounds work. | Done");

            // _logger.LogInformation("OnStopping has been called.");
            this.zzDebug = "sdfdf";

            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }

        private void OnStopped()
        {
            // Perform post-stopped activities here
            this._logger.LogInformation("OnStopped has been called.");

            this.zzDebug = "sdfdf";
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }
    }
}
