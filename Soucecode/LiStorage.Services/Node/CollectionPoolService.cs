// <summary>
// Storage pool work.
// </summary>
// <copyright file="CollectionPoolService.cs" company="LiSoLi">
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
    using LiStorage.Models.CollectionPool;
    using LiStorage.Models.StoragePool;
    using LiTools.Helpers.Organize;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Collection handler.
    /// </summary>
    public class CollectionPoolService
    {
        private readonly object _lockKey;
        private readonly ILogger<CollectionPoolService> _logger;
        private readonly RundataService _rundata;
        private readonly RundataNodeService _node;
        private readonly BackgroundWorkService _bgWork;
        
        // private readonly TaskService _task;

        /*
        //private readonly IHostApplicationLifetime _hostApplicationLifetime;

        //private readonly FileOperationService _fileOperation;
        //private readonly StoragePoolService _storagepool;
        //private readonly ObjectStorageService _objectStorage;
        //private readonly NodeHttpService _httpserver;
        */

        private Dictionary<string, CollectionPoolModel> Collections { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionPoolService"/> class.
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
        /// <param name="taskService">BackgroundWorkService.</param>
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Reviewed.")]
        public CollectionPoolService(ILogger<CollectionPoolService> logger,  RundataService rundataService, RundataNodeService rundataNode, BackgroundWorkService taskService) // , TaskService taskService) , StoragePoolService storagePoolService, ObjectStorageService objectStorageService, NodeHttpService nodeHttpService)
        {
            // FileOperationService fileOperation, IHostApplicationLifetime hostappLifetime,
            this.zzDebug = "NodeWorker";

            //this._task = taskService;
            this._bgWork = taskService;
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
            this.Collections = new Dictionary<string, CollectionPoolModel>();
        }

        #region Background task Variables

        /// <summary>
        /// Gets or sets a value indicating whether is init done on this service?.
        /// </summary>
        public bool InitDone { get; set; }

        private string TaskName => "collectionpoolservice";

        #endregion

        /*
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

        

        */

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
        public bool Add(string key, CollectionPoolModel data)
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
        public CollectionPoolModel Get(string collId)
        {
            CollectionPoolModel data;

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
        public Dictionary<string, CollectionPoolModel> GetAll()
        {
            Dictionary<string, CollectionPoolModel> data = new Dictionary<string, CollectionPoolModel>();

            lock (this._lockKey)
            {
                data = (Dictionary<string, CollectionPoolModel>)this.Collections;
            }

            return data;
        }

        #region Background work handler.

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

            // Do bg work exist.
            var tmptaskdata = this._bgWork.GetTaskModel(this.TaskName);

            if (tmptaskdata == null)
            {
                // Task dont exist.  Create new background task work.
                this._bgWork.Start(new BackgroundWorkModel()
                {
                    Name = this.TaskName,
                    Enabled = true,
                    AutoDeleteWhenDone = false,
                    BackgroundTaskRunning = false,
                    BackgroundTaskShodbeRunning = true,
                    TaskAction = this.BackgroundTask,
                    TaskType = TaskRunTypeEnum.Long,
                    WhileInterval = 1000 * 60,          //  1min.
                    WhileIntervalErrorInSek = 60 * 10,      // 10 min.
                    WhileIntervalErrorAction = this.BackgroundTaskError,
                    WhileIntervalNoticInSek = 0,
                    WhileIntervalWarningInSek = 0,
                });

                return;
            }

            if (!tmptaskdata.Enabled)
            {
                // this work is not enables. return check
                return;
            }

            if (!tmptaskdata.BackgroundTaskShodbeRunning)
            {
                // Work it nos set as shod be running. Set shodberunning to True.
                tmptaskdata.BackgroundTaskShodbeRunning = true;
                return;
            }

            this.zzDebug = "Check";
        }

        private void BackgroundTaskError()
        {
            // TODO Fix this.
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }

            this.zzDebug = "sdfdsf";
        }

        private void BackgroundTask()
        {
            this._logger.LogInformation("Collection Service running at: {time}", DateTimeOffset.Now);
        }

        #endregion
    }
}
