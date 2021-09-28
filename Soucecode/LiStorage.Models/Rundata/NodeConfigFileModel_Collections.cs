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

    public class NodeConfigFileModel_Collections : NodeConfigFileModel_CollectionsAreas
    {
        public NodeConfigFileModel_Collections()
        {
            this.Id = "";
            this.Name = "";
            this.Enabled = false;
            this.StoragePoolMetaDefault = "";
            this.StoragePoolDataDefault = "";
            this.Areas = new List<NodeConfigFileModel_CollectionsAreas>();
        }

        public List<NodeConfigFileModel_CollectionsAreas> Areas { get; set; }
    }
}
