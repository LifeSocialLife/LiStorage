// <summary>
// Model NodeConfigFileModel_CollectionsAreas.
// </summary>
// <copyright file="NodeConfigFileModel_CollectionsAreas.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Models.Rundata
{
    using System;

    /// <summary>
    /// Model for all collections areas in node config file.
    /// </summary>
    public class NodeConfigFileModel_CollectionsAreas
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeConfigFileModel_CollectionsAreas"/> class.
        /// </summary>
        public NodeConfigFileModel_CollectionsAreas()
        {
            this.Id = string.Empty;
            this.Name = string.Empty;
            this.StoragePoolMetaDefault = string.Empty;
            this.StoragePoolDataDefault = string.Empty;
            this.Enabled = false;
        }

        /// <summary>
        /// Gets or sets id of this collections Area.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets name of this collection.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets default storage pool for Meta data.
        /// </summary>
        public string StoragePoolMetaDefault { get; set; }

        /// <summary>
        /// Gets or sets default storage pool for Data.
        /// </summary>
        public string StoragePoolDataDefault { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is this allow to be used.
        /// </summary>
        public bool Enabled { get; set; }
    }
}
