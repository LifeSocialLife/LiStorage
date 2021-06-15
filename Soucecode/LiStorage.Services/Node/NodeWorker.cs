using System;using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace LiStorage.Services.Node
{
    public class NodeWorker : BackgroundService
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private string zzDebug { get; set; }

        private readonly ILogger<NodeWorker> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        private readonly RundataService _rundata;
        private readonly RundataNodeService _node;
        private readonly FileOperationService _fileOperation;
        private readonly StoragePoolService _storagepool;

        public NodeWorker(ILogger<NodeWorker> logger, IHostApplicationLifetime hostappLifetime, RundataService rundataService, FileOperationService fileOperation, RundataNodeService rundataNode, StoragePoolService storagePoolService)
        {
            this.zzDebug = "NodeWorker";

            _logger = logger;


            this._hostApplicationLifetime = hostappLifetime;
            this._rundata = rundataService;
            this._node = rundataNode;
            this._fileOperation = fileOperation;
            this._storagepool = storagePoolService;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _hostApplicationLifetime.ApplicationStarted.Register(OnStarted, true);
            _hostApplicationLifetime.ApplicationStopping.Register(OnStopping, true);
            _hostApplicationLifetime.ApplicationStopped.Register(OnStopped, true);


            #region Do we have a configfile for this node.

            this._rundata.Folders.ConfigFile = this._fileOperation.LocateFileInKnownLocations("LiStorageNode.conf");
            this._node.StartUpStatus.ConfigFileExist = Models.Rundata.NodeStartUpStatusEnum.Running;
            if (string.IsNullOrEmpty(this._rundata.Folders.ConfigFile))
            {
                this._node.StartUpStatus.ConfigFileExist = Models.Rundata.NodeStartUpStatusEnum.Error;
            }
            else
                this._node.StartUpStatus.ConfigFileExist = Models.Rundata.NodeStartUpStatusEnum.Done;

            #endregion


            #region DEV - Create configfile for dev node


            //var gg = LiStorage.Helpers.Configuration.ConfigFileNodeHelper.CreateTemplate();
            //var a1 = Helpers.CommonHelper.SerializeJson(gg, true);

            this.zzDebug = "sdfdf";

            //this._fileOperation.WriteFile(this._rundata.Folders.ConfigFile, a1, false);


            #endregion


            this.zzDebug = "sdfdf";


            // Get configuration file. if the configfile exist in system
            if (this._node.StartUpStatus.ConfigFileExist == Models.Rundata.NodeStartUpStatusEnum.Done)
                this.ReadConfigFile();
            else
            {
                this._node.StartUpStatus.ConfigFileExist = Models.Rundata.NodeStartUpStatusEnum.Error;
            }
            
            var ddd = this._rundata;
            




            this.zzDebug = "sdfdf";

            

            while (!stoppingToken.IsCancellationRequested)
            {
               

                //_logger.LogInformation($"Background service running :: {stoppingToken.IsCancellationRequested}");
                await Task.Delay(2000, stoppingToken);
            }




            this.zzDebug = "sdfdsf";


        }

        private void ReadConfigFile()
        {
            //  Read data from configfile
            this._node.StartUpStatus.ConfigFileRead = Models.Rundata.NodeStartUpStatusEnum.Running;
            var tmpConfigFileAsString = this._fileOperation.ReadTextFile(this._rundata.Folders.ConfigFile);

            if (string.IsNullOrEmpty(tmpConfigFileAsString))
            {
                this._node.StartUpStatus.ConfigFileRead = Models.Rundata.NodeStartUpStatusEnum.Error;
                return;

                //  
            }
            this._node.StartUpStatus.ConfigFileRead = Models.Rundata.NodeStartUpStatusEnum.Done;


            #region Get Version of configfile and convert to model
            
            this._node.StartUpStatus.ConfigFileWorking = Models.Rundata.NodeStartUpStatusEnum.Running;
            
            if (tmpConfigFileAsString.Contains("Version"))
            {
                int hej = tmpConfigFileAsString.IndexOf("Version");
                string tmpString = tmpConfigFileAsString.Substring(hej);
                
                if ((tmpString.Contains(":")) && (tmpString.Contains(",")))
                {
                    int tmpIdFirst = tmpString.IndexOf(":");
                    int tmpIdLast = tmpString.IndexOf(",");
                    string tmpVersionData = tmpString.Substring(tmpIdFirst + 1, tmpIdLast - tmpIdFirst - 1).Trim();

                    //  Convert version string into uint16
                    try
                    {
                        this._node.ConfigFileData.Version = UInt16.Parse(tmpVersionData);
                        //Console.WriteLine(result);
                    }
                    catch (FormatException)
                    {
                        this._node.StartUpStatus.ConfigFileWorking = Models.Rundata.NodeStartUpStatusEnum.Error;
                        // Console.WriteLine($"Unable to parse '{input}'");
                    }

                    if (this._node.ConfigFileData.Version == 0)
                        this._node.StartUpStatus.ConfigFileWorking = Models.Rundata.NodeStartUpStatusEnum.Error;

                    this.zzDebug = "dsfdsf";
                }
                else
                    this._node.StartUpStatus.ConfigFileWorking = Models.Rundata.NodeStartUpStatusEnum.Error;

                this.zzDebug = "sfdsf";

            }
            else
                this._node.StartUpStatus.ConfigFileWorking = Models.Rundata.NodeStartUpStatusEnum.Error;
            


            this.zzDebug = "fdsf";

            if (this._node.StartUpStatus.ConfigFileWorking == Models.Rundata.NodeStartUpStatusEnum.Error)
                return;

          
            this.zzDebug = "sdfdf";

            //  Convert json string into model

            var tmpModel = LiStorage.Helpers.CommonHelper.DeserializeJson<Models.Rundata.NodeConfigFileModel>(tmpConfigFileAsString);

            #endregion

            #region Importdata into Node rundata service.

            if (tmpModel.Version == 1)
            {
                //  This is a version 1 import.

                this._node.ConfigFileData.NodeName = tmpModel.NodeName;
                this._node.ConfigFileData.ClusterKey = tmpModel.ClusterKey;

                #region Import masters

                if ((tmpModel.Masters != null) && (tmpModel.Masters.Count >0))
                {
                    foreach (var master in tmpModel.Masters)
                    {
                        if (master.Name == "demo") continue;

                        if (!this._node.Masters.ContainsKey(master.Name))
                        {
                            //  Dont exist. add information to rundata note.
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
                        if (collection.Id == "demo") continue;

                        if (this._node.Collections.ContainsKey(collection.Id + "-default"))
                            continue;       // This collection already exist

                        this._node.Collections.Add(collection.Id + "-default", new RundataNodeServiceCollectionModel()
                        {
                            Filedata = collection,
                        });

                        if ((collection.Areas != null) && (collection.Areas.Count > 0))
                        {
                            foreach (var area in collection.Areas)
                            {
                                if (this._node.Collections.ContainsKey(collection.Id + "-" + area.Id))
                                    continue;       // This collection already exist

                                this._node.Collections.Add(collection.Id + "-" + area.Id, new RundataNodeServiceCollectionModel()
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
                        if (storage.Id == "demo") continue;

                        if (this._node.Storage.ContainsKey(storage.Id))
                            continue;  //   Already exist

                        this._node.Storage.Add(storage.Id, new RundataNodeServiceStorageModel()
                        {
                            Filedata = storage,
                        });

                        
                    }
                        
                }

                #endregion
                    this.zzDebug = "sdfdf";
            }
            else
                this._node.ConfigFileData.NeedToBeSaved = true;


            #endregion


            this._node.StartUpStatus.ConfigFileWorking = Models.Rundata.NodeStartUpStatusEnum.Done;


            

            this.zzDebug = "sdfdf";



            
            this.zzDebug = "sdfd";

            
            // var aa3 = DeserializeJson(aa1);


            // return Common.DeserializeJson<Settings>(Common.ReadTextFile(@filename));

            this.zzDebug = "sdfdf";

        }

         
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Background service stopping : {cancellationToken.IsCancellationRequested}");


            this.zzDebug = "sdfdf";

            await Task.CompletedTask;
        }

        private void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");

            this.zzDebug = "sdfdf";
            // Perform post-startup activities here
        }

        private void OnStopping()
        {
            _logger.LogInformation("NodeWorker | OnStopping | Stop all backgrounds work.");

         

            _logger.LogInformation("NodeWorker | OnStopping | Stop all backgrounds work. | Done");
            // _logger.LogInformation("OnStopping has been called.");

            this.zzDebug = "sdfdf";
            // Perform on-stopping activities here
        }

        private void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");

            this.zzDebug = "sdfdf";
            // Perform post-stopped activities here
        }
    }
}
