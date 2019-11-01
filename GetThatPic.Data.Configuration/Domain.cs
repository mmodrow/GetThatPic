// <copyright file="Domain.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

using System;
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
        /// The download directory.
        /// </summary>
        private string downloadDirectory;
        
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
        public Regex Url { get; set; }

        /// <summary>
        /// Gets or sets the Regex to identify the path portion of the url.
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
        public IEnumerable<DomElementAccessor> Images { get; set; } = new List<DomElementAccessor>();

        /// <summary>
        /// Gets or sets the file name fragments.
        /// </summary>
        /// <value>
        /// The file name fragments.
        /// </value>
        public IEnumerable<DomElementAccessor> FileNameFragments { get; set; } = new List<DomElementAccessor>();

        /// <summary>
        /// Gets or sets the file name fragment delimiter.
        /// </summary>
        /// <value>
        /// The file name fragment delimiter.
        /// </value>
        public string FileNameFragmentDelimiter { get; set; } = " - ";

        /// <summary>
        /// Gets or sets the download directory.
        /// Defaults to %userprofile%\Pictures\GetThatPic\»DomainName«
        /// </summary>
        /// <value>
        /// The download directory.
        /// </value>
        public string DownloadDirectory
        {
            get
            {
                if (string.IsNullOrWhiteSpace(downloadDirectory))
                {
                    DownloadDirectory = Name;
                }

                if (IsPathRelative)
                {
                    return Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) +
                       @"\GetThatPic\" + downloadDirectory + @"\";
                }

                return downloadDirectory + @"\";
            }

            set => downloadDirectory = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is path relative.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is path relative; otherwise, <c>false</c>.
        /// </value>
        public bool IsPathRelative { get; set; } = true;

        /// <summary>
        /// Gets or sets the default file ending.
        /// </summary>
        /// <value>
        /// The default file ending.
        /// </value>
        public string DefaultFileEnding { get; set; } = string.Empty;

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
