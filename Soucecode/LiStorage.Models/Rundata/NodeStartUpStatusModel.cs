using System;
using System.Collections.Generic;
using System.Text;

namespace LiStorage.Models.Rundata
{
    public class NodeStartUpStatusModel
    {
        public NodeStartUpStatusEnum ConfigFileExist { get; set; }
        public NodeStartUpStatusEnum ConfigFileRead { get; set; }
        public NodeStartUpStatusEnum ConfigFileWorking { get; set; }

        public NodeStartUpStatusModel()
        {
            this.ConfigFileExist = NodeStartUpStatusEnum.Notstarted;
            this.ConfigFileRead = NodeStartUpStatusEnum.Notstarted;
            this.ConfigFileWorking = NodeStartUpStatusEnum.Notstarted;
        }
    }

    public enum NodeStartUpStatusEnum { Notstarted = 0, Running = 1, Error = 2, Done = 3 }
}
