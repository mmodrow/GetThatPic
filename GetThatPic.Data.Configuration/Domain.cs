// <copyright file="Domain.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GetThatPic.Data.Configuration
{
    /// <summary>
    /// Models a Domain Configuration Node
    /// </summary>
    public class Domain
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public Regex Path { get; set; }

        /// <summary>
        /// Gets or sets the image download instructions.
        /// </summary>
        /// <value>
        /// The images.
        /// </value>
        public IEnumerable<IImageDownloadInstruction> Images { get; set; }
    }
}
