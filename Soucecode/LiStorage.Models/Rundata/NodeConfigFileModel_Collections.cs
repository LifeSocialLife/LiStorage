// <summary>
// Model NodeConfigFileModel_Collections.
// </summary>
// <copyright file="NodeConfigFileModel_Collections.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Models.Rundata
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Model for all collections in node config file.
    /// </summary>
    public class NodeConfigFileModel_Collections : NodeConfigFileModel_CollectionsAreas
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeConfigFileModel_Collections"/> class.
        /// </summary>
        public NodeConfigFileModel_Collections()
        {
            this.Id = string.Empty;
            this.Name = string.Empty;
            this.Enabled = false;
            this.StoragePoolMetaDefault = string.Empty;
            this.StoragePoolDataDefault = string.Empty;
            this.Areas = new List<NodeConfigFileModel_CollectionsAreas>();
        }

        /// <summary>
        /// Gets or sets list of all areas inside collections.
        /// </summary>
        public List<NodeConfigFileModel_CollectionsAreas> Areas { get; set; }
    }
}
