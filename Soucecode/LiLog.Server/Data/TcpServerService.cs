// <summary>
// Main handler for all background work, node and master.
// </summary>
// <copyright file="TcpServerService.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>

namespace LiLog.Server.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WatsonTcp;

    public class TcpServerService
    {
#pragma warning disable SA1309 // FieldNamesMustNotBeginWithUnderscore
        private WatsonTcpServer _Server; // = null;
#pragma warning restore SA1309 // FieldNamesMustNotBeginWithUnderscore



        public TcpServerService()
        {
            this._Server = new WatsonTcpServer("0.0.0.0", 9000);
        }

        public void StartServer()
        {
            this._Server = new WatsonTcpServer(null, 9000);
            this._Server.Events.ClientConnected += this.ClientConnected;
            this._Server.Events.ClientDisconnected += ClientDisconnected;
            this._Server.Events.MessageReceived += MessageReceived;

            this._Server.Events.ServerStarted += ServerStarted;
            this._Server.Events.ServerStopped += ServerStopped;

            this._Server.Callbacks.SyncRequestReceived = SyncRequestReceived;

            this._Server.Start();

        }

        private void SendToClient()
        {
            //this._Server.Send()
        }

        /// <summary>
        /// New client has connected to server.
        /// </summary>
        /// <param name="sender">object.</param>
        /// <param name="args">ConnectionEventArgs.</param>
        private void ClientConnected(object? sender, ConnectionEventArgs args)
        {
            Console.WriteLine("Client connected: " + args.IpPort);
        }

        static void ClientDisconnected(object sender, DisconnectionEventArgs args)
        {
            Console.WriteLine("Client disconnected: " + args.IpPort + ": " + args.Reason.ToString());
        }

        private void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Console.WriteLine("Message from " + args.IpPort + ": " + Encoding.UTF8.GetString(args.Data));
            // _Server.cli
        }

        private static void ServerStarted(object sender, EventArgs args)
        {
            Console.WriteLine("Server started");
        }

        private static void ServerStopped(object sender, EventArgs args)
        {
            Console.WriteLine("Server stopped");
        }

        static SyncResponse SyncRequestReceived(SyncRequest req)
        {
            Console.Write("Synchronous request received from " + req.IpPort + ": ");
            if (req.Data != null) Console.WriteLine(Encoding.UTF8.GetString(req.Data));
            else Console.WriteLine("[null]");

            if (req.Metadata != null && req.Metadata.Count > 0)
            {
                Console.WriteLine("Metadata:");
                foreach (KeyValuePair<object, object> curr in req.Metadata)
                {
                    Console.WriteLine("  " + curr.Key.ToString() + ": " + curr.Value.ToString());
                }
            }

            Dictionary<object, object> retMetadata = new Dictionary<object, object>();
            retMetadata.Add("foo", "bar");
            retMetadata.Add("bar", "baz");

            // Uncomment to test timeout
            // Task.Delay(10000).Wait();
            Console.WriteLine("Sending synchronous response");
            return new SyncResponse(req, retMetadata, "Here is your response!");
        }
      
    }
}
