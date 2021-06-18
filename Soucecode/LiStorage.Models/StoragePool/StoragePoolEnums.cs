using System;
using System.Collections.Generic;
using System.Text;

namespace LiStorage.Models.StoragePool
{

    public enum StoragePoolStatusEnum { Nodata, Init, FileMissing, Error, Warning, Working }

    public enum StoragePoolTypesEnum
    {
        None = 0,
        Singel = 1,
        Raid0 = 2,
        Raid1 = 3,
        Raid5 = 4,
        Raid6 = 5,
        Raid10 = 6
    }

}
