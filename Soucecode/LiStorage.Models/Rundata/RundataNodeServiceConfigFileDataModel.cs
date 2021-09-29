// <summary>
// Model RundataNodeServiceConfigFileDataModel.
// </summary>
// <copyright file="RundataNodeServiceConfigFileDataModel.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Models.Rundata
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Model for Node config file.
    /// </summary>
    public class RundataNodeServiceConfigFileDataModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RundataNodeServiceConfigFileDataModel"/> class.
        /// </summary>
        public RundataNodeServiceConfigFileDataModel()
        {
            this.NodeName = string.Empty;
            this.ClusterKey = string.Empty;
            this.HeaderApiKey = string.Empty;
            this.Version = 0;
            this.NeedToBeSaved = false;
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
        /// Gets or sets a value indicating whether do this need to be saved?.
        /// </summary>
        public bool NeedToBeSaved { get; set; }
    }
}
