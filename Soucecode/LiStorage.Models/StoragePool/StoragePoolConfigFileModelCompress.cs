// <summary>
// Model StoragePoolConfigFileModelCompress.
// </summary>
// <copyright file="StoragePoolConfigFileModelCompress.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Models.StoragePool
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Model for Compress of data.
    /// </summary>
    public class StoragePoolConfigFileModelCompress
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoragePoolConfigFileModelCompress"/> class.
        /// </summary>
        public StoragePoolConfigFileModelCompress()
        {
            this.CompressAllData = false;
            this.CompressAllData = false;
            this.CompressAllMeta = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether compression is allowd.
        /// </summary>
        public bool CompressAllowd { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether compress all data.
        /// </summary>
        public bool CompressAllData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether compress all Meta.
        /// </summary>
        public bool CompressAllMeta { get; set; }
    }
}
