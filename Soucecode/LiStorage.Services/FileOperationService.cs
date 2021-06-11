using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace LiStorage.Services
{
    public class FileOperationService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private string zzDebug { get; set; }

        private readonly ILogger<FileOperationService> _logger;

        private readonly RundataService _rundata;

        public FileOperationService(RundataService rundataService, ILogger<FileOperationService> logger)
        {
            this._logger = logger;

            this._rundata = rundataService;
            this.zzDebug = "FileOperationService";
        }
        public string GetFolderFromFilePath(string file)
        {
            if (string.IsNullOrEmpty(file))
                return "";

            return Path.GetDirectoryName(file);
        }
        
        
        public string LocateFileInKnownLocations(string configfileName)
        {
            // string configfileName = "LiStorageNode.conf";

            #region Locate in run folder
         
            var tmpfile = new FileInfo(Path.Combine(Environment.CurrentDirectory, configfileName));

            if (tmpfile.Exists)
            {
                this.zzDebug = "dsfdsf";
                return tmpfile.FullName;
            }

            #endregion

            #region Locate in Appdata local folder

            tmpfile = new FileInfo(Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "LiStorage", configfileName));

            if (tmpfile.Exists)
            {
                this.zzDebug = "dsfdsf";
                return tmpfile.FullName;
            }

            #endregion

            this.zzDebug = "dsfdsf";

            if (Debugger.IsAttached)
                Debugger.Break();


            return "";

            #region Testing code

            //internal static readonly FileInfo Mp4WithAudio = new FileInfo(Path.Combine(Environment.CurrentDirectory, "Resources", "input.mp4"));
            // var tmpfile = new FileInfo(Path.Combine(Environment.CurrentDirectory, configfileName));
            //var hh2 = new FileInfo(Path.Combine(Environment.SpecialFolder.LocalApplicationData.ToString(), "LiStorage", configfileName));
            //  Check appdata local folder
            //var hh3 = Environment.GetEnvironmentVariable("LocalAppData");

            #endregion

        }
    }
}
