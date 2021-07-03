// <summary>
// Main handler for all background work, node and master.
// </summary>
// <copyright file="Worker.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>

namespace LiStorage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using LiStorage.Services;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Listorage background worker.
    /// </summary>
    public class Worker : BackgroundService
    {
#pragma warning disable SA1309 // FieldNamesMustNotBeginWithUnderscore
        private readonly ILogger<Worker> _logger;
        private readonly RundataService _rundata;
        private readonly IConfiguration _configuration;
#pragma warning restore SA1309 // FieldNamesMustNotBeginWithUnderscore

        /// <summary>
        /// Initializes a new instance of the <see cref="Worker"/> class.
        /// </summary>
        /// <param name="logger">ILogger.</param>
        /// <param name="configuration">IConfiguration.</param>
        /// <param name="rundataService">RundataService.</param>
        public Worker(ILogger<Worker> logger, IConfiguration configuration, RundataService rundataService)
        {
            this.zzDebug = "Worker";

            this._logger = logger;
            this._configuration = configuration;
            this._rundata = rundataService;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        private string zzDebug { get; set; }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var dd = WebHostDefaults.ContentRootKey;
            var d2 = this._configuration.GetValue<string>(WebHostDefaults.ContentRootKey);

            this._rundata.Folders.PathRuntimes = this._configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
            this.zzDebug = "dfdsf";


            while (!stoppingToken.IsCancellationRequested)
            {
                this._logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                try
                {
                    await Task.Delay(1000, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }
    }
}
