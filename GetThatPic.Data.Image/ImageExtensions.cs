// <copyright file="ImageExtensions.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

namespace GetThatPic.Data.Image
{
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.PixelFormats;

    /// <summary>
    /// New functionality for the BitmapImage class.
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// Determines whether the source is equal to source.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="isLossless">
        /// The is Lossless.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified target is equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEqual(this Image source, Image target, bool isLossless)
        {
            if (source == null || target == null)
            {
                return source == null && target == null;
            }

            bool metaEqual = source.Height == target.Height && source.Width == target.Width
                                                            && source.PixelType.Equals(target.PixelType);
            if (!metaEqual)
            {
                return false;
            }

            return !isLossless || CompareContent(source, target);
        }

        /// <summary>
        /// The compare content.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool CompareContent(Image source, Image target)
        {
            var sourceFrameCount = source.Frames.Count;
            if (sourceFrameCount != target.Frames.Count)
            {
                return false;
            }

            for (int i = 0; i < sourceFrameCount; i++)
            {
                var sourceFrame = source.Frames[i];
                var targetFrame = target.Frames[i];
                if (!AreFramesEqual(sourceFrame, targetFrame))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// The are frames equal.
        /// </summary>
        /// <param name="sourceFrame">
        /// The source frame.
        /// </param>
        /// <param name="targetFrame">
        /// The target frame.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool AreFramesEqual(ImageFrame sourceFrame, ImageFrame targetFrame)
        {
            // TODO: Find a way to actually compare them without getting too specific...
            return sourceFrame.Equals(targetFrame);
        }
    }
}