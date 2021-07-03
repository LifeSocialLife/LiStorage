// <summary>
// Background worker for liLog Server.
// </summary>
// <copyright file="BgWorkerService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace LiLog.Server.Data
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// LiLog server background woker.
    /// </summary>
    public class BgWorkerService : BackgroundService
    {
#pragma warning disable SA1309 // FieldNamesMustNotBeginWithUnderscore
        private readonly ILogger<BgWorkerService> _logger;
        private readonly TcpServerService _tcpServer;
        // private readonly RundataService _rundata;
        private readonly IConfiguration _configuration;
#pragma warning restore SA1309 // FieldNamesMustNotBeginWithUnderscore

        /// <summary>
        /// Initializes a new instance of the <see cref="BgWorkerService"/> class.
        /// </summary>
        /// <param name="logger">ILogger.</param>
        /// <param name="configuration">IConfiguration.</param>
        public BgWorkerService(ILogger<BgWorkerService> logger, IConfiguration configuration, TcpServerService tcpServer)
        {
            this.zzDebug = "Worker";

            this._logger = logger;
            this._configuration = configuration;
            this._tcpServer = tcpServer;

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

            this.zzDebug = "dfdsf";

            this._tcpServer.StartServer();


            this.zzDebug = "dfdsf";

            while (!stoppingToken.IsCancellationRequested)
            {
                // this._logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
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
