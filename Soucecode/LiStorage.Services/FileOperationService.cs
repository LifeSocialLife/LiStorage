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
            {
                Debugger.Break();
            }

            return String.Empty;

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

        #region Move All this to helper classes

        #region Directory - Moved to Helpers.IO.Directory

        //public List<string> GetSubdirectoryList(string directory, bool recursive)
        //{
        //    try
        //    {
        //        string[] folders;

        //        if (recursive)
        //        {
        //            folders = Directory.GetDirectories(@directory, "*", SearchOption.AllDirectories);
        //        }
        //        else
        //        {
        //            folders = Directory.GetDirectories(@directory, "*", SearchOption.TopDirectoryOnly);
        //        }

        //        List<string> folderList = new List<string>();

        //        foreach (string folder in folders)
        //        {
        //            folderList.Add(folder);
        //        }

        //        return folderList;
        //    }
        //    catch (Exception)
        //    {
        //        return new List<string>();
        //    }
        //}

        //public bool DeleteDirectory(string dir, bool recursive)
        //{
        //    try
        //    {
        //        Directory.Delete(dir, recursive);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        //public bool RenameDirectory(string from, string to)
        //{
        //    try
        //    {
        //        if (String.IsNullOrEmpty(from)) return false;
        //        if (String.IsNullOrEmpty(to)) return false;
        //        if (String.Compare(from, to) == 0) return true;
        //        Directory.Move(from, to);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        //public bool MoveDirectory(string from, string to)
        //{
        //    try
        //    {
        //        if (String.IsNullOrEmpty(from)) return false;
        //        if (String.IsNullOrEmpty(to)) return false;
        //        if (String.Compare(from, to) == 0) return true;
        //        Directory.Move(from, to);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        //internal bool DirectoryExist(string dir)
        //{

        //    try
        //    {
        //        if (System.IO.Directory.Exists(dir))
        //            return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }

        //    return false;
        //}

        //internal bool DirectoryCreate(string folder)
        //{
        //    try
        //    {
        //        if (!this.DirectoryExist(folder))
        //            System.IO.Directory.CreateDirectory(folder);

        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }

        //}

        ///// <summary>
        ///// Get Size of directory.
        ///// </summary>
        ///// <param name="dir">Path to scan.</param>
        ///// <param name="followChildren">Get space of children.</param>
        ///// <param name="returnAs">Return as value.</param>
        ///// <returns>Size of folder.</returns>
        //internal ulong DirectoryGetSize(string dir, bool followChildren, LiTools.Helpers.Convert.FileSizeEnums returnAs)
        //{
        //    if (string.IsNullOrEmpty(dir))
        //    {
        //        return 0;
        //    }

        //    if (!this.DirectoryExist(dir))
        //    {
        //        return 0;
        //    }

        //    DirectoryInfo info = new DirectoryInfo(dir);

        //    ulong totalSize = this.DirectorySize(info, followChildren);

        //    if (returnAs == LiTools.Helpers.Convert.FileSizeEnums.Bytes)
        //    {
        //        return (ulong)totalSize;
        //    }

        //    var aa = LiTools.Helpers.Convert.Bytes.To(returnAs, (long)totalSize);
        //    this.zzDebug = "sdfdsf";

        //    return (ulong)aa;

        //    // return Convert.ToInt64(aa);
        //}

        //private ulong DirectorySize(DirectoryInfo dir, bool followChildren)
        //{
        //    ulong totalSize = (ulong)dir.GetFiles().Sum(fi => fi.Length);

        //    if (followChildren)
        //    {
        //        var dd = dir.GetDirectories();
        //        foreach (var hej in dd)
        //        {
        //            totalSize += this.DirectorySize(hej, followChildren);
        //        }

        //        this.zzDebug = "sdfdf";

        //        // totalSize += dir.GetDirectories().Sum(di => this.DirectorySize(di, followChildren));
        //    }

        //    return totalSize;

        //    // return dir.GetFiles().Sum(fi => fi.Length) +
        //    //       dir.GetDirectories().Sum(di => DirectorySize(di, followChildren));
        //}

        #endregion


        #endregion

    }
}
