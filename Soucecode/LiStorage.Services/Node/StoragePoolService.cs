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
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using LiStorage.Models.Rundata;
    using LiStorage.Models.StoragePool;
    using LiStorage.Services.Classes;
    using LiTools.Helpers.Convert;
    using LiTools.Helpers.Organize;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Storag pools work service.
    /// </summary>
    public class StoragePoolService
    {
        private readonly ILogger<StoragePoolService> _logger;
        private readonly RundataService _rundata;
        private readonly RundataNodeService _node;
        private readonly FileOperationService _fileOperation;
        // private readonly TaskService _task;
        private readonly BackgroundWorkService _bgWork;
        private readonly object _lockKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoragePoolService"/> class.
        /// </summary>
        /// <param name="logger">ILogger.</param>
        /// <param name="rundataService">RundataService.</param>
        /// <param name="fileOperation">FileOperationService.</param>
        /// <param name="rundataNode">RundataNodeService.</param>
        /// <param name="taskService">TaskService.</param>
        public StoragePoolService(ILogger<StoragePoolService> logger, RundataService rundataService, RundataNodeService rundataNode, FileOperationService fileOperation, BackgroundWorkService taskService)
        {
            this.zzDebug = "StoragePoolService";

            this._logger = logger;

            this._rundata = rundataService;
            this._node = rundataNode;
            this._fileOperation = fileOperation;
            this._bgWork = taskService;
            this.Storage = new Dictionary<string, StoragePoolModel>();
            this.StorageLastChecked = Convert.ToDateTime("2000-01-01 00:00:00");
            this._lockKey = new object();
            this.InitDone = false;
            //this.BackgroundTaskRunning = false;
            //this.BackgroundTaskShodbeRunning = false;
            //this.BackgroundTaskLastRun = Convert.ToDateTime("2000-01-01 00:00:00");
        }

        /// <summary>
        /// Gets or sets a value indicating whether is init done on this service?.
        /// </summary>
        public bool InitDone { get; set; }

        private string TaskName => "storagepoolservice";

        //TODO remove variables.
        #region Background task Variables

        /*
         * 
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

        #endregion



        private string zzDebug { get; set; }

        /// <summary>
        /// Gets or sets storage model. Information about all storage pool in dictonary.
        /// </summary>
        private Dictionary<string, StoragePoolModel> Storage { get; set; }

        /// <summary>
        /// Gets or sets when was storage last checked?.
        /// </summary>
        private DateTime StorageLastChecked { get; set; }

        /// <summary>
        /// Dp storage pool exist?.
        /// </summary>
        /// <param name="name">storage id (Key).</param>
        /// <returns>bool - true or false.</returns>
        public bool ContainsKey(string name)
        {
            bool tmpReturn = false;

            lock (this._lockKey)
            {
                if (this.Storage.ContainsKey(name))
                {
                    tmpReturn = true;
                }
            }

            return tmpReturn;
        }

        /// <summary>
        /// Add data to storage pool.
        /// </summary>
        /// <param name="key">Storagepool Id.</param>
        /// <param name="data">Storagepool data.</param>
        /// <returns>True if it was added. else false.</returns>
        public bool Add(string key, StoragePoolModel data)
        {
            bool tmpReturn = false;

            lock (this._lockKey)
            {
                if (!this.Storage.ContainsKey(key))
                {
                    this.Storage.Add(key, data);
                    tmpReturn = true;
                }
            }

            return tmpReturn;
        }

        

        /// <summary>
        /// Get information.
        /// </summary>
        /// <param name="key">storage pool id.</param>
        /// <returns>StoragePoolModel.</returns>
        public StoragePoolModel Get(string key)
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
        public Models.ObjectStorage.BlockStorageDictModel GetMetaFile(StoragePoolModel stg, string[] path, Models.ObjectStorage.BlockStorageDictModel data)
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
        /// Run check on storagepools.
        /// </summary>
        public void BackgroundTaskChecker()
        {
            this.zzDebug = "sfdsf";

            // Init it not done. return
            if (!this.InitDone)
            {
                return;
            }

            /*
            bool startBackgroundTask = false;
            bool bgTaskStart = false;
            bool bgTaskRestart = false;
            */

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
                    WhileInterval = 1000 * 60 * 5,          //  5min.
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

            return;
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
            this._logger.LogInformation("StoragePoolService running at: {time}", DateTimeOffset.UtcNow);

            // Check all storagepools.
            if (Helpers.TimeHelper.TimeShodTrigger(this.StorageLastChecked, Helpers.TimeValuesEnum.Minutes, 2))
            {
                this.CheckStoragePools();
            }

            // Collect information about drives in system.
            if (Helpers.TimeHelper.TimeShodTrigger(this._node.DrivesInformation.LastChecked, Helpers.TimeValuesEnum.Minutes, 2))
            {
                this.CollecNodesDriveInformation();
                this.zzDebug = "dfdf";

                // var dddf = this._node.DrivesInformation;
                this.NodesDriveInformationDataChangeHandler();
            }



            #region Old code.

            /*
            // int i = 0;

            // while (!LiTools.Helpers.Organize.ParallelTask.Token.IsCancellationRequested)
            while (!this._task.IsCancellationRequested("storagepoolservice"))
            {
                // Debug code
                
                i++;
                if (i == 10)
                {
                    i = 99;
                    this._task.CancelTask("storagepoolservice");
                }
                

                this.BackgroundTaskRunning = true;
                this.BackgroundTaskLastRun = DateTime.UtcNow;
                this.CheckStoragePools();

                if (!this.BackgroundTaskShodbeRunning)
                {
                    break;
                }

                this._logger.LogInformation("StoragePoolService running at: {time}", DateTimeOffset.Now);
                try
                {
                    System.Threading.Thread.Sleep(1000);

                    // Task.Delay(1000, LiTools.Helpers.Organize.ParallelTask.Token.Token);
                    // await Task.Delay(1000, LiTools.Helpers.Organize.ParallelTask.Token.Token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }

            */
            #endregion

            this.zzDebug = "sdfdsf";
        }

        /// <summary>
        /// Check all storage pool that the are working as it shod.
        /// </summary>
        private void CheckStoragePools()
        {
            this.zzDebug = "sdfdf";

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
                    // var dd = this._node.Storage;
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
        private bool DoInitOnStoragePool(string stgId)
        {
            if (!this.Storage.ContainsKey(stgId))
            {
                // StoragePool dont exist
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }

                return false;
            }

            StoragePoolModel stg = this.Storage[stgId];

            if (!stg.Filedata.Enabled)
            {
                stg.Status = StoragePoolStatusEnum.Nodata;
                stg.InitDone = true;
                return false;
            }

            this.zzDebug = "sfdsf";

            // Check that directory exist.
            // if (!this._fileOperation.DirectoryExist(stg.Filedata.FolderPath))
            if (!LiTools.Helpers.IO.Directory.Exist(stg.Filedata.FolderPath))
            {
                // Directory dont exist.
                stg.Status = StoragePoolStatusEnum.Error;
                stg.InitDone = true;
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }

                return false;
            }

            this.zzDebug = "sfdf";

            // Do file storage pool config file exist in directory
            // if (!this._fileOperation.FileExists($"{stg.Filedata.FolderPath}\\{stg.Filedata.Id}.stg"))
            if (!LiTools.Helpers.IO.File.Exist($"{stg.Filedata.FolderPath}\\{stg.Filedata.Id}.stg"))
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
                    // if (!this._fileOperation.WriteFile($"{stg.Filedata.FolderPath}\\{stg.Filedata.Id}.stg", tmpconfigFileAsJson, false))
                    if (!LiTools.Helpers.IO.File.WriteFile($"{stg.Filedata.FolderPath}\\{stg.Filedata.Id}.stg", tmpconfigFileAsJson, false))
                    {
                        // Error saving the file.
                        stg.Status = StoragePoolStatusEnum.FileMissing;
                        stg.InitDone = true;

                        this.zzDebug = "dfdsf";
                        if (System.Diagnostics.Debugger.IsAttached)
                        {
                            System.Diagnostics.Debugger.Break();
                        }

                        return false;
                    }
                }
            }

            // Read storage config file from storage pool volume
            // var tmpConfigFileAsString = this._fileOperation.ReadTextFile($"{stg.Filedata.FolderPath}\\{stg.Filedata.Id}.stg");
            var tmpConfigFilReadData = LiTools.Helpers.IO.File.ReadTextFile($"{stg.Filedata.FolderPath}\\{stg.Filedata.Id}.stg");

            if ((!tmpConfigFilReadData.Item1) || string.IsNullOrEmpty(tmpConfigFilReadData.Item2))
            {
                stg.Status = StoragePoolStatusEnum.Error;
                stg.InitDone = true;
                return false;
            }

            string tmpConfigFileAsString = tmpConfigFilReadData.Item2;

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

            // var aa = this._fileOperation.GetSubdirectoryList(stg.Filedata.FolderPath, false);
            var aaGetData = LiTools.Helpers.IO.Directory.GetSubdirectoryList(stg.Filedata.FolderPath, false);

            if (!aaGetData.Item1)
            {
                // Error read folders
                stg.Status = StoragePoolStatusEnum.Error;
                stg.InitDone = true;
                return false;
            }

            var aa = aaGetData.Item2;
            var dirData = aa.FindAll(s => s.IndexOf("data", StringComparison.OrdinalIgnoreCase) >= 0);
            var dirMeta = aa.FindAll(s => s.IndexOf("meta", StringComparison.OrdinalIgnoreCase) >= 0);

            this.zzDebug = "dsfd";

            if ((dirMeta.Count == 0) && stg.ConfigData.AllowMeta)
            {
                // Meta dont exist. shod it exist.
                // if (!this._fileOperation.DirectoryCreate($"{stg.Filedata.FolderPath}\\meta"))
                if (!LiTools.Helpers.IO.Directory.Create($"{stg.Filedata.FolderPath}\\meta"))
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
                // if (!this._fileOperation.DirectoryCreate($"{stg.Filedata.FolderPath}\\data"))
                if (!LiTools.Helpers.IO.Directory.Create($"{stg.Filedata.FolderPath}\\data"))
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

            // var dd1 = this._fileOperation.DirectoryGetSize(this._node.Storage[stgId].Filedata.FolderPath, true, FileSizeEnums.Bytes);
            // var dd1 = this._fileOperation.DirectoryGetSize(this.Get(stgId).Filedata.FolderPath, true, FileSizeEnums.Bytes);
            var dd1 = LiTools.Helpers.IO.Directory.DirectoryGetSize(this.Get(stgId).Filedata.FolderPath, true, FileSizeEnums.Bytes);

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
