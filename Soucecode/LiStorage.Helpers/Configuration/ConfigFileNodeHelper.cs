using LiStorage.Models.Rundata;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
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
                StoragePools = new List<Models.StoragePool.StoragePoolNodeConfigFileModel>() 
                {
                    new Models.StoragePool.StoragePoolNodeConfigFileModel()
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
    }
}
