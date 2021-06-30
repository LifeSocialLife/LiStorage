// <copyright file="NodeHttpService.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>

namespace LiStorage.Services.Node
{
    using LiStorage.Services.Classes;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using WatsonWebserver;

    /// <summary>
    /// Https webserver to handle web requests.
    /// </summary>
    public class NodeHttpService
    {
#pragma warning disable SA1309 // Field names should not begin with underscore

        private readonly ILogger<NodeHttpService> _logger;
        private readonly RundataService _rundata;
        private readonly RundataNodeService _node;
        private readonly FileOperationService _fileOperation;
        private readonly StoragePoolService _storagePool;
        private readonly BlockStorageService _objectStorage;

        private Server? _server;
        private ConnectionManager _connMgr;
        private string _header = "[LiStorageNode] ";

#pragma warning restore SA1309 // Field names should not begin with underscore

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        private string zzDebug { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeHttpService"/> class.
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="rundataService">RundataService</param>
        /// <param name="fileOperation">FileOperationService</param>
        /// <param name="rundataNode">RundataNodeService</param>
        /// <param name="storagePoolService">StoragePoolService</param>
        /// <param name="objectStorage">BlockStorageService</param>
        public NodeHttpService(ILogger<NodeHttpService> logger, RundataService rundataService, FileOperationService fileOperation, RundataNodeService rundataNode, StoragePoolService storagePoolService, BlockStorageService objectStorage)
        {
            this.zzDebug = "NodeHttpService";
            this._logger = logger;

            this._rundata = rundataService;
            this._node = rundataNode;
            this._fileOperation = fileOperation;
            this._storagePool = storagePoolService;
            this._objectStorage = objectStorage;
            this.IsRunning = false;

            this._connMgr = new ConnectionManager();
        }

        public bool IsRunning { get; set; }

        public void StartWebserver()
        {
            this._server = new Server("localhost", 9550, false, this.RequestReceived);
            this._server.Events.ExceptionEncountered += WebserverException;
            this._server.Start();

            this.IsRunning = true;
            /*
            this._server = new Server(
    _Settings.Server.DnsHostname,
    _Settings.Server.Port,
    _Settings.Server.Ssl,
    RequestReceived);
            _Server.Events.ExceptionEncountered += WebserverException;
            _Server.Start();

            */
        }

        // http://localhost:9550/robots.txt
        // http://localhost:9550
        // curl http://localhost:9550/testcollection/testarea/firstfile.txt
        // curl -X POST -d "My first object!" "http://localhost:9550/testcollection/testarea/firstfile.txt?x-api-key=default"


        private async Task RequestReceived(HttpContext ctx)
        {
            string header = this._header + ctx.Request.Source.IpAddress + ":" + ctx.Request.Source.Port + " ";

            DateTime startTime = DateTime.Now;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            RequestMetadata md = new RequestMetadata();
            md.Http = ctx;

            try
            {
                if (ctx.Request.Method == HttpMethod.OPTIONS)
                {
                    // await OptionsHandler(ctx);
                    return;
                }

                if (ctx.Request.Url.Elements != null && ctx.Request.Url.Elements.Length > 0)
                {
                    if (ctx.Request.Url.Elements[0].Equals("favicon.ico"))
                    {
                        ctx.Response.StatusCode = 200;
                        await ctx.Response.Send(LiTools.Helpers.IO.File.ReadBinaryFile("./wwwroot/favicon.ico"));

                        return;
                    }

                    if (ctx.Request.Url.Elements[0].Equals("robots.txt"))
                    {
                        ctx.Response.StatusCode = 200;
                        ctx.Response.ContentType = "text/plain";
                        await ctx.Response.Send("User-Agent: *\r\nDisallow:\r\n");
                        return;
                    }
                }

                this._connMgr.Add(Thread.CurrentThread.ManagedThreadId, ctx);

                // string apiKeyVal = ctx.Request.RetrieveHeaderValue(_Settings.Server.HeaderApiKey);
                string apiKeyVal = ctx.Request.RetrieveHeaderValue(this._node.ConfigFileData.HeaderApiKey);

                switch (ctx.Request.Method)
                {
                    case HttpMethod.GET:

                        this.zzDebug = "dfd";
                        await this.HttpMethodGet(md);
                        this.zzDebug = "dffs";

                        break;
                    case HttpMethod.POST:
                        await this.HttpMethodPost(md);
                        this.zzDebug = "sdfdf";
                        break;
                    default:
                        this.zzDebug = "sdfd";
                        break;
                }

                this.zzDebug = "sdfdf";


                //if (ctx.Request.Url.Elements != null
                //    && ctx.Request.Url.Elements.Length >= 2
                //    && ctx.Request.Url.Elements[0].Equals("admin")
                //    && md.Perm.IsAdmin)
                //{
                //    await AdminApiHandler(md);
                //}
                //else
                //{
                //    await UserApiHandler(md);
                //}


                return;
            }
            catch (Exception e)
            {
                //_Logging.Exception("StorageServer", "RequestReceived", e);
                ctx.Response.StatusCode = 500;
                ctx.Response.ContentType = "application/json";
                await ctx.Response.Send("User-Agent: *\r\nDisallow:\r\n");
                // await ctx.Response.Send(Common.SerializeJson(new ErrorResponse(1, 500, "Outer exception.", null), true));
                return;
            }
            finally
            {
                sw.Stop();

                this._connMgr.Close(Thread.CurrentThread.ManagedThreadId);

                string msg =
                    header +
                    ctx.Request.Method + " " + ctx.Request.Url.RawWithoutQuery + " " +
                    ctx.Response.StatusCode + " " +
                    "[" + sw.ElapsedMilliseconds + "ms]";

                this.zzDebug = "sfdsf";

                // _Logging.Debug(msg);
            }

            this.zzDebug = "sdfdf";

        }
        private async Task HttpMethodPost(RequestMetadata md)
        {
            if (md?.Http == null)
            {
                return;
            }

            if (md.Http.Request.Url.Elements.Length >= 3)
            {
                // Api access to object saved on this node.

                await this._objectStorage.HttpPostObject(md);
                //await HttpGetObject(md);
                this.zzDebug = "sfdsf";
                return;
            }

            md.Http.Response.StatusCode = 404;
            md.Http.Response.ContentType = "application/json";
            await md.Http.Response.Send(Helpers.CommonHelper.SerializeJson(new Models.Http.ResponseModel(4, 404, null, null), true));

            this.zzDebug = "sdfdf";
        
        }

        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:BracesMustNotBeOmitted", Justification = "Reviewed.")]
        private async Task HttpMethodGet(RequestMetadata md)
        {
            //static async Task UserApiHandler(RequestMetadata md)
            //if (md == null)
            //    return;

            if (md?.Http == null)
                return;

            if (md.Http.Request.Url.RawWithoutQuery.Equals("/containers"))
            {
                this.zzDebug = "sfdsf";
                //await HttpGetContainerList(md);
                return;
            }
            //else if (md.Http.Request.Url.Elements.Length == 1)
            //{
            //    this.zzDebug = "sfdsf";
            //    //await HttpGetContainerList(md);
            //    return;
            //}
            //else if (md.Http.Request.Url.Elements.Length == 2)
            //{
            //    this.zzDebug = "sfdsf";
            //    //await HttpGetContainer(md);
            //    return;
            //}
            else if (md.Http.Request.Url.Elements.Length >= 3)
            {
                // Api access to object saved on this node.

                await this._objectStorage.HttpGetObject(md);
                //await HttpGetObject(md);
                this.zzDebug = "sfdsf";
                return;
            }

            md.Http.Response.StatusCode = 404;
            md.Http.Response.ContentType = "application/json";
            await md.Http.Response.Send(Helpers.CommonHelper.SerializeJson(new Models.Http.ResponseModel(4, 404, null, null), true));

            this.zzDebug = "sdfdsf";
        }

        private void WebserverException(object sender, ExceptionEventArgs args)
        {
            // _logger.Warn(_Header + "exception for " + args.Ip + ":" + args.Port + ":" + Environment.NewLine + Common.SerializeJson(args.Exception, true));

            this.zzDebug = "Sdfd";
        }
    }

