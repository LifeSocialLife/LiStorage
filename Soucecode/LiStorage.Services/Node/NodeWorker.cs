// <summary>
// {one line to give the library's name and an idea of what it does.}
// </summary>
// <copyright file="NodeWorker.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Services.Node
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using LiStorage.Models.StoragePool;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Nodeworker that is running all backend work for the storage node.
    /// </summary>
    public class NodeWorker : BackgroundService
    {
#pragma warning disable SA1309 // FieldNamesMustNotBeginWithUnderscore

        // [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed.")]
        private readonly ILogger<NodeWorker> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly RundataService _rundata;
        private readonly RundataNodeService _node;
        private readonly FileOperationService _fileOperation;
        private readonly CollectionService _collections;
        private readonly StoragePoolService _storagepool;
        private readonly BlockStorageService _objectStorage;
        private readonly NodeHttpService _httpserver;
#pragma warning restore SA1309 // FieldNamesMustNotBeginWithUnderscore

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeWorker"/> class.
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
        /// <param name="collectionService">CollectionService.</param>
        public NodeWorker(ILogger<NodeWorker> logger, IHostApplicationLifetime hostappLifetime, RundataService rundataService, FileOperationService fileOperation, RundataNodeService rundataNode, StoragePoolService storagePoolService, BlockStorageService objectStorageService, NodeHttpService nodeHttpService, CollectionService collectionService)
        {
            this.zzDebug = "NodeWorker";

            this._logger = logger;

            this._hostApplicationLifetime = hostappLifetime;
            this._rundata = rundataService;
            this._node = rundataNode;
            this._fileOperation = fileOperation;
            this._storagepool = storagePoolService;
            this._objectStorage = objectStorageService;
            this._httpserver = nodeHttpService;
            this._collections = collectionService;
        }

        // [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]

        private string zzDebug { get; set; }

        /// <inheritdoc/>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            this._logger.LogInformation($"Background service stopping : {cancellationToken.IsCancellationRequested}");

            this.zzDebug = "sdfdf";

            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1123:DoNotPlaceRegionsWithinElements", Justification = "Reviewed.")]
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this._hostApplicationLifetime.ApplicationStarted.Register(this.OnStarted, true);
            this._hostApplicationLifetime.ApplicationStopping.Register(this.OnStopping, true);
            this._hostApplicationLifetime.ApplicationStopped.Register(this.OnStopped, true);

            this._rundata.Folders.ConfigFile = this._fileOperation.LocateFileInKnownLocations("LiStorageNode.conf");
            this._node.StartUpStatus.ConfigFileExist = Models.Rundata.NodeStartUpStatusEnum.Running;

            // Do we have a configfile for this node.
            if (string.IsNullOrEmpty(this._rundata.Folders.ConfigFile))
            {
                this._node.StartUpStatus.ConfigFileExist = Models.Rundata.NodeStartUpStatusEnum.Error;
            }
            else
            {
                this._node.StartUpStatus.ConfigFileExist = Models.Rundata.NodeStartUpStatusEnum.Done;
            }

            #region DEV - Create configfile for dev node

            // var gg = LiStorage.Helpers.Configuration.ConfigFileNodeHelper.CreateTemplate();
            // var a1 = Helpers.CommonHelper.SerializeJson(gg, true);
            // this.zzDebug = "sdfdf";
            // this._fileOperation.WriteFile(this._rundata.Folders.ConfigFile, a1, false);
            #endregion

            this.zzDebug = "sdfdf";

            // Get configuration file. if the configfile exist in system
            if (this._node.StartUpStatus.ConfigFileExist == Models.Rundata.NodeStartUpStatusEnum.Done)
            {
                this.ReadConfigFile();
            }
            else
            {
                this._node.StartUpStatus.ConfigFileExist = Models.Rundata.NodeStartUpStatusEnum.Error;
            }

            // var ddd = this._rundata;
            this.zzDebug = "sdfdf";

            while (!stoppingToken.IsCancellationRequested)
            {
                // Check storage pools.
                this._storagepool.CheckStoragePools();


                //this._fileStorage.CheckCollections();

                // Start watson webserver
                // var dd = this._node.Collections;
                this.zzDebug = "Sfsdf";

                if (!this._httpserver.IsRunning)
                {
                    this._httpserver.StartWebserver();
                }

                // _logger.LogInformation($"Background service running :: {stoppingToken.IsCancellationRequested}");
                await Task.Delay(2000, stoppingToken);
            }

            this.zzDebug = "sdfdsf";
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1123:DoNotPlaceRegionsWithinElements", Justification = "Reviewed.")]
        private void ReadConfigFile()
        {
            // Read data from configfile
            this._node.StartUpStatus.ConfigFileRead = Models.Rundata.NodeStartUpStatusEnum.Running;
            var tmpConfigFileAsString = this._fileOperation.ReadTextFile(this._rundata.Folders.ConfigFile);

            if (string.IsNullOrEmpty(tmpConfigFileAsString))
            {
                this._node.StartUpStatus.ConfigFileRead = Models.Rundata.NodeStartUpStatusEnum.Error;
                return;
            }

            this._node.StartUpStatus.ConfigFileRead = Models.Rundata.NodeStartUpStatusEnum.Done;

            #region Get Version of configfile and convert to model

            this._node.StartUpStatus.ConfigFileWorking = Models.Rundata.NodeStartUpStatusEnum.Running;

            if (tmpConfigFileAsString.Contains("Version"))
            {
                int hej = tmpConfigFileAsString.IndexOf("Version");

                // string tmpString = tmpConfigFileAsString.Substring(hej);
                string tmpString = tmpConfigFileAsString[hej..];

                if (tmpString.Contains(":") && tmpString.Contains(","))
                {
                    int tmpIdFirst = tmpString.IndexOf(":");
                    int tmpIdLast = tmpString.IndexOf(",");
                    string tmpVersionData = tmpString.Substring(tmpIdFirst + 1, tmpIdLast - tmpIdFirst - 1).Trim();

                    // Convert version string into uint16
                    try
                    {
                        this._node.ConfigFileData.Version = ushort.Parse(tmpVersionData);

                        // Console.WriteLine(result);
                    }
                    catch (FormatException)
                    {
                        this._node.StartUpStatus.ConfigFileWorking = Models.Rundata.NodeStartUpStatusEnum.Error;

                        // Console.WriteLine($"Unable to parse '{input}'");
                    }

                    if (this._node.ConfigFileData.Version == 0)
                    {
                        this._node.StartUpStatus.ConfigFileWorking = Models.Rundata.NodeStartUpStatusEnum.Error;
                    }

                    this.zzDebug = "dsfdsf";
                }
                else
                {
                    this._node.StartUpStatus.ConfigFileWorking = Models.Rundata.NodeStartUpStatusEnum.Error;
                }

                this.zzDebug = "sfdsf";
            }
            else
            {
                this._node.StartUpStatus.ConfigFileWorking = Models.Rundata.NodeStartUpStatusEnum.Error;
            }

            this.zzDebug = "fdsf";

            if (this._node.StartUpStatus.ConfigFileWorking == Models.Rundata.NodeStartUpStatusEnum.Error)
            {
                return;
            }

            this.zzDebug = "sdfdf";

            // Convert json string into model
            var tmpModel = LiStorage.Helpers.CommonHelper.DeserializeJson<Models.Rundata.NodeConfigFileModel>(tmpConfigFileAsString);

            #endregion

            #region Importdata into Node rundata service.

            if (tmpModel.Version == 1)
            {
                // This is a version 1 import.
                this._node.ConfigFileData.NodeName = tmpModel.NodeName;
                this._node.ConfigFileData.ClusterKey = tmpModel.ClusterKey;
                this._node.ConfigFileData.HeaderApiKey = tmpModel.HeaderApiKey;

                #region Import masters

                if ((tmpModel.Masters != null) && (tmpModel.Masters.Count > 0))
                {
                    foreach (var master in tmpModel.Masters)
                    {
                        if (master.Name == "demo")
                        {
                            continue;
                        }

                        if (!this._node.Masters.ContainsKey(master.Name))
                        {
                            // Dont exist. add information to rundata note.
                            this._node.Masters.Add(master.Name, new RundataNodeServiceMastersModel()
                            {
                                Filedata = master,
                            });
                        }
                    }
                }
                #endregion

                #region Import Collections

                if ((tmpModel.Collections != null) && (tmpModel.Collections.Count > 0))
                {
                    foreach (var collection in tmpModel.Collections)
                    {
                        if (collection.Id == "demo")
                        {
                            continue;
                        }

                        // if (this._node.Collections.ContainsKey(collection.Id + "-default"))
                        if (this._collections.ContainsKey(collection.Id + "-default"))
                        {
                            continue;       // This collection already exist
                        }


                        this._collections.Add(collection.Id + "-default", new RundataNodeServiceCollectionModel()
                        {
                            Filedata = collection,
                        });

                        if ((collection.Areas != null) && (collection.Areas.Count > 0))
                        {
                            foreach (var area in collection.Areas)
                            {
                                if (this._collections.ContainsKey(collection.Id + "-" + area.Id))
                                {
                                    continue;       // This collection already exist
                                }

                                this._collections.Add(collection.Id + "-" + area.Id, new RundataNodeServiceCollectionModel()
                                {
                                    Filedata = area,
                                });
                            }
                        }
                    }
                }

                #endregion

                #region Import StoragePools

                if ((tmpModel.StoragePools != null) && (tmpModel.StoragePools.Count > 0))
                {
                    foreach (var storage in tmpModel.StoragePools)
                    {
                        if (storage.Id == "demo")
                        {
                            continue;
                        }

                        if (this._storagepool.ContainsKey(storage.Id))
                        {
                            continue;  // Already exist
                        }

                        this._storagepool.Add(storage.Id, new StoragePoolModel()
                        {
                            Filedata = storage,
                        });
                    }
                }
                #endregion

                this.zzDebug = "sdfdf";
            }
            else
            {
                this._node.ConfigFileData.NeedToBeSaved = true;
            }

            #endregion

            this._node.StartUpStatus.ConfigFileWorking = Models.Rundata.NodeStartUpStatusEnum.Done;

            this.zzDebug = "sdfdf";

            // var aa3 = DeserializeJson(aa1);
            // return Common.DeserializeJson<Settings>(Common.ReadTextFile(@filename));
            // .
            this.zzDebug = "sdfdf";
        }

        private void OnStarted()
        {
            this._logger.LogInformation("OnStarted has been called.");

            this.zzDebug = "sdfdf";

            // Perform post-startup activities here
        }

        private void OnStopping()
        {
            this._logger.LogInformation("NodeWorker | OnStopping | Stop all backgrounds work.");
            this._logger.LogInformation("NodeWorker | OnStopping | Stop all backgrounds work. | Done");

            // _logger.LogInformation("OnStopping has been called.");
            this.zzDebug = "sdfdf";

            // Perform on-stopping activities here
        }

        private void OnStopped()
        {
            // Perform post-stopped activities here
            this._logger.LogInformation("OnStopped has been called.");

            this.zzDebug = "sdfdf";
        }
    }
}
