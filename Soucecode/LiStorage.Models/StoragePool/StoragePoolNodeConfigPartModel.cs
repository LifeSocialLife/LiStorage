using System;
using System.Collections.Generic;
using System.Text;

namespace LiStorage.Models.StoragePool
{
    public class StoragePoolNodeConfigPartModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public string FolderPath { get; set; }
        public StoragePoolNodeConfigPartModel()
        {
            this.Id = "";
            this.Name = "";
            this.Enabled = false;
            this.FolderPath = "";
        }

    }

    
}
