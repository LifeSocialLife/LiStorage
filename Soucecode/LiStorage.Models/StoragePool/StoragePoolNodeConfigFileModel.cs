using System;
using System.Collections.Generic;
using System.Text;

namespace LiStorage.Models.StoragePool
{
    public class StoragePoolNodeConfigFileModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public string FolderPath { get; set; }
        public StoragePoolNodeConfigFileModel()
        {
            this.Id = "";
            this.Name = "";
            this.Enabled = false;
            this.FolderPath = "";
        }

    }

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
