// <summary>
// Model NodeConfigFileModel_MasterServers.
// </summary>
// <copyright file="NodeConfigFileModel_MasterServers.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Models.Rundata
{
    using System;

    /// <summary>
    /// Model for all Master servers in node config file.
    /// </summary>
    public class NodeConfigFileModel_MasterServers
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeConfigFileModel_MasterServers"/> class.
        /// </summary>
        public NodeConfigFileModel_MasterServers()
        {
            this.Name = string.Empty;
            this.Ip = string.Empty;
            this.Port = 30511;
            this.Prio = 5;
        }

        /// <summary>
        /// Gets or sets name of the master server.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets ip adress to master server.
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// Gets or sets port of the master server.
        /// </summary>
        public short Port { get; set; }

        /// <summary>
        /// Gets or sets prio of this master server.
        /// </summary>
        public ushort Prio { get; set; }
    }
}
