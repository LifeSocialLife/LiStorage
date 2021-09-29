// <summary>
// Model RundataNodeServiceMastersModel.
// </summary>
// <copyright file="RundataNodeServiceMastersModel.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Models.Rundata
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Model for RundataNodeServiceMastersModel.
    /// </summary>
    public class RundataNodeServiceMastersModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RundataNodeServiceMastersModel"/> class.
        /// </summary>
        public RundataNodeServiceMastersModel()
        {
            this.Filedata = new NodeConfigFileModel_MasterServers();
        }

        /// <summary>
        /// Gets or sets filedata - Information from configfile.
        /// </summary>
        public NodeConfigFileModel_MasterServers Filedata { get; set; }
    }
}