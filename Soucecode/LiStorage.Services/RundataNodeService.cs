using LiStorage.Models.Rundata;
using LiStorage.Models.StoragePool;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiStorage.Services
{
    public class RundataNodeService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private string zzDebug { get; set; }

        internal Dictionary<string, RundataNodeServiceMastersModel> Masters { get; set; }
        internal Dictionary<string, RundataNodeServiceCollectionModel> Collections { get; set; }
        internal Dictionary<string, StoragePoolModel> Storage { get; set; }
        public NodeStartUpStatusModel StartUpStatus { get; set; }
        public RundataNodeServiceConfigFileDataModel ConfigFileData { get; set; }
        public RundataNodeService() // FileOperationService fileOperation)
        {
            //this._fileOperation = fileOperation;
            this.Masters = new Dictionary<string, RundataNodeServiceMastersModel>();
            this.Collections = new Dictionary<string, RundataNodeServiceCollectionModel>();
            this.Storage = new Dictionary<string, StoragePoolModel>();
            this.StartUpStatus = new NodeStartUpStatusModel();
            this.ConfigFileData = new RundataNodeServiceConfigFileDataModel();
            this.zzDebug = "RundataNodeService";

        }


    }

    public class RundataNodeServiceMastersModel
    {
        public NodeConfigFileModel_MasterServers Filedata { get; set; }
        public RundataNodeServiceMastersModel()
        {
            this.Filedata = new NodeConfigFileModel_MasterServers();
        }
    }

    public class RundataNodeServiceCollectionModel
    {
        internal NodeConfigFileModel_CollectionsAreas Filedata { get; set; }
        //public NodeConfigFileModel_MasterServers Fildata { get; set; }
        public RundataNodeServiceCollectionModel()
        {
            this.Filedata = new NodeConfigFileModel_CollectionsAreas();
        }
    }

    public class RundataNodeServiceConfigFileDataModel
    {
        public string NodeName { get; set; }
        public string ClusterKey { get; set; }
        public UInt16 Version { get; set; }

        public bool NeedToBeSaved { get; set; }
        public RundataNodeServiceConfigFileDataModel()
        {
            
            this.NodeName = "";
            this.ClusterKey = "";
            this.Version = 0;
            this.NeedToBeSaved = false;
        }
    }



}
