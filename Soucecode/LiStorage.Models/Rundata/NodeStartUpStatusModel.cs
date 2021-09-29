// <summary>
// Main handler for all background work, node and master.
// </summary>
// <copyright file="NodeStartUpStatusModel.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>

namespace LiStorage.Models.Rundata
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Node statup status set types.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Reviewed.")]
    public enum NodeStartUpStatusEnum
    {
        Notstarted = 0,
        Running = 1,
        Error = 2,
        Done = 3,
    }

    /// <summary>
    /// Node startup status model.
    /// </summary>
    public class NodeStartUpStatusModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeStartUpStatusModel"/> class.
        /// </summary>
        public NodeStartUpStatusModel()
        {
            this.ConfigFile = NodeStartUpStatusEnum.Notstarted;
            this.ConfigFileExist = false;
            this.ConfigFileLastError = string.Empty;
            /*
            //this.ConfigFileExist = NodeStartUpStatusEnum.Notstarted;
            //this.ConfigFileRead = NodeStartUpStatusEnum.Notstarted;
            //this.ConfigFileWorking = NodeStartUpStatusEnum.Notstarted;
            */
        }

        /// <summary>
        /// Gets or sets configfile status. see NodeStartUpStatusEnum  ex: Notstarted = 0, Running = 1, Error = 2, Done = 3.
        /// </summary>
        public NodeStartUpStatusEnum ConfigFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether configfile exist in node.
        /// </summary>
        public bool ConfigFileExist { get; set; }

        /// <summary>
        /// Gets or sets if error exist. what was the last error.
        /// </summary>
        public string ConfigFileLastError { get; set; }

        /*
        //public NodeStartUpStatusEnum ConfigFileRead { get; set; }
        //public NodeStartUpStatusEnum ConfigFileWorking { get; set; }
        */
    }
}
