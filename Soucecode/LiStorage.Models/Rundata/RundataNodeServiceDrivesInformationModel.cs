// <summary>
// Model RundataNodeServiceDrivesInformationModel.
// </summary>
// <copyright file="RundataNodeServiceDrivesInformationModel.cs" company="LiSoLi">
// Copyright (c) LiSoLi. All rights reserved.
// </copyright>
// <author>Lennie Wennerlund (lempa)</author>

namespace LiStorage.Models.Rundata
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Drives information Model.
    /// </summary>
    public class RundataNodeServiceDrivesInformationModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RundataNodeServiceDrivesInformationModel"/> class.
        /// </summary>
        public RundataNodeServiceDrivesInformationModel()
        {
            this.Drive = new Dictionary<string, RundataNodeServiceDrivesInformationDictModel>();
            this.LastChecked = Convert.ToDateTime("2000-01-01 00:00:00");
            this.CheckedIsRunning = false;
        }

        /// <summary>
        /// Gets or sets information about all drives that exist in this node.
        /// </summary>
        public Dictionary<string, RundataNodeServiceDrivesInformationDictModel> Drive { get; set; }

        /// <summary>
        /// Gets or sets date and time in utc when it was last run.
        /// </summary>
        public DateTime LastChecked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is the drives collecter running now.
        /// </summary>
        public bool CheckedIsRunning { get; set; }
    }
}
