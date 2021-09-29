// <summary>
// Model RundataNodeServiceDrivesInformationDictModel.
// </summary>
// <copyright file="RundataNodeServiceDrivesInformationDictModel.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Models.Rundata
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using static LiTools.Helpers.IO.Drives;

    /// <summary>
    /// RundataNodeServiceDrivesInformationDictModel.
    /// </summary>
    public class RundataNodeServiceDrivesInformationDictModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RundataNodeServiceDrivesInformationDictModel"/> class.
        /// </summary>
        public RundataNodeServiceDrivesInformationDictModel()
        {
            this.ContainNewData = false;
            this.Data = new GetDrivesInformationReturnModel();
            this.DtAdded = DateTime.UtcNow;
            this.DtLastChecked = DateTime.UtcNow;
            this.DtLastUpdated = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets or sets a value indicating whether is this information new or old information?.
        /// </summary>
        public bool ContainNewData { get; set; }

        /// <summary>
        /// Gets or sets drivesInformation Return Model from Drives Helper.
        /// </summary>
        public GetDrivesInformationReturnModel Data { get; set; }

        /// <summary>
        /// Gets or sets when was it last updated.
        /// </summary>
        public DateTime DtLastUpdated { get; set; }

        /// <summary>
        /// Gets or sets when was it last checked.
        /// </summary>
        public DateTime DtLastChecked { get; set; }

        /// <summary>
        /// Gets or sets when was it added.
        /// </summary>
        public DateTime DtAdded { get; set; }
    }
}
