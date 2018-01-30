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
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the Regex to identify the path portion of the url.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public Regex Path { get; set; }

        public string PathPatternString
        {
            set => Path = new Regex(value);
        }

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
                    DownloadDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) +
                                      @"\GetThatPic\" + Name;
                }

                return downloadDirectory;
            }

            set => downloadDirectory = value;
        }
    }
}
