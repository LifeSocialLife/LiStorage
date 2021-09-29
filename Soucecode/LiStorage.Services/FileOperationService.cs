// <summary>
// File Operaton Service.
// </summary>
// <copyright file="FileOperationService.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LiStorage.Helpers;
    using LiStorage.Models.StoragePool;
    using LiStorage.Services.Classes;
    using LiStorage.Services.Node;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// File Operaton Service.
    /// </summary>
    public class FileOperationService
    {
        private readonly ILogger<FileOperationService> _logger;
        private readonly RundataService _rundata;
        private readonly RundataNodeService _node;
        private readonly CollectionPoolService _collections;

        // private readonly StoragePoolService _storagepool;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileOperationService"/> class.
        /// </summary>
        /// <param name="rundataService">RundataService.</param>
        /// <param name="logger">ILogger.</param>
        /// <param name="rundataNode">RundataNodeService.</param>
        /// <param name="collectionService">CollectionService.</param>
        /// <param name="storagePoolService">StoragePoolService.</param>
        public FileOperationService(RundataService rundataService, ILogger<FileOperationService> logger, RundataNodeService rundataNode, CollectionPoolService collectionService)
        {
            this._logger = logger;

            this._rundata = rundataService;
            this._node = rundataNode;
            this._collections = collectionService;
            this.zzDebug = "FileOperationService";
        }

        [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Reviewed.")]
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        private string zzDebug { get; set; }

        /// <summary>
        /// Locate file in known location on computer.
        /// </summary>
        /// <param name="configfileName">Filename to locate.</param>
        /// <returns>Path as string to file.</returns>
        public string LocateFileInKnownLocations(string configfileName)
        {
            /*
             * string configfileName = "LiStorageNode.conf";
            */

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
            {
                Debugger.Break();
            }

            return string.Empty;

            #region Testing code

            /*
            //internal static readonly FileInfo Mp4WithAudio = new FileInfo(Path.Combine(Environment.CurrentDirectory, "Resources", "input.mp4"));
            // var tmpfile = new FileInfo(Path.Combine(Environment.CurrentDirectory, configfileName));
            //var hh2 = new FileInfo(Path.Combine(Environment.SpecialFolder.LocalApplicationData.ToString(), "LiStorage", configfileName));
            //  Check appdata local folder
            //var hh3 = Environment.GetEnvironmentVariable("LocalAppData");
            */

            #endregion
        }
    }
}
