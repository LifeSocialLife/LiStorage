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
    using System.ComponentModel.Design;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using LiStorage.Models.StoragePool;
    using LiStorage.Services.Classes;
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
        private readonly object _lockKey;
#pragma warning restore SA1309 // Field names should not begin with underscore

        /// <summary>
        /// Initializes a new instance of the <see cref="StoragePoolService"/> class.
        /// </summary>
        /// <param name="logger">ILogger.</param>
        /// <param name="rundataService">RundataService.</param>
        /// <param name="fileOperation">FileOperationService.</param>
        /// <param name="rundataNode">RundataNodeService.</param>
        public StoragePoolService(ILogger<StoragePoolService> logger, RundataService rundataService, FileOperationService fileOperation, RundataNodeService rundataNode)
        {
            this.zzDebug = "StoragePoolService";

            this._logger = logger;

            this._rundata = rundataService;
            this._node = rundataNode;
            this._fileOperation = fileOperation;
            this.Storage = new Dictionary<string, StoragePoolModel>();
            this.StorageLastChecked = Convert.ToDateTime("2000-01-01 00:00:00");
            this._lockKey = new object();
        }

        /// <summary>
        /// Dp storage pool exist?.
        /// </summary>
        /// <param name="name">storage id (Key).</param>
        /// <returns>bool - true or false.</returns>
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:BracesMustNotBeOmitted", Justification = "Reviewed.")]
        public bool ContainsKey(string name)
        {
            bool tmpReturn = false;

            lock (this._lockKey)
            {
                if (this.Storage.ContainsKey(name))
                    tmpReturn = true;
            }

            return tmpReturn;
        }

        public bool Add(string key, StoragePoolModel data)
        {

            lock (this._lockKey)
            {
                this.Storage.Add(key,data);
            }

            return true;
        }

        /// <summary>
        /// Get information.
        /// </summary>
        /// <param name="key">storage pool id.</param>
        /// <returns>StoragePoolModel.</returns>
        internal StoragePoolModel Get(string key)
        {
            StoragePoolModel? data;

            lock (this._lockKey)
            {
                data = this.Storage[key];
            }

            return data;
        }

        /// <summary>
        /// Get meta file from storagepool.
        /// </summary>
        /// <param name="stg">StoragePoolModel.</param>
        /// <param name="path">string[] path.</param>
        /// <param name="data">BlockStorageDictModel.</param>
        /// <returns>model: BlockStorageDictModel.</returns>
        internal Models.ObjectStorage.BlockStorageDictModel GetMetaFile(StoragePoolModel stg, string[] path, Models.ObjectStorage.BlockStorageDictModel data)
        {
            data.FileMetaPath = Path.Combine("meta", Path.Combine(path)).Trim().ToLower() + ".meta";
            data.FileMetaPathWhitStoragePath = Path.Combine(stg.Filedata.FolderPath, data.FileMetaPath).Trim().ToLower();

            this.zzDebug = "sfdsf";

            // Check if meta data file exist?
            if (!LiTools.Helpers.IO.File.Exist(data.FileMetaPathWhitStoragePath))
            {
                data.FileMetaExist = false;
                this.zzDebug = "sdfds";
                return data;
            }


            this.zzDebug = "sdfds";
            return data;
        }

        /// <summary>
        /// Gets or sets storage model. Information about all storage pool in dictonary.
        /// </summary>
        private Dictionary<string, StoragePoolModel> Storage { get; set; }

        /// <summary>
        /// Gets or sets when was storage last checked?.
        /// </summary>
        private DateTime StorageLastChecked { get; set; }

        /// <summary>
        /// Check all storage pool that the are working as it shod.
        /// </summary>
        internal void CheckStoragePools()
        {
            this.zzDebug = "sdfdf";
            if (!Helpers.TimeHelper.TimeShodTrigger(this.StorageLastChecked, Helpers.TimeValuesEnum.Minutes, 2))
            {
                return;
            }

            if (this.Storage?.Count > 0)
            {
                lock (this._lockKey)
                {
                    foreach (var stg in this.Storage)
                    {
                        // Is init not done for a storage pool. run init.
                        if (!stg.Value.InitDone)
                        {
                            if (this.DoInitOnStoragePool(stg.Key))
                            {
                                stg.Value.Status = StoragePoolStatusEnum.Working;
                            }
                            else
                            {
                                stg.Value.Status = StoragePoolStatusEnum.Error;

                                if (System.Diagnostics.Debugger.IsAttached)
                                {
                                    System.Diagnostics.Debugger.Break();
                                }
                            }
                        }

                        stg.Value.DtLastCheck = DateTime.UtcNow;
                        this.zzDebug = "dsfdsf";
                    }
                }

            }

            this.zzDebug = "sdfd";

            // Collect information about drives in system.
            if (Helpers.TimeHelper.TimeShodTrigger(this._node.DrivesInformation.LastChecked, Helpers.TimeValuesEnum.Minutes, 2))
            {
                this.CollecNodesDriveInformation();
                this.zzDebug = "dfdf";
            }

            // var dddf = this._node.DrivesInformation;
            this.NodesDriveInformationDataChangeHandler();

            this.StorageLastChecked = DateTime.UtcNow;
            this.zzDebug = "sdfdf";
        }

        /// <summary>
        /// Collect information about all drives in running system.
        /// </summary>
        private void CollecNodesDriveInformation()
        {
            if (this._node.DrivesInformation.CheckedIsRunning)
            {
                return;
            }

            this._node.DrivesInformation.CheckedIsRunning = true;

            var tmpDriveInfo = LiTools.Helpers.IO.Drives.GetDrivesInformation(true, true);
            foreach (var di in tmpDriveInfo)
            {
                if (this._node.DrivesInformation.Drive.ContainsKey(di.Name))
                {
                    // Already exist. Check data inside of model
                    this._node.DrivesInformation.Drive[di.Name].DtLastChecked = DateTime.UtcNow;

                    if (this._node.DrivesInformation.Drive[di.Name].Data.IsReady != di.IsReady ||
                        this._node.DrivesInformation.Drive[di.Name].Data.AvailableFreeSpace != di.AvailableFreeSpace ||
                        this._node.DrivesInformation.Drive[di.Name].Data.DriveFormat != di.DriveFormat ||
                        this._node.DrivesInformation.Drive[di.Name].Data.DriveType != di.DriveType ||
                        this._node.DrivesInformation.Drive[di.Name].Data.TotalFreeSpace != di.TotalFreeSpace ||
                        this._node.DrivesInformation.Drive[di.Name].Data.TotalSize != di.TotalSize ||
                        this._node.DrivesInformation.Drive[di.Name].Data.VolumeLabel != di.VolumeLabel)
                    {
                        this._node.DrivesInformation.Drive[di.Name].Data = di;
                        this._node.DrivesInformation.Drive[di.Name].ContainNewData = true;

                        this._node.DrivesInformation.Drive[di.Name].DtLastUpdated = DateTime.UtcNow;
                    }
                }
                else
                {
                    this._node.DrivesInformation.Drive.Add(di.Name, new RundataNodeServiceDrivesInformationDictModel()
                    {
                        ContainNewData = true,
                        Data = di,
                        DtAdded = DateTime.UtcNow,
                        DtLastChecked = DateTime.UtcNow,
                        DtLastUpdated = DateTime.UtcNow,
                    });
                }

                this.zzDebug = "dsfd";
            }

            this._node.DrivesInformation.LastChecked = DateTime.UtcNow;
            this._node.DrivesInformation.CheckedIsRunning = false;
        }

        private void NodesDriveInformationDataChangeHandler()
        {
            foreach (var drive in this._node.DrivesInformation.Drive)
            {
                if (drive.Value.ContainNewData)
                {
                    // This drive has new information
                    // .
                    //var dd = this._node.Storage;

                    List<StoragePoolModel> tmpMatchStorage = new List<StoragePoolModel>();

                    // var tmpMatchStorage = this._node.Storage.Values.Where(x => x.Filedata.FolderPath.ToLower().StartsWith(drive.Value.Data.Name.ToLower())).ToList();
                    lock (this._lockKey)
                    {
                        tmpMatchStorage = this.Storage.Values.Where(x => x.Filedata.FolderPath.ToLower().StartsWith(drive.Value.Data.Name.ToLower())).ToList();
                    }

                    if (tmpMatchStorage.Count > 0)
                    {
                        foreach (var stg in tmpMatchStorage)
                        {
                            stg.SpaceFreeInMbytes = (ulong)LiTools.Helpers.Convert.Bytes.To(FileSizeEnums.Megabytes, drive.Value.Data.AvailableFreeSpace);
                            stg.SpaceFreeCollected = true;
                        }

                        this.zzDebug = "dsfdsf";
                    }

                    drive.Value.ContainNewData = false;
                    this.zzDebug = "sdfsd";
                }
            }

            this.zzDebug = "sdfsd";

        }

        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:BracesMustNotBeOmitted", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1123:DoNotPlaceRegionsWithinElements", Justification = "Reviewed.")]
        private bool DoInitOnStoragePool(string stgId)
        {
            if (!this.Storage.ContainsKey(stgId))
            {
                // StoragePool dont exist
                return false;
            }

            StoragePoolModel stg = this.Storage[stgId];

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
                // StoragePool config file dont exist. Create default configfile and save.

                // Generate default storage config file.
                stg.ConfigData = LiStorage.Helpers.Configuration.ConfigFileNodeHelper.NodeConfigStoragePoolConfigFile(5, 40);
                stg.ConfigData.Id = stg.Filedata.Id;

                // Convert to json
                var tmpconfigFileAsJson = Helpers.CommonHelper.SerializeJson(stg.ConfigData, true);

                if (tmpconfigFileAsJson != null)
                {
                    // Save file.
                    if (!this._fileOperation.WriteFile($"{stg.Filedata.FolderPath}\\{stg.Filedata.Id}.stg", tmpconfigFileAsJson, false))
                    {
                        // Error saving the file.
                        stg.Status = StoragePoolStatusEnum.FileMissing;
                        stg.InitDone = true;

                        this.zzDebug = "dfdsf";
                        if (System.Diagnostics.Debugger.IsAttached)
                            System.Diagnostics.Debugger.Break();
                        return false;
                    }
                }
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

            if ((dirMeta.Count == 0) && stg.ConfigData.AllowMeta)
            {
                // Meta dont exist. shod it exist.
                if (!this._fileOperation.DirectoryCreate($"{stg.Filedata.FolderPath}\\meta"))
                {
                    // Error creating meta folder
                    stg.Status = StoragePoolStatusEnum.Error;
                    stg.InitDone = true;
                    return false;
                }
            }

            if (dirData.Count == 0 && stg.ConfigData.AllowData)
            {
                // Meta dont exist. shod it exist.
                if (!this._fileOperation.DirectoryCreate($"{stg.Filedata.FolderPath}\\data"))
                {
                    // Error creating meta folder
                    stg.Status = StoragePoolStatusEnum.Error;
                    stg.InitDone = true;
                    return false;
                }
            }

            #endregion

            // var dd1 = this._fileOperation.DirectoryGetSize(stg.Filedata.FolderPath, true, FileSizeEnums.Megabytes);
            // var dd2 = this._fileOperation.DirectoryGetSize(@"E:\Xplane Custom Scenery", true, FileSizeEnums.Megabytes);
            // var dd3 = this._fileOperation.DirectoryGetSize(@"E:\games", true, FileSizeEnums.Megabytes);
            this.zzDebug = "sfdsf";

            stg.InitDone = true;
            stg.Status = StoragePoolStatusEnum.Working;
            return true;
        }

        private ulong GetUsedSpaceInMegabyte(string stgId)
        {
            if (!this.ContainsKey(stgId))
            {
                // Storage pool dont exist.
                return 0;
            }

            var dd1 = this._fileOperation.DirectoryGetSize(this.Get(stgId).Filedata.FolderPath, true, FileSizeEnums.Bytes);
            // var dd1 = this._fileOperation.DirectoryGetSize(this._node.Storage[stgId].Filedata.FolderPath, true, FileSizeEnums.Bytes);

            // Do folder conatin data?
            if (dd1 <= 100)
            {
                return 0;
            }
            else if (dd1 <= 1048576)
            {
                return 1;
            }

            long tmpdd1 = (long)dd1;

            double tmpReturn = LiTools.Helpers.Convert.Bytes.To(FileSizeEnums.Megabytes, (long)dd1);

            // ulong vOut = Convert.ToUInt64(tmpReturn);

            return Convert.ToUInt64(tmpReturn);
        }


       

    }
}
