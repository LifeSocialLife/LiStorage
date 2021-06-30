// <copyright file="CollectionService.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>

namespace LiStorage.Services.Node
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using LiStorage.Models.StoragePool;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Collection handler.
    /// </summary>
    public class CollectionService
    {
#pragma warning disable SA1309 // FieldNamesMustNotBeginWithUnderscore
        private readonly object _lockKey;
        private readonly ILogger<NodeWorker> _logger;
        //private readonly IHostApplicationLifetime _hostApplicationLifetime;
        //private readonly RundataService _rundata;
        //private readonly RundataNodeService _node;
        //private readonly FileOperationService _fileOperation;
        //private readonly StoragePoolService _storagepool;
        //private readonly ObjectStorageService _objectStorage;
        //private readonly NodeHttpService _httpserver;
#pragma warning restore SA1309 // FieldNamesMustNotBeginWithUnderscore

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
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Reviewed.")]
        public CollectionService(ILogger<NodeWorker> logger) // , IHostApplicationLifetime hostappLifetime, RundataService rundataService, FileOperationService fileOperation, RundataNodeService rundataNode, StoragePoolService storagePoolService, ObjectStorageService objectStorageService, NodeHttpService nodeHttpService)
        {
            this.zzDebug = "NodeWorker";

            this._logger = logger;

            //this._hostApplicationLifetime = hostappLifetime;
            //this._rundata = rundataService;
            //this._node = rundataNode;
            //this._fileOperation = fileOperation;
            //this._storagepool = storagePoolService;
            //this._objectStorage = objectStorageService;
            //this._httpserver = nodeHttpService;
            this._lockKey = new object();
            this.Collections = new Dictionary<string, RundataNodeServiceCollectionModel>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        private string zzDebug { get; set; }

        /// <summary>
        /// Do key exist?.
        /// </summary>
        /// <param name="key">collection key id</param>
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
        /// <returns>true</returns>
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

        public Dictionary<string, RundataNodeServiceCollectionModel> GetAll()
        {
            Dictionary<string, RundataNodeServiceCollectionModel> data = new Dictionary<string, RundataNodeServiceCollectionModel>();

            lock (this._lockKey)
            {
                data = (Dictionary<string, RundataNodeServiceCollectionModel>)this.Collections;
            }

            return data;
        }
    }
}
