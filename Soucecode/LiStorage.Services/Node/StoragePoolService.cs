using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiStorage.Services.Node
{
    public class StoragePoolService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private string zzDebug { get; set; }

        private readonly ILogger<StoragePoolService> _logger;

        private readonly RundataService _rundata;
        private readonly RundataNodeService _node;
        private readonly FileOperationService _fileOperation;

        public StoragePoolService(ILogger<StoragePoolService> logger, RundataService rundataService, FileOperationService fileOperation, RundataNodeService rundataNode)
        {
            this.zzDebug = "NodeWorker";

            _logger = logger;
           
            this._rundata = rundataService;
            this._node = rundataNode;
            this._fileOperation = fileOperation;


        }
    }
}
