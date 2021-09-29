// <copyright file="LiLogService.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>

namespace LiLog.Client
{
    using LiLog.Client.Models;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using System.Xml.Serialization;
    using WatsonTcp;

    public class LiLogService
    {
#pragma warning disable SA1309 // FieldNamesMustNotBeginWithUnderscore
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private WatsonTcpClient _client;
        private string _ServerIp = "172.16.100.88";
        private int _ServerPort = 9000;
        private static string _PresharedKey = null;
#pragma warning restore SA1309 // FieldNamesMustNotBeginWithUnderscore

        private ServerConnectionModel ServerSettings { get; set; }

        private bool ServerSettingsExist { get; set; }

        public LiLogService(IHostApplicationLifetime hostApplicationLifetime)
        {
            this._hostApplicationLifetime = hostApplicationLifetime;
            this._hostApplicationLifetime.ApplicationStarted.Register(this.OnStarted, true);
            this._hostApplicationLifetime.ApplicationStopping.Register(this.OnStopping, true);
            this._hostApplicationLifetime.ApplicationStopped.Register(this.OnStopped, true);

            this.ServerSettings = new ServerConnectionModel();
            this.ServerSettingsExist = false;
            this.zzDebug = "LiLogService";
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]

        private string zzDebug { get; set; }


        public void Init()
        {
            this._client = new WatsonTcpClient(_ServerIp, _ServerPort);

            this._client.Events.AuthenticationFailure += AuthenticationFailure;
            this._client.Events.AuthenticationSucceeded += AuthenticationSucceeded;
            this._client.Events.ServerConnected += ServerConnected;
            this._client.Events.ServerDisconnected += ServerDisconnected;
            this._client.Events.MessageReceived += MessageReceived;

            this._client.Callbacks.SyncRequestReceived = SyncRequestReceived;
            this._client.Callbacks.AuthenticationRequested = AuthenticationRequested;

            this._client.Keepalive.EnableTcpKeepAlives = true;
            this._client.Keepalive.TcpKeepAliveInterval = 1;
            this._client.Keepalive.TcpKeepAliveTime = 1;
            this._client.Keepalive.TcpKeepAliveRetryCount = 3;

            this._client.Connect();

        }

        public void send(string text)
        {
            if (this._client.Connected)
            {
                if (!_client.Send(Encoding.UTF8.GetBytes(text))) Console.WriteLine("Failed");
            }
            // userInput = InputString("Data:", null, false);
            

        }

        private void OnStarted()
        {
            //this._logger.LogInformation("OnStarted has been called.");

            this.zzDebug = "sdfdf";

            // Perform post-startup activities here
        }

        private void OnStopping()
        {
            //this._logger.LogInformation("NodeWorker | OnStopping | Stop all backgrounds work.");
            //this._logger.LogInformation("NodeWorker | OnStopping | Stop all backgrounds work. | Done");

            // _logger.LogInformation("OnStopping has been called.");
            this.zzDebug = "sdfdf";

            // Perform on-stopping activities here
        }

        private void OnStopped()
        {
            // Perform post-stopped activities here
            // this._logger.LogInformation("OnStopped has been called.");

            this.zzDebug = "sdfdf";
        }

        private static string AuthenticationRequested()
        {
            // return "0000000000000000";
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Server requests authentication");
            Console.WriteLine("Press ENTER and THEN enter your preshared key");
            if (String.IsNullOrEmpty(_PresharedKey)) _PresharedKey = InputString("Preshared key:", "1234567812345678", false);
            return _PresharedKey;
        }

        private static void AuthenticationSucceeded(object sender, EventArgs args)
        {
            Console.WriteLine("Authentication succeeded");
        }

        private static void AuthenticationFailure(object sender, EventArgs args)
        {
            Console.WriteLine("Authentication failed");
        }

        private static void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Console.Write("Message from " + args.IpPort + ": ");
            if (args.Data != null) Console.WriteLine(Encoding.UTF8.GetString(args.Data));
            else Console.WriteLine("[null]");

            if (args.Metadata != null && args.Metadata.Count > 0)
            {
                Console.WriteLine("Metadata:");
                foreach (KeyValuePair<object, object> curr in args.Metadata)
                {
                    Console.WriteLine("  " + curr.Key.ToString() + ": " + curr.Value.ToString());
                }
            }
        }

        private static SyncResponse SyncRequestReceived(SyncRequest req)
        {
            Console.Write("Message received from " + req.IpPort + ": ");
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
            return new SyncResponse(req, retMetadata, "Here is your response!");
        }

        private static void ServerConnected(object sender, ConnectionEventArgs args)
        {
            Console.WriteLine(args.IpPort + " connected");
        }

        private static void ServerDisconnected(object sender, DisconnectionEventArgs args)
        {
            Console.WriteLine(args.IpPort + " disconnected: " + args.Reason.ToString());
        }
        
         private static string InputString(string question, string defaultAnswer, bool allowNull)
        {
            while (true)
            {
                Console.Write(question);

                if (!String.IsNullOrEmpty(defaultAnswer))
                {
                    Console.Write(" [" + defaultAnswer + "]");
                }

                Console.Write(" ");

                string userInput = Console.ReadLine();

                if (String.IsNullOrEmpty(userInput))
                {
                    if (!String.IsNullOrEmpty(defaultAnswer)) return defaultAnswer;
                    if (allowNull) return null;
                    else continue;
                }

                return userInput;
            }
        }

    }
}
