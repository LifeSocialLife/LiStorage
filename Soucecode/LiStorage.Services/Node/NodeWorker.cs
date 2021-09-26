// <summary>
// {one line to give the library's name and an idea of what it does.}
// </summary>
// <copyright file="NodeWorker.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Services.Node
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using LiStorage.Models.StoragePool;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Nodeworker that is running all backend work for the storage node.
    /// </summary>
    public class NodeWorker : BackgroundService
    {




        /// <inheritdoc/>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1123:DoNotPlaceRegionsWithinElements", Justification = "Reviewed.")]
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {



            /*


            this.zzDebug = "sdfdf";

            // Get configuration file. if the configfile exist in system
            if (this._node.StartUpStatus.ConfigFileExist == Models.Rundata.NodeStartUpStatusEnum.Done)
            {
                this.ReadConfigFile();
            }
            else
            {
                this._node.StartUpStatus.ConfigFileExist = Models.Rundata.NodeStartUpStatusEnum.Error;
            }

            // var ddd = this._rundata;
            this.zzDebug = "sdfdf";

            while (!stoppingToken.IsCancellationRequested)
            {
                // Check storage pools.
                this._storagepool.CheckStoragePools();


                //this._fileStorage.CheckCollections();

                // Start watson webserver
                // var dd = this._node.Collections;
                this.zzDebug = "Sfsdf";

                if (!this._httpserver.IsRunning)
                {
                    this._httpserver.StartWebserver();
                }

                // _logger.LogInformation($"Background service running :: {stoppingToken.IsCancellationRequested}");
                await Task.Delay(2000, stoppingToken);
            }

            this.zzDebug = "sdfdsf";

            */
        }

        

       
    }
}
