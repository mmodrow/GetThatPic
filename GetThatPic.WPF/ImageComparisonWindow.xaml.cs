using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using GetThatPic.WPF.Models;

namespace GetThatPic.WPF
{
    /// <summary>
    /// Interaction logic for ImageComparisonWindow.xaml
    /// </summary>
    public partial class ImageComparisonWindow : Window
    {
        private readonly ImageEntry newImage;
        private readonly string targetPath;

        public ImageComparisonWindow(ImageEntry newImage, string[] diskImagePathes, string targetPath)
        {
            InitializeComponent();
            this.newImage = newImage;
            this.targetPath = targetPath;
            IEnumerable<BitmapImage> diskImages =
                diskImagePathes.Select(imagePath => new BitmapImage(new Uri(imagePath)));
            OldImage.Source = diskImages.FirstOrDefault();
            NewImage.Source = newImage.Bitmap;
        }

        /// <summary>
        /// Handles the OnClick event of the Download control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Download_OnClick(object sender, RoutedEventArgs e)
        {
            newImage.Save(targetPath, false);
            Close();
        }

        /// <summary>
        /// Handles the OnClick event of the Skip control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Skip_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
