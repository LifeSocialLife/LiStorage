// <summary>
// Config file Service.
// </summary>
// <copyright file="ConfigFileService.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Services.Node
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using LiStorage.Models.StoragePool;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Config File Service.
    /// </summary>
    public class ConfigFileService
    {
        private readonly ILogger<ConfigFileService> _logger;
        private readonly RundataService _rundata;
        private readonly RundataNodeService _node;
        private readonly CollectionService _collections;
        private readonly StoragePoolService _storagepool;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigFileService"/> class.
        /// </summary>
        /// <param name="rundataService">RundataService.</param>
        /// <param name="logger">ILogger.</param>
        /// <param name="rundataNode">RundataNodeService.</param>
        /// <param name="collectionService">CollectionService.</param>
        /// <param name="storagePoolService">StoragePoolService.</param>
        public ConfigFileService(RundataService rundataService, ILogger<ConfigFileService> logger, RundataNodeService rundataNode, CollectionService collectionService, StoragePoolService storagePoolService)
        {
            this._logger = logger;

            this._rundata = rundataService;
            this._node = rundataNode;
            this._collections = collectionService;
            this._storagepool = storagePoolService;
            this.zzDebug = "FileOperationService";
        }

        [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Reviewed.")]
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        private string zzDebug { get; set; }

        /// <summary>
        /// Read Congifuration file for this node.
        /// </summary>
        public void ReadNodeConfigFile()
        {
            // Set status reading configfile.
            this._node.StartUpStatus.ConfigFile = Models.Rundata.NodeStartUpStatusEnum.Running;

            #region Read information from node config file. if error exist return whit configfile status error.

            // Read information from configfile
            var tmpConfigFileReadData = LiTools.Helpers.IO.File.ReadTextFile(this._rundata.Folders.ConfigFile);

            if (!tmpConfigFileReadData.Item1)
            {
                // Error reading file.
                this._node.StartUpStatus.ConfigFile = Models.Rundata.NodeStartUpStatusEnum.Error;
                this._node.StartUpStatus.ConfigFileLastError = "File dont exist";
                return;
            }

            /*
            if ((!tmpConfigFileReadData.Item1) || string.IsNullOrEmpty(tmpConfigFileReadData.Item2))
            {
                // Error reading file.
                this._node.StartUpStatus.ConfigFile = Models.Rundata.NodeStartUpStatusEnum.Error;
                return;
            }
            */

            string tmpConfigFileAsString = tmpConfigFileReadData.Item2;

            if (string.IsNullOrEmpty(tmpConfigFileAsString))
            {
                this._node.StartUpStatus.ConfigFile = Models.Rundata.NodeStartUpStatusEnum.Error;
                this._node.StartUpStatus.ConfigFileLastError = "File is empty";
                return;
            }

            #endregion

            this.zzDebug = "sdfdsf";

            #region Get Version of configfile and convert to model

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
                    }
                    catch (FormatException)
                    {
                        // Error converting version string into ushort
                        this._node.StartUpStatus.ConfigFileLastError = "Error converting version string into ushort";
                        this._node.StartUpStatus.ConfigFile = Models.Rundata.NodeStartUpStatusEnum.Error;
                    }

                    if (this._node.ConfigFileData.Version == 0)
                    {
                        // Configfile version is 0. it can not be zero.
                        this._node.StartUpStatus.ConfigFileLastError = "Configfile version is 0. it can not be zero";
                        this._node.StartUpStatus.ConfigFile = Models.Rundata.NodeStartUpStatusEnum.Error;
                    }

                    this.zzDebug = "dsfdsf";
                }

                // Configfile is missing : or , after the version
                else
                {
                    this._node.StartUpStatus.ConfigFileLastError = "Configfile is missing : or , after the version";
                    this._node.StartUpStatus.ConfigFile = Models.Rundata.NodeStartUpStatusEnum.Error;
                }

                this.zzDebug = "sfdsf";
            }

            // String reading from config file dont have "version".
            else
            {
                this._node.StartUpStatus.ConfigFileLastError = "configfile is missing version";
                this._node.StartUpStatus.ConfigFile = Models.Rundata.NodeStartUpStatusEnum.Error;
            }

            this.zzDebug = "fdsf";

            // Do we hae error. return.
            if (this._node.StartUpStatus.ConfigFile == Models.Rundata.NodeStartUpStatusEnum.Error)
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

            if (this._node.ConfigFileData.NeedToBeSaved)
            {
                this.SaveConfigFile(tmpModel);
            }

            this._node.StartUpStatus.ConfigFile = Models.Rundata.NodeStartUpStatusEnum.Done;

            this.zzDebug = "sdfdf";

            // var aa3 = DeserializeJson(aa1);
            // return Common.DeserializeJson<Settings>(Common.ReadTextFile(@filename));
            // .
            this.zzDebug = "sdfdf";
        }

        /// <summary>
        /// Run check on configfile service.
        /// </summary>
        public void Check()
        {
            this.zzDebug = "Check";
        }

        private void SaveConfigFile(Models.Rundata.NodeConfigFileModel data)
        {
            // TODO - FIX THIS!!!!
        }
    }
}
