using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiLog.Server.MongoDb.Services
{
    public class MdbRundataService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private string zzDebug { get; set; }

        public MdbRundataService()
        {
            // this._AppSettings = options;

            this.zzDebug = "SDfdsf";

        }

        private MongoClient GetConnectionBase()
        {
            var db1Credential = MongoCredential.CreateCredential(this._AppSettings.MongoDb.DatabaseName, this._AppSettings.MongoDb.Username, this._AppSettings.MongoDb.Password);

            var settings = new MongoClientSettings
            {
                Server = new MongoServerAddress(this._AppSettings.MongoDb.SrvIp, this._AppSettings.MongoDb.SrvPort),
                ConnectTimeout = TimeSpan.FromSeconds(10),
                HeartbeatTimeout = TimeSpan.FromSeconds(10),
                ServerSelectionTimeout = TimeSpan.FromSeconds(10),
                SocketTimeout = TimeSpan.FromSeconds(30),
                WaitQueueTimeout = TimeSpan.FromSeconds(30),
                Credential = db1Credential,
            };

            // var client = new MongoClient(settings);
            return new MongoClient(settings);
        }

    }
}
