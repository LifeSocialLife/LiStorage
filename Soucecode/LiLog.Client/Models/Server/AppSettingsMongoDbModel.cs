// <copyright file="AppSettingsMongoDbModel.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>

namespace LiLog.Client.Models.Server
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// AppSettings.json configuration model for mongo db settings.
    /// </summary>
    public class AppSettingsMongoDbModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettingsMongoDbModel"/> class.
        /// </summary>
        public AppSettingsMongoDbModel()
        {
            this.MongoHost = string.Empty;
            this.DatabaseName = string.Empty;
            this.Username = string.Empty;
            this.Password = string.Empty;
        }

        /// <summary>
        /// Gets or sets mongo db host information.
        /// </summary>
        public string MongoHost { get; set; }

        /// <summary>
        /// Gets or sets mongo db databasename.
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets mongo db username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets mongo db password.
        /// </summary>
        public string Password { get; set; }
    }
}
