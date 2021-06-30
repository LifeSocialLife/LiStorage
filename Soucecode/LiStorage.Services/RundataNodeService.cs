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
    using Microsoft.CodeAnalysis.VisualBasic;
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
            // this.Collections = new Dictionary<string, RundataNodeServiceCollectionModel>();
            //this.Storage = new Dictionary<string, StoragePoolModel>();
            // this.StorageLastChecked = Convert.ToDateTime("2000-01-01 00:00:00");
            
            this.StartUpStatus = new NodeStartUpStatusModel();
            this.ConfigFileData = new RundataNodeServiceConfigFileDataModel();
            this.DrivesInformation = new RundataNodeServiceDrivesInformationModel();
            this.zzDebug = "RundataNodeService";

        }

        /// <summary>
        /// Gets or sets storage model. Information about all storage pool in dictonary.
        /// </summary>
        //public Dictionary<string, StoragePoolModel> Storage { get; set; }

        /// <summary>
        /// Gets or sets when was storage last checked?.
        /// </summary>
        // public DateTime StorageLastChecked { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        private string zzDebug { get; set; }

        internal Dictionary<string, RundataNodeServiceMastersModel> Masters { get; set; }

        // internal Dictionary<string, RundataNodeServiceCollectionModel> Collections { get; set; }

        public NodeStartUpStatusModel StartUpStatus { get; set; }

        public RundataNodeServiceConfigFileDataModel ConfigFileData { get; set; }

        /// <summary>
        /// Gets or sets information about all drives that exist in this node.
        /// </summary>
        public RundataNodeServiceDrivesInformationModel DrivesInformation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RundataNodeService"/> class.
        /// </summary>
    }

    /// <summary>
    /// Drives information Model.
    /// </summary>
    public class RundataNodeServiceDrivesInformationModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RundataNodeServiceDrivesInformationModel"/> class.
        /// </summary>
        public RundataNodeServiceDrivesInformationModel()
        {
            this.Drive = new Dictionary<string, RundataNodeServiceDrivesInformationDictModel>();
            this.LastChecked = Convert.ToDateTime("2000-01-01 00:00:00");
            this.CheckedIsRunning = false;
        }

        /// <summary>
        /// Gets or sets information about all drives that exist in this node.
        /// </summary>
        public Dictionary<string, RundataNodeServiceDrivesInformationDictModel> Drive { get; set; }

        /// <summary>
        /// Gets or sets date and time in utc when it was last run.
        /// </summary>
        public DateTime LastChecked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is the drives collecter running now.
        /// </summary>
        public bool CheckedIsRunning { get; set; }
    }

    /// <summary>
    /// RundataNodeServiceDrivesInformationDictModel.
    /// </summary>
    public class RundataNodeServiceDrivesInformationDictModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RundataNodeServiceDrivesInformationDictModel"/> class.
        /// </summary>
        public RundataNodeServiceDrivesInformationDictModel()
        {
            this.ContainNewData = false;
            this.Data = new GetDrivesInformationReturnModel();
            this.DtAdded = DateTime.UtcNow;
            this.DtLastChecked = DateTime.UtcNow;
            this.DtLastUpdated = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets or sets a value indicating whether is this information new or old information?.
        /// </summary>
        public bool ContainNewData { get; set; }

        /// <summary>
        /// Gets or sets drivesInformation Return Model from Drives Helper.
        /// </summary>
        public GetDrivesInformationReturnModel Data { get; set; }

        /// <summary>
        /// Gets or sets when was it last updated.
        /// </summary>
        public DateTime DtLastUpdated { get; set; }

        /// <summary>
        /// Gets or sets when was it last checked.
        /// </summary>
        public DateTime DtLastChecked { get; set; }

        /// <summary>
        /// Gets or sets when was it added.
        /// </summary>
        public DateTime DtAdded { get; set; }
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
        
        public RundataNodeServiceCollectionModel()
        {
            this.Filedata = new NodeConfigFileModel_CollectionsAreas();
        }
    }

    public class RundataNodeServiceConfigFileDataModel
    {
        public string NodeName { get; set; }
        public string ClusterKey { get; set; }
        public string HeaderApiKey { get; set; }
        public UInt16 Version { get; set; }

        public bool NeedToBeSaved { get; set; }
        public RundataNodeServiceConfigFileDataModel()
        {
            
            this.NodeName = "";
            this.ClusterKey = "";
            this.HeaderApiKey = "";
            this.Version = 0;
            this.NeedToBeSaved = false;
        }
    }



}
