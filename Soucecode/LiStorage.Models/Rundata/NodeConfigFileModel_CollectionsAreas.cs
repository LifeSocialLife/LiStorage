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
    public class NodeConfigFileModel_CollectionsAreas
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StoragePoolMetaDefault { get; set; }
        public string StoragePoolDataDefault { get; set; }
        public bool Enabled { get; set; }

        public NodeConfigFileModel_CollectionsAreas()
        {
            this.Id = "";
            this.Name = "";
            this.StoragePoolMetaDefault = "";
            this.StoragePoolDataDefault = "";
            this.Enabled = false;
        }
    }
}
