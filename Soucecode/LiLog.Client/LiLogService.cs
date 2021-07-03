using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using WatsonTcp;

namespace LiLog.Client
{
    public class LiLogService
    {
        private WatsonTcpClient _Client = null;

        private string _ServerIp = "172.16.100.88";
        private int _ServerPort = 9000;
        private static string _PresharedKey = null;

        public LiLogService()
        {

        }

        public void Init()
        {
            _Client = new WatsonTcpClient(_ServerIp, _ServerPort);

            _Client.Events.AuthenticationFailure += AuthenticationFailure;
            _Client.Events.AuthenticationSucceeded += AuthenticationSucceeded;
            _Client.Events.ServerConnected += ServerConnected;
            _Client.Events.ServerDisconnected += ServerDisconnected;
            _Client.Events.MessageReceived += MessageReceived;

            _Client.Callbacks.SyncRequestReceived = SyncRequestReceived;
            _Client.Callbacks.AuthenticationRequested = AuthenticationRequested;

            _Client.Keepalive.EnableTcpKeepAlives = true;
            _Client.Keepalive.TcpKeepAliveInterval = 1;
            _Client.Keepalive.TcpKeepAliveTime = 1;
            _Client.Keepalive.TcpKeepAliveRetryCount = 3;

            _Client.Connect();

        }

        public void send(string text)
        {
            if (this._Client.Connected)
            {
                if (!_Client.Send(Encoding.UTF8.GetBytes(text))) Console.WriteLine("Failed");
            }
            // userInput = InputString("Data:", null, false);
            

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
