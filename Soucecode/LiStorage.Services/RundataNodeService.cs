// <copyright file="RundataNodeService.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>

namespace LiStorage.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using LiStorage.Models.Rundata;
    using LiStorage.Models.StoragePool;
    using static LiTools.Helpers.IO.Drives;

    /// <summary>
    /// Rundata Node Service.
    /// </summary>
    public class RundataNodeService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RundataNodeService"/> class.
        /// </summary>
        public RundataNodeService() // FileOperationService fileOperation)
        {
            // this._fileOperation = fileOperation;
            this.Masters = new Dictionary<string, RundataNodeServiceMastersModel>();
            this.Collections = new Dictionary<string, RundataNodeServiceCollectionModel>();
            this.Storage = new Dictionary<string, StoragePoolModel>();
            this.StartUpStatus = new NodeStartUpStatusModel();
            this.ConfigFileData = new RundataNodeServiceConfigFileDataModel();
            this.DrivesInformation = new Dictionary<string, RundataNodeServiceDrivesInformationModel>();
            this.zzDebug = "RundataNodeService";

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        private string zzDebug { get; set; }

        internal Dictionary<string, RundataNodeServiceMastersModel> Masters { get; set; }

        internal Dictionary<string, RundataNodeServiceCollectionModel> Collections { get; set; }

        internal Dictionary<string, StoragePoolModel> Storage { get; set; }

        public NodeStartUpStatusModel StartUpStatus { get; set; }

        public RundataNodeServiceConfigFileDataModel ConfigFileData { get; set; }

        /// <summary>
        /// Gets or sets information about all drives that exist in this node.
        /// </summary>
        public Dictionary<string, RundataNodeServiceDrivesInformationModel> DrivesInformation { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="RundataNodeService"/> class.
        /// </summary>



    }

    /// <summary>
    /// RundataNodeServiceDrivesInformationModel.
    /// </summary>
    public class RundataNodeServiceDrivesInformationModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether is this information new or old information?.
        /// </summary>
        public bool ContainNewData { get; set; }

        /// <summary>
        /// Gets or sets drivesInformation Return Model from Drives Helper.
        /// </summary>
        public GetDrivesInformationReturnModel Data { get; set; }

        /// <summary>
        /// When was it last updated.
        /// </summary>
        public DateTime DtLastUpdated { get; set; }

        /// <summary>
        /// When was it last checked.
        /// </summary>
        public DateTime DtLastChecked { get; set; }

        /// <summary>
        /// When was it added.
        /// </summary>
        public DateTime DtAdded { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RundataNodeServiceDrivesInformationModel"/> class.
    /// </summary>
        public RundataNodeServiceDrivesInformationModel()
        {
            this.ContainNewData = false;
            this.Data = new GetDrivesInformationReturnModel();
            this.DtAdded = DateTime.UtcNow;
            this.DtLastChecked = DateTime.UtcNow;
            this.DtLastUpdated = DateTime.UtcNow;
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
