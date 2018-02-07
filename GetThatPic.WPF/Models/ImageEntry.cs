// <copyright file="ImageEntry.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>
using System;
using System.Windows.Media.Imaging;
using GetThatPic.Parsing.Models;

namespace GetThatPic.WPF.Models
{
    /// <summary>
    /// Combines ImageMetaData with a BitmapImage.
    /// </summary>
    public class ImageEntry
    {
        /// <summary>
        /// The bitmap.
        /// </summary>
        private BitmapImage bitmap;

        /// <summary>
        /// Gets or sets the meta data.
        /// </summary>
        /// <value>
        /// The meta data.
        /// </value>
        public ImageMetaData MetaData { get; set; }
        
        /// <summary>
        /// Gets the bitmap.
        /// </summary>
        /// <value>
        /// The bitmap.
        /// </value>
        public BitmapImage Bitmap => bitmap ?? (bitmap = new BitmapImage(new Uri(MetaData.ImageUrl)));
    }
}
