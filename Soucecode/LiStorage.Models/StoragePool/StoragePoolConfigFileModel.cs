// <summary>
// Model StoragePoolConfigFileModel.
// </summary>
// <copyright file="StoragePoolConfigFileModel.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Models.StoragePool
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Storage Pool Configuration File Model.
    /// </summary>
    public class StoragePoolConfigFileModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoragePoolConfigFileModel"/> class.
        /// </summary>
        public StoragePoolConfigFileModel()
        {
            this.Id = string.Empty;
            this.Version = 0;
            this.StorageType = StoragePoolTypesEnum.None;
            this.AllowData = false;
            this.AllowMeta = false;
            this.Compress = new StoragePoolConfigFileModelCompress();
            this.Encryption = new StoragePoolConfigFileModelEncryption();
        }

        /// <summary>
        /// Gets or sets id of this storage pool.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets version this configuration file is saved in.
        /// </summary>
        public ushort Version { get; set; }

        /// <summary>
        /// Gets or sets type of storage type this is. singel, raid0 to 10.
        /// </summary>
        public StoragePoolTypesEnum StorageType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether do we allow data to be saved in this storage pool.
        /// </summary>
        public bool AllowData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether do we allow meta to be saved in this storage pool.
        /// </summary>
        public bool AllowMeta { get; set; }

        /// <summary>
        /// Gets or sets compress model for this storage pool.
        /// </summary>
        public StoragePoolConfigFileModelCompress Compress { get; set; }

        /// <summary>
        /// Gets or sets encryption for this storage pool.
        /// </summary>
        public StoragePoolConfigFileModelEncryption Encryption { get; set; }
    }
}
