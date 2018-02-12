// <copyright file="MainWindowState.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

using System.Collections.Generic;
using GetThatPic.Data.Structure;
using GetThatPic.Parsing;

namespace GetThatPic.WPF.Models
{
    /// <summary>
    /// Main Window State Model - Keeps track of what is happening.
    /// </summary>
    public class MainWindowState
    {
        /// <summary>
        /// Gets history buffer.
        /// </summary>
        /// <value>
        /// The history.
        /// </value>
        public RingBuffer<ImageEntry> History { get; } = new RingBuffer<ImageEntry>(50);

        /// <summary>
        /// Gets the download queue.
        /// </summary>
        /// <value>
        /// The download queue.
        /// </value>
        public Queue<ImageEntry> DownloadQueue { get; } = new Queue<ImageEntry>();

        /// <summary>
        /// Gets the dropped urls.
        /// </summary>
        /// <value>
        /// The dropped urls.
        /// </value>
        public IList<string> DroppedUrls { get; } = new List<string>();

        /// <summary>
        /// Gets the link parser.
        /// </summary>
        /// <value>
        /// The link parser.
        /// </value>
        public Link LinkParser { get; } = new Link();

        /// <summary>
        /// Gets or sets the preview item.
        /// </summary>
        /// <value>
        /// The preview item.
        /// </value>
        public ImageEntry PreviewItem { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is downloading currently.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is downloading; otherwise, <c>false</c>.
        /// </value>
        public bool IsDownloading { get; set; }
    }
}
