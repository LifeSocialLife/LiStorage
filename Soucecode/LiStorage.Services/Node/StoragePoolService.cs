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
    using System.IO;
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
                    {
                        if (this.DoInitOnStoragePool(stg.Key))
                        {
                            stg.Value.Status = StoragePoolStatusEnum.Working;
                        }
                        else
                        {
                            stg.Value.Status = StoragePoolStatusEnum.Error;
                        }
                    }

                    var ss = this.GetUsedSpaceInMegabyte(stg.Key);

                    // var dd1 = this._fileOperation.DirectoryGetSize(stg.Filedata.FolderPath, true, FileSizeEnums.Megabytes);
                    // var dd2 = this.GetUsedSpaceInMegabyte(@"E:\Xplane Custom Scenery"); // , true, FileSizeEnums.Megabytes);
                    // var dd3 = this.GetUsedSpaceInMegabyte(@"E:\games"); // , true, FileSizeEnums.Megabytes);

                    this.zzDebug = "dsfdsf";
                }
            }

            this.zzDebug = "sdfd";

            // Collect information about drives in system.
            this.CollecNodesDriveInformation();


            var dddf = this._node.DrivesInformation;




            this.zzDebug = "sdfdf";
        }


        private void CollecNodesDriveInformation()
        {
            var tmpDriveInfo = LiTools.Helpers.IO.Drives.GetDrivesInformation(true, true);
            foreach (var di in tmpDriveInfo)
            {
                if (this._node.DrivesInformation.ContainsKey(di.Name))
                {
                    // Already exist. Check data inside of model
                    this._node.DrivesInformation[di.Name].DtLastChecked = DateTime.UtcNow;

                    if (this._node.DrivesInformation[di.Name].Data.IsReady != di.IsReady ||
                        this._node.DrivesInformation[di.Name].Data.AvailableFreeSpace != di.AvailableFreeSpace ||
                        this._node.DrivesInformation[di.Name].Data.DriveFormat != di.DriveFormat ||
                        this._node.DrivesInformation[di.Name].Data.DriveType != di.DriveType ||
                        this._node.DrivesInformation[di.Name].Data.TotalFreeSpace != di.TotalFreeSpace ||
                        this._node.DrivesInformation[di.Name].Data.TotalSize != di.TotalSize ||
                        this._node.DrivesInformation[di.Name].Data.VolumeLabel != di.VolumeLabel)
                    {
                        this._node.DrivesInformation[di.Name].Data = di;
                        this._node.DrivesInformation[di.Name].ContainNewData = true;

                        this._node.DrivesInformation[di.Name].DtLastUpdated = DateTime.UtcNow;
                    }
                }
                else
                {
                    this._node.DrivesInformation.Add(di.Name, new RundataNodeServiceDrivesInformationModel()
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
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1123:DoNotPlaceRegionsWithinElements", Justification = "Reviewed.")]
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
                stg.ConfigData = LiStorage.Helpers.Configuration.ConfigFileNodeHelper.NodeConfigStoragePoolConfigFile(5, 40);
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

            return false;
        }

        private ulong GetUsedSpaceInMegabyte(string stgId)
        {
            if (!this._node.Storage.ContainsKey(stgId))
            {
                // Storage pool dont exist.
                return 0;
            }

            var dd1 = this._fileOperation.DirectoryGetSize(this._node.Storage[stgId].Filedata.FolderPath, true, FileSizeEnums.Bytes);

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

        private void GetFreeSpaceOnDrives()
        {

            

        }
    }
}
