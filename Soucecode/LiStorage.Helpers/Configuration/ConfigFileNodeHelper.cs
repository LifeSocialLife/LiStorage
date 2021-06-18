using LiStorage.Models.Rundata;
using LiStorage.Models.StoragePool;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LiStorage.Helpers.Configuration
{
    public static class ConfigFileNodeHelper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private static string? zzDebug { get; set; }


        public static NodeConfigFileModel CreateTemplate()
        {
            var aa = new NodeConfigFileModel()
            {
                NodeName = "demo",
                ClusterKey = "megahemligsecretfile9876543211234567890abcde",
                Version = 1,
                Masters = new List<NodeConfigFileModel_MasterServers>()
                {
                    new NodeConfigFileModel_MasterServers()
                    {
                        Name = "demo",
                        Ip = "127.0.0.1",
                        Port = 30511,
                        Prio = 5,
                    },
                },
                Collections = new List<NodeConfigFileModel_Collections>() 
                {
                    new NodeConfigFileModel_Collections()
                    {
                        Id = "demo",
                        Enabled = false,
                        Name = "demo",
                        StoragePoolDataDefault = "demo",
                        StoragePoolMetaDefault = "demo",
                        Areas = new List<NodeConfigFileModel_CollectionsAreas>()
                        {
                            new NodeConfigFileModel_CollectionsAreas()
                            {
                                Id = "demo",
                                StoragePoolMetaDefault = "demo",
                                StoragePoolDataDefault = "demo",
                                Name = "demo",
                                Enabled = false,
                            },
                        },
                    },
                },
                StoragePools = new List<Models.StoragePool.StoragePoolNodeConfigPartModel>() 
                {
                    new Models.StoragePool.StoragePoolNodeConfigPartModel()
                    {
                        Enabled = false,
                        Name = "demo",
                        Id = "demo",
                        FolderPath = "demo",
                    },
                },
                
            };


            zzDebug = "edfsdf";

            return aa;
        }

        public static StoragePoolConfigFileModel NodeConfigStoragePoolConfigFile(int AddEncryptionKeysCount = 0, int EncryptionKeyLengt = 20)
        {
            var aa = new StoragePoolConfigFileModel()
            {
                AllowData = false,
                AllowMeta = false,
                Compress = new StoragePoolConfigFileModelCompress()
                {
                    CompressAllowd = true,
                },
                Encryption = new StoragePoolConfigFileModelEncryption()
                {
                    EncryptionAllowd = true,
                    Keys = new Dictionary<string, string>(),
                },
                Id = "demo",
                StorageType = StoragePoolTypesEnum.None,
                Version = 1,
            };

            if (AddEncryptionKeysCount > 0)
            {
                for (int i = 1; i < AddEncryptionKeysCount +1; i++)
                {
                    aa.Encryption.Keys.Add($"def{i}", LiStorage.Helpers.CommonHelper.RandomString(EncryptionKeyLengt));
                }
            }


            return aa;

        }
    }
}