    internal class ConnectionManager
    {
        private List<Connection> _Connections;
        private readonly object _ConnectionsLock;

        internal ConnectionManager()
        {
            _Connections = new List<Connection>();
            _ConnectionsLock = new object();
        }

        internal void Add(int threadId, HttpContext ctx)
        {
            if (threadId <= 0) return;
            if (ctx == null) return;
            if (ctx.Request == null) return;

            Connection conn = new Connection();
            conn.ThreadId = threadId;
            conn.SourceIp = ctx.Request.Source.IpAddress;
            conn.SourcePort = ctx.Request.Source.Port;
            conn.UserGUID = null;
            conn.Method = ctx.Request.Method;
            conn.RawUrl = ctx.Request.Url.RawWithoutQuery;
            conn.StartTime = DateTime.Now;
            conn.EndTime = DateTime.Now;

            lock (_ConnectionsLock)
            {
                _Connections.Add(conn);
            }
        }

        internal void Close(int threadId)
        {
            if (threadId <= 0) return;

            lock (_ConnectionsLock)
            {
                _Connections = _Connections.Where(x => x.ThreadId != threadId).ToList();
            }
        }

        //internal void Update(int threadId, UserMaster user)
        //{
        //    if (threadId <= 0) return;
        //    if (user == null) return;

        //    List<Connection> tempList = new List<Connection>();

        //    lock (_ConnectionsLock)
        //    {
        //        foreach (Connection curr in _Connections)
        //        {
        //            if (curr.ThreadId != threadId)
        //            {
        //                tempList.Add(curr);
        //            }
        //            else
        //            {
        //                curr.UserGUID = user.GUID;
        //                tempList.Add(curr);
        //            }
        //        }

        //        _Connections = tempList;
        //    }
        //}

        internal List<Connection> GetActiveConnections()
        {
            List<Connection> curr = new List<Connection>();

            lock (_ConnectionsLock)
            {
                curr = new List<Connection>(_Connections);
            }

            return curr;
        }
    }

    /// <summary>
    /// An HTTP connection.
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// Thread ID of the connection.
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        /// Source IP address.
        /// </summary>
        public string SourceIp { get; set; }

        /// <summary>
        /// Source TCP port.
        /// </summary>
        public int SourcePort { get; set; }

        /// <summary>
        /// User GUID.
        /// </summary>
        public string UserGUID { get; set; }

        /// <summary>
        /// HTTP method
        /// </summary>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// URL being accessed.
        /// </summary>
        public string RawUrl { get; set; }

        /// <summary>
        /// When the connection was received.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// When the connection was terminated.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Instantiates the object.
        /// </summary>
        public Connection()
        {

        }
    }

}
