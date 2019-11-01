// <copyright file="ImageEntry.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

namespace GetThatPic.Data.Image
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using GetThatPic.Logging;
    using GetThatPic.Parsing;

    using SixLabors.ImageSharp;

    /// <summary>
    /// The download failed.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <returns>The inserted message.</returns>
    public delegate string DownloadFailed(string message);

    /// <summary>
    /// The download's progress was updated.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <returns>The inserted message.</returns>
    public delegate string DownloadProgressUpdate(DownloadProgressChangedEventArgs message);

    /// <summary>
    /// The download was completed.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <returns>The inserted message.</returns>
    public delegate string DownloadComplete(string message);

    /// <summary>
    /// The local saving failed.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <returns>The inserted message.</returns>
    public delegate string SavingFailed(string message);

    /// <summary>
    /// The local saving was completed.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <returns>The inserted message.</returns>
    public delegate string SavingComplete(string message);

    /// <summary>
    /// Combines ImageMetaData with a BitmapImage.
    /// </summary>
    public class ImageEntry : IDisposable
    {
        /// <summary>
        /// Pattern to insert text before a file ending.
        /// </summary>
        private static readonly Regex BeforeFileEnding = new Regex(@"^(.+)(\.)([^\.]+)$");

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly Logging log = new Logging();

        /// <summary>
        /// The image.
        /// </summary>
        private Image? image;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageEntry"/> class.
        /// </summary>
        /// <param name="metaData">
        /// The meta data.
        /// </param>
        public ImageEntry(ImageMetaData metaData)
        {
            this.MetaData = metaData;
        }

        /// <summary>
        /// The download failed.
        /// </summary>
        public event DownloadFailed? DownloadFailed;

        /// <summary>
        /// The download was completed.
        /// </summary>
        public event DownloadComplete? DownloadComplete;

        /// <summary>
        /// The download continues and its progress is updated.
        /// </summary>
        public event DownloadProgressUpdate? DownloadProgressUpdate;

        /// <summary>
        /// The local saving failed.
        /// </summary>
        public event SavingFailed? SavingFailed;

        /// <summary>
        /// The local saving was completed.
        /// </summary>
        public event SavingComplete? SavingComplete;

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public Image? Image => this.image;

        /// <summary>
        /// Gets or sets the meta data.
        /// </summary>
        /// <value>
        /// The meta data.
        /// </value>
        public ImageMetaData MetaData { get; set; }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.image?.Dispose();
        }

        /// <summary>
        /// Load the Image from its remote location.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<bool> Load()
        {
            using (var client = new WebClient())
            {
                var uri = this.MetaData.ImageUrl;
                client.DownloadDataCompleted += (sender, args) =>
                    this.log.Log($"Download of {uri.AbsoluteUri} completed.");
                client.DownloadProgressChanged += (sender, args) =>
                    {
                        var message = $"Download of {uri.AbsoluteUri} continues. {args.BytesReceived} of {args.TotalBytesToReceive} loaded. That's {args.ProgressPercentage}%.";
                        this.log.Log(message);
                        this.DownloadProgressUpdate?.Invoke(args);
                    };

                return await this.DownloadImage(uri, client).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Saves this image to the specified target path.
        /// </summary>
        /// <param name="askIfEqual">
        /// if set to <c>true</c> prompting for approval before downloading possibly identical image.
        /// </param>
        /// <returns>
        /// The success of the saving process.
        /// </returns>
        public bool Save(bool askIfEqual = true)
        {
            string targetPath = this.MetaData.TargetFileSystemLocation;
            if (null == this.Image)
            {
                this.SavingFailed?.Invoke($"No image was loaded from {targetPath}, that could be saved to disk.");
                return false;
            }

            // Create missing directory.
            var targetDirectory = this.CreateTargetDirectoryIfNecessary(targetPath);

            string searchPattern = BeforeFileEnding.Replace(Path.GetFileName(targetPath) ?? string.Empty, "$1*$2$3");

            string[] possibleFallbackNames = Directory.EnumerateFiles(targetDirectory, searchPattern).ToArray();
            bool sameNameExists = File.Exists(targetPath);
            if (!sameNameExists && !possibleFallbackNames.Any())
            {
                // TODO: Save it to disc.
                ////this.SaveImageToDisk(targetPath);
                SavingComplete?.Invoke($"Saving of {this.MetaData.ImageUrl} to {targetPath} was successful.");
                return true;
            }

            string diskPath;
            if (sameNameExists && new FileInfo(targetPath).Length > 0)
            {
                this.log.Log("1 direct match found.");
                diskPath = targetPath;
                possibleFallbackNames = new[] { targetPath };
            }
            else
            {
                if (possibleFallbackNames.Length > 1)
                {
                    this.log.Log(possibleFallbackNames.Length + " indirect matches found.");
                }
                else
                {
                    this.log.Log("1 indirect match found.");
                }

                diskPath = possibleFallbackNames.FirstOrDefault(path => new FileInfo(path).Length > 0)
                           ?? string.Empty;
            }

            try
            {
                Image diskImage = Image.Load(diskPath);
                bool equal = this.Image.IsEqual(diskImage, this.MetaData.IsLossless);
                float similarity = -1;

                this.log.Log(
                    "Existing image " + targetPath + " is " + (equal ? string.Empty : "not ")
                    + "equal to the new one.");
                if (similarity > -1)
                {
                    this.log.Log("Similarity: " + similarity);
                }

                if (!equal || !askIfEqual)
                {
                    targetPath = BeforeFileEnding.Replace(targetPath, "$1_" + Sanitizing.CurrentUnixTime + "$2");
                }

                if (askIfEqual)
                {
                    // ImageComparisonWindow compare = new ImageComparisonWindow(
                    // this,
                    // possibleFallbackNames,
                    // targetPath);
                    // compare.Show();
                    return false;
                }
            }
            catch (Exception e)
            {
                this.log.Error("Image rejected. Probably the target is broken? " + e.Message);
            }

            return true;
        }

        /// <summary>
        /// Download image into <see cref="image"/>.
        /// </summary>
        /// <param name="uri">
        /// The uri.
        /// </param>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task<bool> DownloadImage(Uri uri, WebClient client)
        {
            this.log.Log($"Starting download of {uri.AbsoluteUri}.");
            try
            {
                var content = await client.DownloadDataTaskAsync(uri).ConfigureAwait(false);
                using (var stream = new MemoryStream(content))
                {
                    this.image = Image.Load(stream);
                }

                this.DownloadComplete?.Invoke($"The download of {uri.AbsoluteUri} is complete.");
            }
            catch (WebException ex)
            {
                var message = $"Error while downloading {uri.AbsoluteUri}. Status: {ex.Status}";
                this.OnDownloadFailed(message);
                return false;
            }
            catch (NotSupportedException)
            {
                var message = $"Error while reading {uri.AbsoluteUri} to an image. Not supported.";
                this.OnDownloadFailed(message);
                return false;
            }
            catch (UnknownImageFormatException)
            {
                var message = $"Error while reading {uri.AbsoluteUri} to an image. Image format is unknown.";
                this.OnDownloadFailed(message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Handles a failing download.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        private void OnDownloadFailed(string message)
        {
            this.log.Error(message);
            this.DownloadFailed?.Invoke(message);
        }

        /// <summary>
        /// Creates the target directory if necessary.
        /// </summary>
        /// <param name="targetPath">
        /// The target path.
        /// </param>
        /// <returns>
        /// The target directory path.
        /// </returns>
        private string CreateTargetDirectoryIfNecessary(string targetPath)
        {
            string targetDirectory = Path.GetDirectoryName(targetPath) ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(targetDirectory) && !Directory.Exists(targetDirectory))
            {
                // TODO: Log directory creation.
                Directory.CreateDirectory(targetDirectory);
            }

            return targetDirectory;
        }


        ///// <summary>
        ///// Handles the ImageDownloadProgress event of the ImagEntry control.
        ///// </summary>
        ///// <param name="sender">The source of the event.</param>
        ///// <param name="e">The <see cref="System.Windows.Media.Imaging.DownloadProgressEventArgs"/> instance containing the event data.</param>
        // private void ImagEntry_ImageDownloadProgress(object sender, DownloadProgressEventArgs e)
        // {
        // this.State.IsDownloading = true;
        // this.Window.DownloadProgress.Value = e.Progress;
        // if (this.previousProgressStep != 100 && this.previousProgressStep != 0 && e.Progress >= 100)
        // {
        // this.log.Log("Finshed " + this.MetaData.Name + "!");
        // }

        // this.previousProgressStep = e.Progress;
        // }

        ///// <summary>
        ///// Saves the image to disk.
        ///// </summary>
        ///// <param name="targetPath">The target path.</param>
        ///// <exception cref="System.NotImplementedException">Unknown image type</exception>
        // private void SaveImageToDisk(string targetPath)
        // {
        // string ending = Link.FileEndingFromUrl(targetPath, false).Trim().ToLowerInvariant();

        // try
        // {
        // FileStream filestream = new FileStream(targetPath, FileMode.Create);
        // BitmapEncoder encoder;
        // switch (ending)
        // {
        // case "jpg":
        // case "jpeg":
        // encoder = new JpegBitmapEncoder();
        // break;
        // case "gif":
        // encoder = new GifBitmapEncoder();
        // break;
        // case "png":
        // encoder = new PngBitmapEncoder();
        // break;
        // case "tif":
        // case "tiff":
        // encoder = new TiffBitmapEncoder();
        // break;
        // default:
        // throw new NotImplementedException("Unknown image type");
        // }

        // // FIXME: Loads only one frame of animated GIF
        // encoder.Frames.Add(BitmapFrame.Create(this.Bitmap));

        // encoder.Save(filestream);

        // filestream.Close();

        // this.log.Log("Download Success: " + targetPath);
        // }
        // catch (Exception e)
        // {
        // this.log.Error(e.Message);
        // }
        // }
    }
}