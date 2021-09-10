// <copyright file="FileOperationService.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>

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
    using LiStorage.Services.Classes;
    using Microsoft.Extensions.Logging;

    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1124:DoNotUseRegions", Justification = "Reviewed.")]

    public class FileOperationService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private string zzDebug { get; set; }

        private readonly ILogger<FileOperationService> _logger;

        private readonly RundataService _rundata;
        private readonly RundataNodeService _node;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileOperationService"/> class.
        /// </summary>
        /// <param name="rundataService"></param>
        /// <param name="logger"></param>
        /// <param name="rundataNode"></param>
        public FileOperationService(RundataService rundataService, ILogger<FileOperationService> logger, RundataNodeService rundataNode)
        {
            this._logger = logger;

            this._rundata = rundataService;
            this._node = rundataNode;
            this.zzDebug = "FileOperationService";
        }

       

        #region Move All this to helper classes



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

        #region File

      

        public List<string> GetFileList(string environment, string directory, bool prependFilename)
        {
            try
            {
                /*
                 * 
                 * Returns only the filename unless prepend_filename is set
                 * If prepend_filename is set, directory is prepended
                 * 
                 */

                DirectoryInfo info = new DirectoryInfo(directory);
                FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
                List<string> fileList = new List<string>();

                foreach (FileInfo file in files)
                {
                    if (prependFilename) fileList.Add(directory + "/" + file.Name);
                    else fileList.Add(file.Name);
                }

                return fileList;
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        

        

       


        

        public string GetFileExtension(string filename)
        {
            try
            {
                if (String.IsNullOrEmpty(filename)) return "";
                return Path.GetExtension(filename);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public bool RenameFile(string from, string to)
        {
            try
            {
                if (String.IsNullOrEmpty(from)) return false;
                if (String.IsNullOrEmpty(to)) return false;

                if (String.Compare(from, to) == 0) return true;
                File.Move(from, to);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool MoveFile(string from, string to)
        {
            try
            {
                if (String.IsNullOrEmpty(from)) return false;
                if (String.IsNullOrEmpty(to)) return false;

                if (String.Compare(from, to) == 0) return true;
                File.Move(from, to);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion


        #region Directory 


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
