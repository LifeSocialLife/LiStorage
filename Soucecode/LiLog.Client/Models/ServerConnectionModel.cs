// <copyright file="ServerConnectionModel.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>

namespace LiLog.Client.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Model for setting to connect to LiLog Server.
    /// </summary>
    public class ServerConnectionModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerConnectionModel"/> class.
        /// </summary>
        public ServerConnectionModel()
        {
            this.ServerIp = "0.0.0.0";
            this.ServerPort = 9000;
            this.ApplicationId = string.Empty;
            this.ApplicationInstance = string.Empty;
            this.UseEncryption = false;
            this.ServerSecret = string.Empty;
        }

        /// <summary>
        /// Gets or sets ip adress of LiLog Server.
        /// </summary>
        public string ServerIp { get; set; }

        /// <summary>
        /// Gets or sets port of the LiLog Server.
        /// </summary>
        public int ServerPort { get; set; }

        /// <summary>
        /// Gets or sets name of this application.
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets application instance name, this can be null.
        /// </summary>
        public string? ApplicationInstance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether shod traffic be encrypted between Client And server?.
        /// </summary>
        public bool UseEncryption { get; set; }

        /// <summary>
        /// Gets or sets iF server need connection secret to allow to connect. set the secret here.
        /// </summary>
        public string ServerSecret { get; set; }
    }
}
