// <copyright file="ImageMetaData.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

namespace GetThatPic.Data.Image
{
    using System;

    using GetThatPic.Parsing;

    /// <summary>
    /// Models all data needed to know about a previously downloaded image.
    /// </summary>
    public class ImageMetaData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageMetaData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="imageUrl">
        /// The image url.
        /// </param>
        /// <param name="targetFileSystemLocation">
        /// The target file system location.
        /// </param>
        public ImageMetaData(string name, Uri imageUrl, string targetFileSystemLocation)
        {
            name = name.Trim();
            if (name.Length == 0)
            {
                throw new ArgumentException($"{nameof(name)} may not be empty.");
            }

            targetFileSystemLocation = targetFileSystemLocation.Trim();
            if (targetFileSystemLocation.Length == 0)
            {
                throw new ArgumentException($"{nameof(targetFileSystemLocation)} may not be empty.");
            }

            this.Name = name;
            this.ImageUrl = imageUrl;
            this.TargetFileSystemLocation = targetFileSystemLocation;
        }

        /// <summary>
        /// Gets the image url.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public Uri ImageUrl { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is lossless.
        /// The decision is based on the UriSource file ending.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the specified image is lossless; otherwise, <c>false</c>.
        /// </value>
        /// <exception cref="NotImplementedException">Unknown image type</exception>
        public bool IsLossless
        {
            get
            {
                var extension = Link.FileEndingFromUrl(this.ImageUrl.AbsoluteUri, false).ToLowerInvariant();
                switch (extension)
                {
                    case "jpg":
                    case "jpeg":
                        return false;

                    case "png":
                    case "gif":
                    case "bmp":
                    case "tif":
                    case "tiff":
                        return true;

                    default:
                        throw new NotImplementedException("Unknown image type");
                }
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the file system location.
        /// </summary>
        /// <value>
        /// The file system location.
        /// </value>
        public string TargetFileSystemLocation { get; }
    }
}