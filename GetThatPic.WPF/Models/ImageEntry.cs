using System.Windows.Media.Imaging;

namespace GetThatPic.WPF.Models
{
    /// <summary>
    /// Models all data needed to know about a previously downloaded image.
    /// </summary>
    class ImageEntry
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public BitmapImage Content { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the file system location.
        /// </summary>
        /// <value>
        /// The file system location.
        /// </value>
        public string FileSystemLocation { get; set; }
    }
}
