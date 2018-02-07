// <copyright file="MainWindow.xaml.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using GetThatPic.Data.IO;
using GetThatPic.Parsing;
using GetThatPic.Parsing.Models;
using GetThatPic.WPF.Models;
using SixLabors.ImageSharp;
using WpfAnimatedGif;
using Clipboard = System.Windows.Clipboard;
using DragEventArgs = System.Windows.DragEventArgs;

namespace GetThatPic.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The state.
        /// </summary>
        private readonly MainWindowState state;

        /// <summary>
        /// The log call to action for the initializing of the drop-target-log TextBox.
        /// </summary>
        private string logCallToAction = "Drop URLs here!";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            state = new MainWindowState();
            LogTextBox.Text = logCallToAction;

            // TODO: Click image to open it in the file system.
            // TODO: Download.
            // TODO: Async status updates on big downloads.
            // TODO: Error Notification.
            // TODO: Check download target directory for file before saving copy. (Check by name and/or identity)
            // TODO: Command line Interface?
            // TODO: Login functionality for private/mature content.
            // TODO: Transpile old XML configs to JSON.
        }

        /// <summary>
        /// Previews the previous image.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void PreviewPreviousImage(object sender, RoutedEventArgs e)
        {
            ImageEntry newPreviewItem = state.History.Previous;
            UpdatePreview(newPreviewItem);
        }

        /// <summary>
        /// Previews the next image.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void PreviewNextImage(object sender, RoutedEventArgs e)
        {
            ImageEntry newPreviewItem = state.History.Next;
            UpdatePreview(newPreviewItem);
        }

        /// <summary>
        /// Updates the preview.
        /// </summary>
        /// <param name="newPreviewItem">The new preview item.</param>
        private void UpdatePreview(ImageEntry newPreviewItem)
        {
            if (null == newPreviewItem)
            {
                return;
            }

            state.PreviewItem = newPreviewItem;

            PreviewImageName.Text = newPreviewItem.Name;
            if (newPreviewItem.FileSystemLocation?.EndsWith(".gif") ?? false)
            {
                // FIXME: find out how to bind Image to Image.
                ////ImageBehavior.SetAnimatedSource(PreviewImage, newPreviewItem.Content);
            }
            else
            {
                ImageBehavior.SetAnimatedSource(PreviewImage, null);

                // FIXME: Determine correct colour format/palette and other values from Image.
                BitmapSource source = BitmapSource.Create(
                    newPreviewItem.Content.Width, 
                    newPreviewItem.Content.Height,
                    72.0, 
                    72.0, 
                    PixelFormats.Bgra32, 
                    BitmapPalettes.WebPalette, 
                    newPreviewItem.Content.SavePixelData(), 
                    newPreviewItem.Content.Width*4);
                    PreviewImage.Source = source;
            }
        }

        /// <summary>
        /// Handles the Click event of the ClearLogButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ClearLogButton_Click(object sender, RoutedEventArgs e)
        {
            LogTextBox.Text = logCallToAction;
        }

        /// <summary>
        /// Handles the Drop event of the LogTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private async void DropUrl(object sender, System.Windows.DragEventArgs e)
        {
            // TODO: Migrate to using Link-Class here
            string droppedUrl = (string)e.Data.GetData(System.Windows.DataFormats.Text);
            LogTextBox.Text += "\n" + droppedUrl;
            IList<ImageEntry> images = await state.LinkParser.GetImages(droppedUrl);

            foreach (ImageEntry image in images)
            {
                UpdatePreview(image);

                state.History.Push(image);
            }
        }

        /// <summary>
        /// Handles the DragOver event of the LogTextBox control.
        /// Indicating that dropping is possible.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private void LogTextBox_DragOver(object sender, DragEventArgs e)
        {
            // Method intentionally left empty - for now.
        }

        /// <summary>
        /// Handles the DragLeave event of the LogTextBox control.
        /// Indicating that dropping is no longer possible.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private void LogTextBox_DragLeave(object sender, System.Windows.DragEventArgs e)
        {
            // Method intentionally left empty - for now.
        }

        /// <summary>
        /// Handles the PreviewDragOver event of the LogTextBox control.
        /// Needed to make the Log Text Box droppable.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private void EnableDropping(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// Handles the Click event of the CopyLastFilePathButton control.
        /// Copies the last downloaded file's path to the clipboard.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CopyLastFilePathButton_Click(object sender, RoutedEventArgs e)
        {
            if (null == state.History?.LastWritten?.FileSystemLocation)
            {
                return;
            }

            Clipboard.SetText(state.History.LastWritten.FileSystemLocation);
        }

        /// <summary>
        /// Handles the Click event of the OpenLastFileButton control.
        /// Opens the last downloaded file with the default means.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OpenLastFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (null == state.History?.LastWritten?.FileSystemLocation)
            {
                return;
            }

            System.Diagnostics.Process.Start((string)state.History.LastWritten.FileSystemLocation);
        }

        /// <summary>
        /// Handles the RequestNavigate event of the Hyperlink control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RequestNavigateEventArgs"/> instance containing the event data.</param>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
