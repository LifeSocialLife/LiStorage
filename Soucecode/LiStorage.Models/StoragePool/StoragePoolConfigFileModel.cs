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

        public string Id { get; set; }
        public ushort Version { get; set; }

        /// <summary>
        /// Gets or sets type of storage type this is. singel, raid0 to 10.
        /// </summary>
        public StoragePoolTypesEnum StorageType { get; set; }
        public bool AllowData { get; set; }
        public bool AllowMeta { get; set; }
        public StoragePoolConfigFileModelCompress Compress { get; set; }
        public StoragePoolConfigFileModelEncryption Encryption { get; set; }

        
    }
    public class StoragePoolConfigFileModelCompress
    {
        public bool CompressAllowd { get; set; }
        public bool CompressAllData { get; set; }
        public bool CompressAllMeta { get; set; }
        public StoragePoolConfigFileModelCompress()
        {
            this.CompressAllData = false;
            this.CompressAllData = false;
            this.CompressAllMeta = false;
        }
    }
    public class StoragePoolConfigFileModelEncryption
    {
        public bool EncryptionAllowd { get; set; }
        public bool EncryptAllData { get; set; }
        public bool EncryptAllMeta { get; set; }
        public Dictionary<string, string> Keys { get; set; }

        public StoragePoolConfigFileModelEncryption()
        {
            this.EncryptionAllowd = false;
            this.EncryptAllData = false;
            this.EncryptAllMeta = false;
            this.Keys = new Dictionary<string, string>();
        }
    }
}
