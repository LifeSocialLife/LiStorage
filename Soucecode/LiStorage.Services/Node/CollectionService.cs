// <summary>
// Storage pool work.
// </summary>
// <copyright file="CollectionService.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Services.Node
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using LiStorage.Models.StoragePool;
    using LiTools.Helpers.Organize;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Collection handler.
    /// </summary>
    public class CollectionService
    {
        private readonly object _lockKey;
        private readonly ILogger<CollectionService> _logger;
        private readonly RundataService _rundata;
        private readonly RundataNodeService _node;
        private readonly TaskService _task;

        /*
        //private readonly IHostApplicationLifetime _hostApplicationLifetime;

        //private readonly FileOperationService _fileOperation;
        //private readonly StoragePoolService _storagepool;
        //private readonly ObjectStorageService _objectStorage;
        //private readonly NodeHttpService _httpserver;
        */

        private Dictionary<string, RundataNodeServiceCollectionModel> Collections { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionService"/> class.
        /// </summary>
        /// <param name="logger">ILogger.</param>
        /// <param name="hostappLifetime">IHostApplicationLifetime.</param>
        /// <param name="rundataService">RundataService.</param>
        /// <param name="fileOperation">FileOperationService.</param>
        /// <param name="rundataNode">RundataNodeService.</param>
        /// <param name="storagePoolService">StoragePoolService.</param>
        /// <param name="fileStorageService">FileStorageService.</param>
        /// <param name="objectStorageService">ObjectStorageService.</param>
        /// <param name="nodeHttpService">NodeHttpService.</param>
        /// <param name="taskService">TaskService.</param>
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Reviewed.")]
        public CollectionService(ILogger<CollectionService> logger,  RundataService rundataService, RundataNodeService rundataNode, TaskService taskService) // , StoragePoolService storagePoolService, ObjectStorageService objectStorageService, NodeHttpService nodeHttpService)
        {
            // FileOperationService fileOperation, IHostApplicationLifetime hostappLifetime,
            this.zzDebug = "NodeWorker";

            this._task = taskService;
            this._logger = logger;
            this._rundata = rundataService;
            this._node = rundataNode;

            /*
            //this._hostApplicationLifetime = hostappLifetime;
            //this._fileOperation = fileOperation;
            //this._storagepool = storagePoolService;
            //this._objectStorage = objectStorageService;
            //this._httpserver = nodeHttpService;
            */

            this._lockKey = new object();
            this.Collections = new Dictionary<string, RundataNodeServiceCollectionModel>();
        }

        #region Background task Variables

        /// <summary>
        /// Gets or sets a value indicating whether is init done on this service?.
        /// </summary>
        public bool InitDone { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is background task runinng?.
        /// </summary>
        public bool BackgroundTaskRunning { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether shod background task be running?.
        /// </summary>
        public bool BackgroundTaskShodbeRunning { get; set; }

        /// <summary>
        /// Gets or sets when was the background task last run?.
        /// </summary>
        public DateTime BackgroundTaskLastRun { get; set; }

        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        private string zzDebug { get; set; }

        /// <summary>
        /// Do key exist?.
        /// </summary>
        /// <param name="key">collection key id.</param>
        /// <returns>true or false. do it exist.</returns>
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:BracesMustNotBeOmitted", Justification = "Reviewed.")]
        public bool ContainsKey(string key)
        {
            bool tmpReturn = false;

            lock (this._lockKey)
            {
                if (this.Collections.ContainsKey(key))
                    tmpReturn = true;
            }

            return tmpReturn;
        }

        /// <summary>
        /// Add item to collection dictionary.
        /// </summary>
        /// <param name="key">collection id.</param>
        /// <param name="data">collection data.</param>
        /// <returns>true.</returns>
        public bool Add(string key, RundataNodeServiceCollectionModel data)
        {
            lock (this._lockKey)
            {
                this.Collections.Add(key, data);
            }

            return true;
        }

        /// <summary>
        /// Get collection model.
        /// </summary>
        /// <param name="collId">Key of collection.</param>
        /// <returns>RundataNodeServiceCollectionModel.</returns>
        public RundataNodeServiceCollectionModel Get(string collId)
        {
            RundataNodeServiceCollectionModel data;

            lock (this._lockKey)
            {
                data = this.Collections[collId];
            }

            return data;
        }

        /// <summary>
        /// Get all collections and return as new dictonary.
        /// </summary>
        /// <returns>Dictionary whit all collections.</returns>
        public Dictionary<string, RundataNodeServiceCollectionModel> GetAll()
        {
            Dictionary<string, RundataNodeServiceCollectionModel> data = new Dictionary<string, RundataNodeServiceCollectionModel>();

            lock (this._lockKey)
            {
                data = (Dictionary<string, RundataNodeServiceCollectionModel>)this.Collections;
            }

            return data;
        }

        /// <summary>
        /// Run check on storagepools.
        /// </summary>
        public void BackgroundTaskChecker()
        {
            // Init it not done. return
            if (!this.InitDone)
            {
                return;
            }

            bool startBackgroundTask = false;

            if ((!this.BackgroundTaskRunning) && this.BackgroundTaskShodbeRunning)
            {
                // Background work is not running.. Start backgroundwork.
                startBackgroundTask = true;
            }
            else if ((DateTime.UtcNow - this.BackgroundTaskLastRun).TotalMinutes > 5)
            {
                // Background shod be running but have not reported anything for more then 5 min.
                // TODO Fix this.
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            }

            if (startBackgroundTask)
            {
                // Check if task already exist?
                if (this._task.TaskExists("collectionservice"))
                {
                    this._task.Check("collectionservice");
                    System.Threading.Thread.Sleep(500);
                }

                // if (LiTools.Helpers.Organize.ParallelTask.Exist("storagepoolservice"))
                if (this._task.TaskExists("collectionservice"))
                {
                    // Task already exist. what to do??
                    // TODO Fix this.
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }
                }
                else
                {
                    // Start task.
                    this.zzDebug = "dsf";
                    this._task.StartNew(this.BackgroundTask, TaskRunTypeEnum.Long, "collectionservice", true);

                    // LiTools.Helpers.Organize.ParallelTask.StartLongRunning(this.BackgroundTask, LiTools.Helpers.Organize.ParallelTask.Token.Token, "storagepoolservice");
                }
            }

            this.zzDebug = "Check";
        }

        private async void BackgroundTask()
        {
            while (!LiTools.Helpers.Organize.ParallelTask.Token.IsCancellationRequested)
            {
                this.BackgroundTaskRunning = true;
                this.BackgroundTaskLastRun = DateTime.UtcNow;

                if (!this.BackgroundTaskShodbeRunning)
                {
                    break;
                }

                this._logger.LogInformation("Collection Service running at: {time}", DateTimeOffset.Now);
                try
                {
                    await Task.Delay(1000, LiTools.Helpers.Organize.ParallelTask.Token.Token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }

            this.BackgroundTaskRunning = false;
        }
    }
}
