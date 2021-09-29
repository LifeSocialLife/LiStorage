// <summary>
// Main configuration file for node model.
// </summary>
// <copyright file="NodeConfigFileModel.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Models.Rundata
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using LiStorage.Models.StoragePool;

    /// <summary>
    /// Node configuration file model.
    /// </summary>
    public class NodeConfigFileModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeConfigFileModel"/> class.
        /// </summary>
        public NodeConfigFileModel()
        {
            this.NodeName = string.Empty;
            this.ClusterKey = string.Empty;
            this.HeaderApiKey = string.Empty;
            this.Version = 1;
            this.Masters = new List<NodeConfigFileModel_MasterServers>();
            this.Collections = new List<NodeConfigFileModel_Collections>();
            this.StoragePools = new List<StoragePoolNodeConfigPartModel>();
        }

        /// <summary>
        /// Gets or sets name of this node.
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// Gets or sets security key for the cluster.
        /// </summary>
        public string ClusterKey { get; set; }

        /// <summary>
        /// Gets or sets header security key for access.
        /// </summary>
        public string HeaderApiKey { get; set; }

        /// <summary>
        /// Gets or sets version of the configuration file.
        /// </summary>
        public ushort Version { get; set; }

        /// <summary>
        /// Gets or sets list of all master servers.
        /// </summary>
        public List<NodeConfigFileModel_MasterServers> Masters { get; set; }

        /// <summary>
        /// Gets or sets list for all collections.
        /// </summary>
        public List<NodeConfigFileModel_Collections> Collections { get; set; }

        /// <summary>
        /// Gets or sets list of all storage pools.
        /// </summary>
        public List<StoragePoolNodeConfigPartModel> StoragePools { get; set; }
    }
}