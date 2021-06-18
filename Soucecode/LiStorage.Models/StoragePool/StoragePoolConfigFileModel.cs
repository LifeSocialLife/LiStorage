using System;
using System.Collections.Generic;
using System.Text;

namespace LiStorage.Models.StoragePool
{
    public class StoragePoolConfigFileModel
    {
        public string Id { get; set; }
        public UInt16 Version { get; set; }
        public StoragePoolTypesEnum StorageType { get; set; }
        public bool AllowData { get; set; }
        public bool AllowMeta { get; set; }
        public StoragePoolConfigFileModelCompress Compress { get; set; }
        public StoragePoolConfigFileModelEncryption Encryption { get; set; }

        public StoragePoolConfigFileModel()
        {
            this.Id = "";
            this.Version = 0;
            this.StorageType = StoragePoolTypesEnum.None;
            this.AllowData = false;
            this.AllowMeta = false;
            this.Compress = new StoragePoolConfigFileModelCompress();
            this.Encryption = new StoragePoolConfigFileModelEncryption();
        }
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
