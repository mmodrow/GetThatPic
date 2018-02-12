// <copyright file="ImageComparisonWindow.xaml.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>
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
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class ImageComparisonWindow : Window
    {
        /// <summary>
        /// The new image.
        /// </summary>
        private readonly ImageEntry newImage;
        
        /// <summary>
        /// The target path.
        /// </summary>
        private readonly string targetPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageComparisonWindow"/> class.
        /// </summary>
        /// <param name="newImage">The new image.</param>
        /// <param name="diskImagePathes">The disk image pathes.</param>
        /// <param name="targetPath">The target path.</param>
        public ImageComparisonWindow(ImageEntry newImage, string[] diskImagePathes, string targetPath)
        {
            InitializeComponent();
            this.newImage = newImage;
            this.targetPath = targetPath;
            IEnumerable<BitmapImage> diskImages =
                diskImagePathes.Select(imagePath => new BitmapImage(new Uri(imagePath)));
            OldImage.Source = diskImages.FirstOrDefault();
            NewImage.Source = newImage.Bitmap;
            Focus();
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
