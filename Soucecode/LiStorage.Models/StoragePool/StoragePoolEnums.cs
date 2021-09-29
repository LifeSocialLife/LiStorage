// <summary>
// Enums for Storage pools
// </summary>
// <copyright file="StoragePoolEnums.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Models.StoragePool
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Status of the storage pools.
    /// </summary>
    public enum StoragePoolStatusEnum
    {
        /// <summary>Storage pool has not status.</summary>
        Nodata = 0,

        /// <summary>Storage pool is in init stage.</summary>
        Init = 1,

        /// <summary>Storage pool has filemissing status.</summary>
        FileMissing = 2,

        /// <summary>Storage pool is in error mode.</summary>
        Error = 3,

        /// <summary>Storage pool is in warning mode.</summary>
        Warning = 4,

        /// <summary>Storage pool is Working.</summary>
        Working = 5,
    }

    /// <summary>
    /// Types of storage pools that exist in LiStorage software.
    /// </summary>
    public enum StoragePoolTypesEnum
    {
        /// <summary>Storage pool type not set.</summary>
        None = 0,

        /// <summary>Singel disk storage pool.</summary>
        Singel = 1,

        /// <summary>Raid 0 Disk storage (Stripe set).</summary>
        Raid0 = 2,

        /// <summary>Raid 1 Disk storage (Mirror set).</summary>
        Raid1 = 3,

        /// <summary>Raid 5 Disk storage (block-level striping with distributed parity - 1 parity disks).</summary>
        Raid5 = 4,

        /// <summary>Raid 6 Disk storage (block-level striping with distributed parity - 2 parity disks).</summary>
        Raid6 = 5,

        /// <summary>Raid 10 Disk storage (Disk mirror whit disk Stripe set).</summary
        Raid10 = 6,
    }
}