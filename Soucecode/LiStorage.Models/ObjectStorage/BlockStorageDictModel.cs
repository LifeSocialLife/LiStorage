// <copyright file="BlockStorageDictModel.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>

namespace LiStorage.Models.ObjectStorage
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// BlockStorageDictModel.
    /// </summary>
    public class BlockStorageDictModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether do this block exist in this node.
        /// </summary>
        public bool Exists { get; set; }
        public string FileMetaPath { get; set; }
        public string FileMetaPathWhitStoragePath { get; set; }
        public bool FileMetaExist { get; set; }
        public BlockStorageDictModel()
        {
            this.Exists = false;
            this.FileMetaExist = false;
            this.FileMetaPath = string.Empty;
            this.FileMetaPathWhitStoragePath = string.Empty;

            //this.FilePathMeta = string.Empty;
        }
    }

    public class BlockFileMetaModel
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string StoragePoolData { get; set; }

    }
}
