using LiStorage.Models.StoragePool;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace LiStorage.Models.Rundata
{
    public class NodeConfigFileModel
    {
        public string NodeName { get; set; }
        public string ClusterKey { get; set; }
        public UInt16 Version { get; set; }
        public List<NodeConfigFileModel_MasterServers> Masters { get; set; }
        public List<NodeConfigFileModel_Collections> Collections { get; set; }
        public List<StoragePoolNodeConfigPartModel> StoragePools { get; set; }
        public NodeConfigFileModel()
        {
            this.NodeName = "";
            this.ClusterKey = "";
            this.Version = 1;
            this.Masters = new List<NodeConfigFileModel_MasterServers>();
            this.Collections = new List<NodeConfigFileModel_Collections>();
            this.StoragePools = new List<StoragePoolNodeConfigPartModel>();

        }
    }

    #region Collection models

    public class NodeConfigFileModel_Collections : NodeConfigFileModel_CollectionsAreas
    {
        //public string Id { get; set; }
        //public string Name { get; set; }
        //public bool Enabled { get; set; }
        //public string StoragePoolMetaDefault { get; set; }
        //public string StoragePoolDataDefault { get; set; }
        public List<NodeConfigFileModel_CollectionsAreas> Areas { get; set; }

        public NodeConfigFileModel_Collections()
        {
            this.Id = "";
            this.Name = "";
            this.Enabled = false;
            this.StoragePoolMetaDefault = "";
            this.StoragePoolDataDefault = "";
            this.Areas = new List<NodeConfigFileModel_CollectionsAreas>();
        }
    }
    public class NodeConfigFileModel_CollectionsAreas
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StoragePoolMetaDefault { get; set; }
        public string StoragePoolDataDefault { get; set; }
        public bool Enabled { get; set; }

        public NodeConfigFileModel_CollectionsAreas()
        {
            this.Id = "";
            this.Name = "";
            this.StoragePoolMetaDefault = "";
            this.StoragePoolDataDefault = "";
            this.Enabled = false;
        }
    }

    #endregion

  

    public class NodeConfigFileModel_MasterServers
    {
        public string Name { get; set; }
        public string Ip { get; set; }
        public Int16 Port { get; set; }
        public UInt16 Prio { get; set; }

        public NodeConfigFileModel_MasterServers()
        {
            this.Name = "";
            this.Ip = "";
            this.Port = 30511;
            this.Prio = 5;
        }
    }
}

