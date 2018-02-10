using System;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using GetThatPic.Data.IO;
using GetThatPic.Parsing;

namespace GetThatPic.WPF.Extensions
{
    public static class BitmapImageExtensions
    {
        public static bool IsEqual(this BitmapImage image1, BitmapImage image2)
        {
            if (image1 == null || image2 == null)
            {
                return image1 == null && image2 == null;
            }

            bool metaEqual = image1.PixelHeight == image2.PixelHeight
                   && image1.PixelWidth == image2.PixelWidth
                   && Math.Abs(image1.DpiX - image2.DpiX) < 1
                   && Math.Abs(image1.DpiY - image2.DpiY) < 1
                   && image1.Format.Equals(image2.Format);
            if (metaEqual)
            {
                if (image2.IsLossless())
                {
                    return image1.ToBytes().SequenceEqual(image2.ToBytes());
                }

                return true;
            }

            return false;
        }

        public static byte[] ToBytes(this BitmapImage image)
        {
            byte[] data = { };
            if (image != null)
            {
                try
                {
                    var encoder = new BmpBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        data = ms.ToArray();
                    }
                    return data;
                }
                catch (Exception ex)
                {
                    Logger.Log("Bitmap could not be read");
                }
            }
            return data;
        }

        public static float Similarity(this BitmapImage image1, BitmapImage image2, double threshold = 0)
        {
            byte[] image1Bytes = image1.ToBytes();
            byte[] image2Bytes = image2.ToBytes();

            int smallerSize = Math.Min(image1Bytes.Length, image2Bytes.Length);
            int biggerSize = Math.Max(image1Bytes.Length, image2Bytes.Length);
            int differentBytes = 0;
            for (int i = 0; i < smallerSize; i++)
            {
                int offset = Math.Abs(image1Bytes[i] - image2Bytes[i]);
                if ((float)offset / byte.MaxValue > threshold)
                {
                    differentBytes++;
                }
            }

            float totalDifference =  differentBytes == 0 ? 1 : 1 - (float)differentBytes / (float)smallerSize;
            return totalDifference * smallerSize / biggerSize;
        }

        public static bool IsLossless(this BitmapImage image)
        {
            string extension = Link.FileEndingFromUrl(image.UriSource.ToString(), false).ToLowerInvariant();
            switch (extension)
            {
                case "jpg":
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
}
