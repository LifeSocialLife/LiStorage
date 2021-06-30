// <summary>
// Storage pool work.
// </summary>
// <copyright file="RundataService.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Rundata service.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1512:Single-line comments should not be followed by blank line", Justification = "Reviewed.")]
    public class RundataService
    {

        public RundataServiceFoldersModel Folders { get; set; }

        public RundataServiceHardwareModel Hardware { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        private string zzDebug { get; set; }


        // private IConfiguration _Configuration { get; }
        // private readonly FileOperationService _fileOperation;

        /// <summary>
        /// Initializes a new instance of the <see cref="RundataService"/> class.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Reviewed.")]
        public RundataService() // FileOperationService fileOperation)
        {
            // this._fileOperation = fileOperation;

            this.Folders = new RundataServiceFoldersModel();
            this.Hardware = new RundataServiceHardwareModel();

            this.GetPlatformInformation();

            this.zzDebug = "RundataService";

        }

        private void GetPlatformInformation()
        {
            //this.Folders.PathRuntimes = this._Configuration.GetValue<string>(WebHostDefaults.ContentRootKey);

            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                this.Hardware.Platform = PlatformEnum.Windows;
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
            {
                this.Hardware.Platform = PlatformEnum.Linux;
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotImplementedException();
            }


            this.Folders.PathExecuteFile = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;


            this.Hardware.OsPlatform = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            this.Hardware.FrameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;


            switch (System.Runtime.InteropServices.RuntimeInformation.OSArchitecture)
            {
                case System.Runtime.InteropServices.Architecture.X64:
                    this.Hardware.OSArchitecture = ArchitectureEnum.X64;
                    break;
                default:
                    throw new NotImplementedException();
                    //break;
            }

            switch (System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture)
            {
                case System.Runtime.InteropServices.Architecture.X64:
                    this.Hardware.ProcesArchitecture = ArchitectureEnum.X64;
                    break;
                default:
                    throw new NotImplementedException();
                    //break;
            }

            //  Get ExecutePath
            this.Folders.PathExecute = Path.GetDirectoryName(this.Folders.PathExecuteFile);

            this.zzDebug = "sdfdsf";

        }
    }

    public class RundataServiceFoldersModel
    {
        public string PathRuntimes { get; set; }
        public string PathExecuteFile { get; set; }
        public string PathExecute { get; set; }
        public string ConfigFile { get; set; }


        public RundataServiceFoldersModel()
        {
            this.PathRuntimes = "";
            this.PathExecuteFile = "";
            this.PathExecute = "";
            this.ConfigFile = "";
        }

        public Dictionary<string, string> GetAllData()
        {
            var aa = new Dictionary<string, string>();
            aa.Add("PathRuntimes", this.PathRuntimes);
            aa.Add("PathExecuteFile", this.PathExecuteFile);
            aa.Add("PathExecute", this.PathExecute);
            aa.Add("ConfigFile", this.ConfigFile);
            return aa;

        }
    }

    public class RundataServiceHardwareModel
    {
        public string OsPlatform { get; set; }
        public string FrameworkDescription { get; set; }
        public ArchitectureEnum OSArchitecture { get; set; }
        public ArchitectureEnum ProcesArchitecture { get; set; }
        public PlatformEnum Platform { get; set; }
        public RundataServiceHardwareModel()
        {
            this.OsPlatform = "";
            this.FrameworkDescription = "";
            this.OSArchitecture = ArchitectureEnum.none;
            this.ProcesArchitecture = ArchitectureEnum.none;
            this.Platform = PlatformEnum.None;
        }
        public Dictionary<string,string> GetAllData()
        {
            var aa = new Dictionary<string, string>();
            aa.Add("OsPlatform", this.OsPlatform);
            aa.Add("FrameworkDescription", this.FrameworkDescription);
            aa.Add("OSArchitecture", this.OSArchitecture.ToString());
            aa.Add("ProcesArchitecture", this.ProcesArchitecture.ToString());
            aa.Add("Platform", this.Platform.ToString());
            return aa;

        }
    }
    public enum ArchitectureEnum { none, X86, X64, Arm, Arm64 }
    public enum PlatformEnum {  None, Windows, Linux, Osx}

}


/*
 * 
 *  
 * 
 * */