using LiStorage.Models.StoragePool;
using LiTools.Helpers.Convert;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiStorage.Services.Node
{
    public class StoragePoolService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private string zzDebug { get; set; }

        private readonly ILogger<StoragePoolService> _logger;

        private readonly RundataService _rundata;
        private readonly RundataNodeService _node;
        private readonly FileOperationService _fileOperation;

        public StoragePoolService(ILogger<StoragePoolService> logger, RundataService rundataService, FileOperationService fileOperation, RundataNodeService rundataNode)
        {
            this.zzDebug = "NodeWorker";

            _logger = logger;
           
            this._rundata = rundataService;
            this._node = rundataNode;
            this._fileOperation = fileOperation;
        }

        internal void CheckStoragePool()
        {
            this.zzDebug = "sdfdf";

            if (this._node.Storage?.Count > 0)
            {
                foreach (var stg in this._node.Storage)
                {
                    if (!stg.Value.InitDone)
                        this.DoInitOnStoragePool(stg.Key);
                }
                
            }

            this.zzDebug = "sdfdf";
        }

        internal bool DoInitOnStoragePool(string stgId)
        {
            if (!this._node.Storage.ContainsKey(stgId))
                return false;       //  StoragePool dont exist

            var stg = this._node.Storage[stgId];

            if (!stg.Filedata.Enabled)
            {
                stg.Status = StoragePoolStatusEnum.Nodata;
                stg.InitDone = true;
                return false;
            }
            
            //  Check that directory exist.
            if (!this._fileOperation.DirectoryExist(stg.Filedata.FolderPath))
            {
                //  Directory dont exist.
                stg.Status = StoragePoolStatusEnum.Error;
                stg.InitDone = true;
                return false;
            }

            //  Do file storage pool config file exist in directory
            if (!this._fileOperation.FileExists($"{stg.Filedata.FolderPath}\\{stg.Filedata.Id}.stg"))
            {
                //  StoragePool config file dont exist. Create default configfile and save 

                //  Generate default storage config file.
                stg.ConfigData = LiStorage.Helpers.Configuration.ConfigFileNodeHelper.NodeConfigStoragePoolConfigFile(5,40);
                stg.ConfigData.Id = stg.Filedata.Id;

                //  Convert to json
                var tmpconfigFileAsJson = Helpers.CommonHelper.SerializeJson(stg.ConfigData, true);

                //  Save file.
                if (!this._fileOperation.WriteFile($"{stg.Filedata.FolderPath}\\{stg.Filedata.Id}.stg", tmpconfigFileAsJson, false))
                {
                    //  Error saving the file.
                    stg.Status = StoragePoolStatusEnum.FileMissing;
                    stg.InitDone = true;

                    this.zzDebug = "dfdsf";
                    return false;
                }
                this.zzDebug = "sdfdsf";
            }

            #region Read storage config file from storage pool volume

            var tmpConfigFileAsString = this._fileOperation.ReadTextFile($"{stg.Filedata.FolderPath}\\{stg.Filedata.Id}.stg");
            
            if (string.IsNullOrEmpty(tmpConfigFileAsString))
            {
                stg.Status = StoragePoolStatusEnum.Error;
                stg.InitDone = true;
                return false;
            }

            #endregion


            
            #region Get Version of configfile and convert to model

            UInt16 FileAsStringVersion = 0;

            if (tmpConfigFileAsString.Contains("Version"))
            {
                int hej = tmpConfigFileAsString.IndexOf("Version");
                // string tmpString = tmpConfigFileAsString.Substring(hej);
                string tmpString = tmpConfigFileAsString[hej..];


                if ((tmpString.Contains(":")) && (tmpString.Contains(",")))
                {
                    int tmpIdFirst = tmpString.IndexOf(":");
                    int tmpIdLast = tmpString.IndexOf(",");
                    string tmpVersionData = tmpString.Substring(tmpIdFirst + 1, tmpIdLast - tmpIdFirst - 1).Trim();

                    //  Convert version string into uint16
                    try
                    {
                        FileAsStringVersion = UInt16.Parse(tmpVersionData);
                        this.zzDebug = "sdfdsf";
                        //Console.WriteLine(result);
                    }
                    catch (FormatException)
                    {
                        this.zzDebug = "dsfdsf";
                        stg.Status = StoragePoolStatusEnum.Error;
                        stg.InitDone = true;
                        return false;
                        // Console.WriteLine($"Unable to parse '{input}'");
                    }


                    this.zzDebug = "dsfdsf";
                }
                else
                {
                    this.zzDebug = "dsfdsf";
                    stg.Status = StoragePoolStatusEnum.Error;
                    stg.InitDone = true;
                    return false;
                }
                    

                this.zzDebug = "sfdsf";

            }
            else
            {
                this.zzDebug = "dsfdsf";
                stg.Status = StoragePoolStatusEnum.Error;
                stg.InitDone = true;
                return false;
            }
                

            if (FileAsStringVersion == 0)
            {
                //  This shod not be 0
                this.zzDebug = "dsfdsf";
                stg.Status = StoragePoolStatusEnum.Error;
                stg.InitDone = true;
                return false;
            }

            this.zzDebug = "sdfdf";

            //  Convert json string into model
            stg.ConfigData = LiStorage.Helpers.CommonHelper.DeserializeJson<StoragePoolConfigFileModel>(tmpConfigFileAsString);

            if (FileAsStringVersion != stg.ConfigData.Version)
            {
                //  Version error in config file
                this.zzDebug = "dsfdsf";
                stg.Status = StoragePoolStatusEnum.Error;
                stg.InitDone = true;
                return false;

            }
            #endregion


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
