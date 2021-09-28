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
