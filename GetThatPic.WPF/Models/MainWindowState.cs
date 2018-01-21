using System;
using System.Windows.Media.Imaging;
using GetThatPic.Data.Struct;

namespace GetThatPic.WPF.Models
{
    /// <summary>
    /// Main Window State Model - Keeps track of what is happening.
    /// </summary>
    class MainWindowState
    {
        /// <summary>
        /// The history buffer.
        /// </summary>
        public RingBuffer<ImageEntry> History = new RingBuffer<ImageEntry>(50);

        public ImageEntry PreviewItem { get; set; }

        /// <summary>
        /// Loads the image from URL to preview.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public BitmapImage LoadImageFromUrlToPreview(string url)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(url, UriKind.Absolute);
            bitmap.EndInit();

            return bitmap;
        }
    }
}
