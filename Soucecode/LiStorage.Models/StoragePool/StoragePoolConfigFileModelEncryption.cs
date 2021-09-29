// <summary>
// Model StoragePoolConfigFileModelEncryption.
// </summary>
// <copyright file="StoragePoolConfigFileModelEncryption.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Models.StoragePool
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Model for encyption.
    /// </summary>
    public class StoragePoolConfigFileModelEncryption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoragePoolConfigFileModelEncryption"/> class.
        /// </summary>
        public StoragePoolConfigFileModelEncryption()
        {
            this.EncryptionAllowd = false;
            this.EncryptAllData = false;
            this.EncryptAllMeta = false;
            this.Keys = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether allow encryption.
        /// </summary>
        public bool EncryptionAllowd { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether encrypt all data.
        /// </summary>
        public bool EncryptAllData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether encrypt all meta.
        /// </summary>
        public bool EncryptAllMeta { get; set; }

        /// <summary>
        /// Gets or sets encryptions keys.
        /// </summary>
        public Dictionary<string, string> Keys { get; set; }
    }
}
