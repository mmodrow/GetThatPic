using System;
using System.Drawing;
using System.Windows.Media.Imaging;
using GetThatPic.Parsing.Models;

namespace GetThatPic.WPF.Models
{
    public class ImageEntry
    {
        public ImageMetaData MetaData { get; set; }

        private BitmapImage bitmap;

        public BitmapImage Bitmap => bitmap ?? (bitmap = new BitmapImage(new Uri(MetaData.ImageUrl)));
    }
}
