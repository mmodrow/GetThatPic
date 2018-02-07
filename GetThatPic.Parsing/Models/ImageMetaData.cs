// <copyright file="ImageMetaData.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

namespace GetThatPic.Parsing.Models
{
    /// <summary>
    /// Models all data needed to know about a previously downloaded image.
    /// </summary>
    public class ImageMetaData
    {
        /// <summary>
        /// Gets or sets the image url.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the file system location.
        /// </summary>
        /// <value>
        /// The file system location.
        /// </value>
        public string TargetFileSystemLocation { get; set; }
    }
}
