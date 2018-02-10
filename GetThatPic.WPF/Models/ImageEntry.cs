// <copyright file="ImageEntry.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using GetThatPic.Data.IO;
using GetThatPic.Parsing;
using GetThatPic.Parsing.Models;
using GetThatPic.WPF.Extensions;

namespace GetThatPic.WPF.Models
{
    /// <summary>
    /// Combines ImageMetaData with a BitmapImage.
    /// </summary>
    public class ImageEntry
    {
        /// <summary>
        /// Pattern to insert text before a file ending.
        /// </summary>
        private static readonly Regex BeforeFileEnding = new Regex(@"^(.+)(\..+)$");

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
        /// TODO: Status update in Frontend.
        /// See: https://stackoverflow.com/questions/4161359/save-bitmapimage-to-file
        /// </summary>
        /// <value>
        /// The bitmap.
        /// </value>
        public BitmapImage Bitmap => bitmap ?? (bitmap = new BitmapImage(new Uri(MetaData.ImageUrl)));

        /// <summary>
        /// Saves this image to the specified target path.
        /// </summary>
        /// <param name="targetPath">The target path.</param>
        public void Save(string targetPath = null)
        {
            if (string.IsNullOrWhiteSpace(targetPath))
            {
                targetPath = MetaData.TargetFileSystemLocation;
            }

            // Create missing directory.
            string targetDirectory = Path.GetDirectoryName(targetPath);
            if (!string.IsNullOrWhiteSpace(targetDirectory) && !Directory.Exists(targetDirectory))
            {
                // TODO: Log directory creation.
                Directory.CreateDirectory(targetDirectory);
            }

            if (File.Exists(targetPath))
            {
                BitmapImage diskImage = new BitmapImage(new Uri(targetPath));

                bool equal = Bitmap.IsEqual(diskImage);
                if (equal && !diskImage.IsLossless())
                {
                    float similarity = diskImage.Similarity(Bitmap, 0.02);
                    equal = similarity > 0.9;
                }

                if (!equal)
                {
                    targetPath = BeforeFileEnding.Replace(targetPath, "$1_" + Sanitizing.CurrentUnixTime + "$2");
                }

                Logger.Log("Existing image is " + (equal ? string.Empty : "not ") + "equal to the new one.");
            }

            SaveImageToDisk(targetPath);
        }

        /// <summary>
        /// Saves the image to disk.
        /// </summary>
        /// <param name="targetPath">The target path.</param>
        /// <exception cref="NotImplementedException">Unknown image type</exception>
        private void SaveImageToDisk(string targetPath)
        {
            string ending = Link.FileEndingFromUrl(targetPath, false).Trim().ToLowerInvariant();

            try
            {
                FileStream filestream = new FileStream(targetPath, FileMode.Create);
                BitmapEncoder encoder;
                switch (ending)
                {
                    case "jpg":
                    case "jpeg":
                        encoder = new JpegBitmapEncoder();
                        break;
                    case "gif":
                        encoder = new GifBitmapEncoder();
                        break;
                    case "png":
                        encoder = new PngBitmapEncoder();
                        break;
                    case "tif":
                    case "tiff":
                        encoder = new TiffBitmapEncoder();
                        break;
                    default:
                        throw new NotImplementedException("Unknown image type");
                }
                
                // FIXME: Loads only one frame of animated GIF
                encoder.Frames.Add(BitmapFrame.Create(Bitmap));

                encoder.Save(filestream);

                filestream.Close();

                Logger.Log("Download Success: " + targetPath);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }
    }
}
