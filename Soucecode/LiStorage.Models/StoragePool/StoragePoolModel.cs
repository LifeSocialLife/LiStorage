// <summary>
// Node storage pool model
// </summary>
// <copyright file="StoragePoolModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Models.StoragePool
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    /// <summary>
    /// Storage Pool configuration running on Nodes.
    /// </summary>
    public class StoragePoolModel
    {
        /// <summary>
        /// Gets or sets configuration storage pool saved in node config file.
        /// </summary>
        public StoragePoolNodeConfigPartModel Filedata { get; set; }

        /// <summary>
        /// Gets or sets configuration saved in storage pool own config file.
        /// </summary>
        public StoragePoolConfigFileModel ConfigData { get; set; }

        /// <summary>
        /// Gets or sets when where status (check) last run on this storage pool
        /// </summary>
        public DateTime DtLastCheck { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is init done on this storage pool.
        /// </summary>
        public bool InitDone { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is this storage pool working?.
        /// </summary>
        // public bool IsWorking { get; set; }

        /// <summary>
        /// Gets or sets. what is the status of this storage pool
        /// </summary>
        public StoragePoolStatusEnum Status { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoragePoolModel"/> class.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Reviewed.")]
        public StoragePoolModel()
        {
            this.InitDone = false;

            // this.IsWorking = false;
            this.Status = StoragePoolStatusEnum.Nodata;
            this.DtLastCheck = Convert.ToDateTime("2000-01-01 00:00:00");

            this.Filedata = new StoragePoolNodeConfigPartModel();
            this.ConfigData = new StoragePoolConfigFileModel();
        }
    }
}
