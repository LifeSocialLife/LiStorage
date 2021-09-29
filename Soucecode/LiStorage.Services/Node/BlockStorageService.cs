// <copyright file="BlockStorageService.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>

namespace LiStorage.Services.Node
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Dynamic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using LiStorage.Models.CollectionPool;
    using LiStorage.Models.StoragePool;
    using LiTools.Helpers.Convert;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// File storage Collections - Area Service
    /// </summary>
    public class BlockStorageService
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed.")]
        private readonly ILogger<BlockStorageService> _logger;

        private Dictionary<string, LiStorage.Models.ObjectStorage.BlockStorageDictModel> Block { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockStorageService"/> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="rundataService"></param>
        /// <param name="fileOperation"></param>
        /// <param name="rundataNode"></param>
        /// <param name="storagePoolService"></param>
        /// <param name="collectionService">CollectionService.</param>
        public BlockStorageService(ILogger<BlockStorageService> logger, RundataService rundataService, FileOperationService fileOperation, RundataNodeService rundataNode, StoragePoolService storagePoolService, CollectionPoolService collectionService)
        {
            this.zzDebug = "ObjectStorageService";
            this._logger = logger;

            this._rundata = rundataService;
            this._node = rundataNode;
            this._fileOperation = fileOperation;
            this._storagePool = storagePoolService;
            this._collections = collectionService;
            this._lockKey = new object();
            this.Block = new Dictionary<string, Models.ObjectStorage.BlockStorageDictModel>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        private string zzDebug { get; set; }
#pragma warning disable SA1309 // Field names should not begin with underscore
        private readonly RundataService _rundata;
        private readonly RundataNodeService _node;
        private readonly FileOperationService _fileOperation;
        private readonly CollectionPoolService _collections;
        private readonly StoragePoolService _storagePool;
        private readonly object _lockKey;
#pragma warning restore SA1309 // Field names should not begin with underscore


        internal async Task HttpPostObject(Classes.RequestMetadata md)
        {
            this.zzDebug = "sddf";

        }

        /// <summary>
        /// Get object from storage pool.
        /// </summary>
        /// <param name="md">RequestMetadata</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1123:DoNotPlaceRegionsWithinElements", Justification = "Reviewed.")]
        internal async Task HttpGetObject(Classes.RequestMetadata md)
        {
            if (md.Http.Request.Url.Elements.Length <= 2)
            {
                md.Http.Response.StatusCode = 404;
                md.Http.Response.ContentType = "application/json";
                await md.Http.Response.Send(Helpers.CommonHelper.SerializeJson(new Models.Http.ResponseModel(2, 404, null, null), true));
                return;
            }

            // var dddd = md.BlockData;

            // Get storage pool name and filename.
            //var tmpfilenmame = md.Http.Request.Url.Elements[md.Http.Request.Url.Elements.Length - 1].ToString().ToLower();
            //var tmpCollectionName = string.Join("-", md.Http.Request.Url.Elements.SkipLast(1)).ToLower();
            //var tmpObjectName = string.Join("-", md.Http.Request.Url.Elements).ToLower();

            this.zzDebug = " sdfdf";

            Models.ObjectStorage.BlockStorageDictModel tmpBlockdata = new Models.ObjectStorage.BlockStorageDictModel();

            // var tmpElementsWhitoutLast = md.Http.Request.Url.Elements.SkipLast(1);
            // var tmpStgName = string.Join("-", tmpElementsWhitoutLast).ToLower();

            // Check if object meta is preeloaded in memory.
            if (this.Exist(md.BlockData.ObjectName))
            {
                tmpBlockdata = this.Get(md.BlockData.ObjectName);
                this.zzDebug = "sdfsdf";
            }
            else
            {
                // Block id dont exist in memory. Locate bloock meta data from storage Pool.
                CollectionPoolModel? collectionInformation = new CollectionPoolModel();

                // Do collection exist?
                if (this._collections.ContainsKey(md.BlockData.CollectionName))
                {
                    // Collection exist. Get information
                    collectionInformation = this._collections.Get(md.BlockData.CollectionName);

                    this.zzDebug = "sdfd";
                }

                if (!collectionInformation.Filedata.Enabled)
                {
                    // Collection dont exist. of is enables = false.
                    md.Http.Response.StatusCode = 404;
                    md.Http.Response.ContentType = "application/json";
                    await md.Http.Response.Send(Helpers.CommonHelper.SerializeJson(new Models.Http.ResponseModel(5, 404, null, null), true));

                    this.zzDebug = "fdsf";
                    return;
                }

                // Do storage Pool exist.
                bool tmpStoragePoolExist = false;
                StoragePoolModel storagepoolMeta = new StoragePoolModel();
                if (this._storagePool.ContainsKey(collectionInformation.Filedata.StoragePoolMetaDefault))
                {
                    tmpStoragePoolExist = true;

                    storagepoolMeta = this._storagePool.Get(collectionInformation.Filedata.StoragePoolMetaDefault);

                    if (!storagepoolMeta.Filedata.Enabled)
                    {
                        tmpStoragePoolExist = false;
                    }

                    this.zzDebug = "sdfdsf";

                }

                if (!tmpStoragePoolExist)
                {
                    // Storage pool dont exist or is enable = false.
                    md.Http.Response.StatusCode = 404;
                    md.Http.Response.ContentType = "application/json";
                    await md.Http.Response.Send(Helpers.CommonHelper.SerializeJson(new Models.Http.ResponseModel(5, 404, null, null), true));

                    this.zzDebug = "fdsf";
                    return;
                }
                this.zzDebug = "sdfsdf";


                #region Get block meta file from storage if the meta file exist.

                var a1 = collectionInformation;
                var a2 = storagepoolMeta;

                //string fullPath = Path.Combine(md.Http.Request.Url.Elements);

                //tmpBlockdata.FileMetaPath = Path.Combine(storagepoolMeta.Filedata.FolderPath, "meta", Path.Combine(md.Http.Request.Url.Elements)).Trim().ToLower() + ".meta";
                tmpBlockdata = this._storagePool.GetMetaFile(storagepoolMeta, md.Http.Request.Url.Elements, tmpBlockdata);


                if (!tmpBlockdata.FileMetaExist)
                {
                    // Meta file dont exist.
                    md.Http.Response.StatusCode = 404;
                    md.Http.Response.ContentType = "application/json";
                    await md.Http.Response.Send(Helpers.CommonHelper.SerializeJson(new Models.Http.ResponseModel(5, 404, null, null), true));

                    this.zzDebug = "fdsf";
                    return;
                }

                this.zzDebug = "sddfdsf";
                //storagepoolData.Filedata.FolderPath
                #endregion

            }


            var d2 = this._collections.GetAll();






            this.zzDebug = "sdfdsf";

            md.Http.Response.StatusCode = 401;
            md.Http.Response.ContentType = "application/json";
            await md.Http.Response.Send(Helpers.CommonHelper.SerializeJson(new Models.Http.ResponseModel(3, 401, null, null), true));
            return;
        }

        /// <summary>
        /// Do this block exist?.
        /// </summary>
        /// <param name="blockId">Block key</param>
        /// <returns>true or false</returns>
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:BracesMustNotBeOmitted", Justification = "Reviewed.")]
        internal bool Exist(string blockId)
        {
            bool tmpReturn = false;
            lock (this._lockKey)
            {
                if (this.Block.ContainsKey(blockId))
                    tmpReturn = true;
            }

            return tmpReturn;
        }

        /// <summary>
        /// Get information about this blockId.
        /// </summary>
        /// <param name="blockId">blockid Key of block to get.</param>
        /// <returns>BlockStorageDictModel.</returns>
        internal Models.ObjectStorage.BlockStorageDictModel Get(string blockId)
        {
            Models.ObjectStorage.BlockStorageDictModel? data;

            lock (this._lockKey)
            {
                data = this.Block[blockId];
            }

            return data;
        }

    }
}
