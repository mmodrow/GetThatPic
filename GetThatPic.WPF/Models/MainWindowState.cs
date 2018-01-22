// <copyright file="MainWindowState.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>
using System;
using System.Windows.Media.Imaging;
using GetThatPic.Data.Structure;

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
        /// Gets or sets the preview item.
        /// </summary>
        /// <value>
        /// The preview item.
        /// </value>
        public ImageEntry PreviewItem { get; set; }

        /// <summary>
        /// Loads the image from URL to preview.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A bitmap of the given url's target image if valid.</returns>
        public BitmapImage LoadImageFromUrlToPreview(string url)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(url, UriKind.Absolute);
                bitmap.EndInit();

                return bitmap;
            }
            catch
            {
                return null;
            }
        }
    }
}
