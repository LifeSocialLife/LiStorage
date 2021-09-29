// <summary>
// {one line to give the library's name and an idea of what it does.}
// </summary>
// <copyright file="RundataNodeService.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

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
            this.Masters = new Dictionary<string, RundataNodeServiceMastersModel>();
            this.StartUpStatus = new NodeStartUpStatusModel();
            this.ConfigFileData = new RundataNodeServiceConfigFileDataModel();
            this.DrivesInformation = new RundataNodeServiceDrivesInformationModel();
            this.zzDebug = "RundataNodeService";

            /* Old code. dont know if this shod be used more.
            // this._fileOperation = fileOperation;
            // this.Collections = new Dictionary<string, RundataNodeServiceCollectionModel>();
            //this.Storage = new Dictionary<string, StoragePoolModel>();
            // this.StorageLastChecked = Convert.ToDateTime("2000-01-01 00:00:00");
            **/
        }

        /* Old code. dont know if this shod be used more.
        /// <summary>
        /// Gets or sets storage model. Information about all storage pool in dictonary.
        /// </summary>
        //public Dictionary<string, StoragePoolModel> Storage { get; set; }

        /// <summary>
        /// Gets or sets when was storage last checked?.
        /// </summary>
        // public DateTime StorageLastChecked { get; set; }

        // internal Dictionary<string, RundataNodeServiceCollectionModel> Collections { get; set; }
         *
         **/

        /// <summary>
        /// Gets or sets dictionary all Master servers.
        /// </summary>
        public Dictionary<string, RundataNodeServiceMastersModel> Masters { get; set; }

        /// <summary>
        /// Gets or sets node startup Status.
        /// </summary>
        public NodeStartUpStatusModel StartUpStatus { get; set; }

        /// <summary>
        /// Gets or sets information about configfile.
        /// </summary>
        public RundataNodeServiceConfigFileDataModel ConfigFileData { get; set; }

        /// <summary>
        /// Gets or sets information about all drives that exist in this node.
        /// </summary>
        public RundataNodeServiceDrivesInformationModel DrivesInformation { get; set; }

        [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Reviewed.")]
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        private string zzDebug { get; set; }
    }
}
