// <summary>
// LiStorageNode starter program.
// </summary>
// <copyright file="Program.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorageNode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Main starter for software.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main starter.
        /// </summary>
        /// <param name="args">string as arrays. staring syntax.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// CreateHostBuilder.
        /// </summary>
        /// <param name="args">string as arrays. staring syntax.</param>
        /// <returns>result of running software.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseUrls("http://0.0.0.0:6060", "https://0.0.0.0:6061");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
