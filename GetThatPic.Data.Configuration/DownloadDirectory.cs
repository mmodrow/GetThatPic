// <copyright file="DownloadDirectory.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>
namespace GetThatPic.Data.Configuration
{
    /// <summary>
    /// Models a DownloadDirectory configuration item.
    /// </summary>
    public class DownloadDirectory
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the directory.
        /// </summary>
        /// <value>
        /// The directory.
        /// </value>
        public string Directory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is path relative.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is path relative; otherwise, <c>false</c>.
        /// </value>
        public bool IsPathRelative { get; set; } = true;
    }
}
