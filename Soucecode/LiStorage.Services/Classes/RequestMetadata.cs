// <copyright file="RequestMetadata.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>

namespace LiStorage.Services.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using WatsonWebserver;

    /// <summary>
    /// Metadata for an incoming HTTP API request.
    /// </summary>
    public class RequestMetadata
    {


        /// <summary>
        /// Initializes a new instance of the <see cref="RequestMetadata"/> class.
        /// </summary>
        public RequestMetadata()
        {
            this.Http = new HttpContext();
            this.BlockDataBuildIsDone = false;
            this._BlockData = new RequestMetadataBlockDataModel();
        }

        /// <summary>
        /// Gets or sets the HttpContext object from the web server.
        /// </summary>
        public HttpContext Http { get; set; }
        public RequestMetadataBlockDataModel BlockData
        {
            get
            {
                if (!this.BlockDataBuildIsDone)
                {
                    this.BlockDataBuild();
                }

                return this._BlockData;
            }
        }
        private bool BlockDataBuildIsDone { get; set; }
        private RequestMetadataBlockDataModel _BlockData { get; set; }
       

        private void BlockDataBuild()
        {
            if (this.Http == null)
            {
                return;
            }

            this._BlockData = new RequestMetadataBlockDataModel()
            {
                CollectionName = string.Join("-", this.Http.Request.Url.Elements.SkipLast(1)).ToLower(),
                Filename = this.Http.Request.Url.Elements[this.Http.Request.Url.Elements.Length - 1].ToString().ToLower(),
                ObjectName = string.Join("-", this.Http.Request.Url.Elements).ToLower(),
            };
            
            // var tmpCollectionName = string.Join("-", md.Http.Request.Url.Elements.SkipLast(1)).ToLower();
            this.BlockDataBuildIsDone = true;
        }

    }

    /// <summary>
    /// Blockdata model for RequestMetadata.
    /// </summary>
    public class RequestMetadataBlockDataModel
    {
        public RequestMetadataBlockDataModel()
        {
            this.Filename = string.Empty;
            this.CollectionName = string.Empty;
            this.ObjectName = string.Empty;
        }
        public string Filename { get; set; }
        public string CollectionName { get; set; }
        public string ObjectName { get; set; }
    }
}
