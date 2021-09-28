// <summary>
// Model for collection dictionary of all collection that exist on this node.
// </summary>
// <copyright file="CollectionPoolModel.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Models.CollectionPool
{
    using System;
    using LiStorage.Models.Rundata;

    /// <summary>
    /// Collection Pool Model.
    /// </summary>
    public class CollectionPoolModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionPoolModel"/> class.
        /// </summary>
        public CollectionPoolModel()
        {
            this.Filedata = new NodeConfigFileModel_CollectionsAreas();
        }

        /// <summary>
        /// Gets or sets configuration collection pool saved in node config file.
        /// </summary>
        public NodeConfigFileModel_CollectionsAreas Filedata { get; set; }

        /// <summary>
        /// Gets or sets when where status (check) last run on this collection pool.
        /// </summary>
        public DateTime DtLastCheck { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is init done on this storage pool.
        /// </summary>
        public bool InitDone { get; set; }

    }
}
