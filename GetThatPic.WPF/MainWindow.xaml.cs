using GetThatPic.WPF.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private string LogCallToAction = "Drop URLs here!";
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            state = new MainWindowState();
            LogTextBox.Text = LogCallToAction;
            // TODO: click image to open it in the file system.
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
            if(null == newPreviewItem)
            {
                return;
            }

            state.PreviewItem = newPreviewItem;

            PreviewImageName.Content = newPreviewItem.Name;
            PreviewImage.Source = newPreviewItem.Content;
        }

        private void ClearLogButton_Click(object sender, RoutedEventArgs e)
        {
            LogTextBox.Text = LogCallToAction;
        }

        /// <summary>
        /// Handles the Drop event of the LogTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private void DropUrl(object sender, DragEventArgs e)
        {
            string droppedUrl = (string)e.Data.GetData(DataFormats.Text);
            LogTextBox.Text += "\n" + droppedUrl;
            BitmapImage bitmap = state.LoadImageFromUrlToPreview(droppedUrl);
            
            PreviewImage.Source = bitmap;
            PreviewImageName.Content = droppedUrl;

            state.History.Push(new ImageEntry
            {
                Name = droppedUrl,
                FileSystemLocation = droppedUrl,
                Content = bitmap
            });
        }

        /// <summary>
        /// Handles the DragOver event of the LogTextBox control.
        /// Indicating that dropping is possible.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private void LogTextBox_DragOver(object sender, DragEventArgs e)
        {

        }

        /// <summary>
        /// Handles the DragLeave event of the LogTextBox control.
        /// Indicating that dropping is no longer possible.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private void LogTextBox_DragLeave(object sender, DragEventArgs e)
        {

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

            System.Diagnostics.Process.Start(state.History.LastWritten.FileSystemLocation);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
