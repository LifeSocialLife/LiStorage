// <summary>
// Storage pool work.
// </summary>
// <copyright file="StoragePoolService.cs" company="LiSoLi">
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
    using LiTools.Helpers.Convert;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Storag pools work service.
    /// </summary>
    public class StoragePoolService
    {



        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        private string zzDebug { get; set; }

#pragma warning disable SA1309 // Field names should not begin with underscore
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Reviewed.")]
        private readonly ILogger<StoragePoolService> _logger;
        private readonly RundataService _rundata;
        private readonly RundataNodeService _node;
        private readonly FileOperationService _fileOperation;
#pragma warning restore SA1309 // Field names should not begin with underscore

        /// <summary>
        /// Initializes a new instance of the <see cref="StoragePoolService"/> class.
        /// </summary>
        /// <param name="logger">ILogger.</param>
        /// <param name="rundataService">RundataService</param>
        /// <param name="fileOperation">FileOperationService</param>
        /// <param name="rundataNode">RundataNodeService.</param>
        public StoragePoolService(ILogger<StoragePoolService> logger, RundataService rundataService, FileOperationService fileOperation, RundataNodeService rundataNode)
        {
            this.zzDebug = "StoragePoolService";

            this._logger = logger;

            this._rundata = rundataService;
            this._node = rundataNode;
            this._fileOperation = fileOperation;
        }


        /// <summary>
        /// Check all storage pool that the are working as it shod.
        /// </summary>
        internal void CheckStoragePool()
        {
            this.zzDebug = "sdfdf";

            if (this._node.Storage?.Count > 0)
            {
                foreach (var stg in this._node.Storage)
                {
                    // Is init not done for a storage pool. run init.
                    if (!stg.Value.InitDone)
                        this.DoInitOnStoragePool(stg.Key);
                }
            }

            this.zzDebug = "sdfdf";
        }

        private bool DoInitOnStoragePool(string stgId)
        {
            if (!this._node.Storage.ContainsKey(stgId))
            {
                // StoragePool dont exist
                return false;
            }

            StoragePoolModel stg = this._node.Storage[stgId];

            if (!stg.Filedata.Enabled)
            {
                stg.Status = StoragePoolStatusEnum.Nodata;
                stg.InitDone = true;
                return false;
            }

            // Check that directory exist.
            if (!this._fileOperation.DirectoryExist(stg.Filedata.FolderPath))
            {
                // Directory dont exist.
                stg.Status = StoragePoolStatusEnum.Error;
                stg.InitDone = true;
                return false;
            }

            // Do file storage pool config file exist in directory
            if (!this._fileOperation.FileExists($"{stg.Filedata.FolderPath}\\{stg.Filedata.Id}.stg"))
            {
                // StoragePool config file dont exist. Create default configfile and save 

                // Generate default storage config file.
                stg.ConfigData = LiStorage.Helpers.Configuration.ConfigFileNodeHelper.NodeConfigStoragePoolConfigFile(5,40);
                stg.ConfigData.Id = stg.Filedata.Id;

                // Convert to json
                var tmpconfigFileAsJson = Helpers.CommonHelper.SerializeJson(stg.ConfigData, true);

                // Save file.
                if (!this._fileOperation.WriteFile($"{stg.Filedata.FolderPath}\\{stg.Filedata.Id}.stg", tmpconfigFileAsJson, false))
                {
                    // Error saving the file.
                    stg.Status = StoragePoolStatusEnum.FileMissing;
                    stg.InitDone = true;

                    this.zzDebug = "dfdsf";
                    return false;
                }

                this.zzDebug = "sdfdsf";
            }

            // Read storage config file from storage pool volume
            var tmpConfigFileAsString = this._fileOperation.ReadTextFile($"{stg.Filedata.FolderPath}\\{stg.Filedata.Id}.stg");

            if (string.IsNullOrEmpty(tmpConfigFileAsString))
            {
                stg.Status = StoragePoolStatusEnum.Error;
                stg.InitDone = true;
                return false;
            }

            // Get Version of configfile.
            var tmpVersionFromString = LiStorage.Helpers.CommonHelper.GetValueFromJsonStringReturnAsUshort(tmpConfigFileAsString, "Version");
            if (!tmpVersionFromString.Item1)
            {
                this.zzDebug = "dsfdsf";
                stg.Status = StoragePoolStatusEnum.Error;
                stg.InitDone = true;
                return false;
            }

            if (tmpVersionFromString.Item2 == 0)
            {
                // This shod not be 0
                this.zzDebug = "dsfdsf";
                stg.Status = StoragePoolStatusEnum.Error;
                stg.InitDone = true;
                return false;
            }

            this.zzDebug = "sdfdf";

            // Convert json string into model
            stg.ConfigData = LiStorage.Helpers.CommonHelper.DeserializeJson<StoragePoolConfigFileModel>(tmpConfigFileAsString);

            if (tmpVersionFromString.Item2 != stg.ConfigData.Version)
            {
                // Version error in config file
                this.zzDebug = "dsfdsf";
                stg.Status = StoragePoolStatusEnum.Error;
                stg.InitDone = true;
                return false;
            }

            #region Create data and meta folder in stg folderpath if the dont exist And shod exist.

            var aa = this._fileOperation.GetSubdirectoryList(stg.Filedata.FolderPath, false);

            var dirData = aa.FindAll(s => s.IndexOf("data", StringComparison.OrdinalIgnoreCase) >= 0);
            var dirMeta = aa.FindAll(s => s.IndexOf("meta", StringComparison.OrdinalIgnoreCase) >= 0);

            this.zzDebug = "dsfd";

            if ((dirMeta.Count == 0) && (stg.ConfigData.AllowMeta))
            {
                //  Meta dont exist. shod it exist.
                if (!this._fileOperation.DirectoryCreate($"{stg.Filedata.FolderPath}\\meta"))
                {
                    //  Error creating meta folder
                    stg.Status = StoragePoolStatusEnum.Error;
                    stg.InitDone = true;
                    return false;
                }
            }

            if ((dirData.Count == 0) && (stg.ConfigData.AllowData))
            {
                //  Meta dont exist. shod it exist.
                if (!this._fileOperation.DirectoryCreate($"{stg.Filedata.FolderPath}\\data"))
                {
                    //  Error creating meta folder
                    stg.Status = StoragePoolStatusEnum.Error;
                    stg.InitDone = true;
                    return false;
                }
            }


            #endregion


            var dd1 = this._fileOperation.DirectoryGetSize(stg.Filedata.FolderPath, true, FileSizeEnums.Megabytes);
            var dd2 = this._fileOperation.DirectoryGetSize(@"E:\Xplane Custom Scenery", true, FileSizeEnums.Megabytes);
            var dd3 = this._fileOperation.DirectoryGetSize(@"E:\games", true, FileSizeEnums.Megabytes);

            this.zzDebug = "sfdsf";

            return false;
        }
    }
}
