// <copyright file="ImageDownloadFromMarkup.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>
namespace GetThatPic.Data.Configuration
{
    /// <summary>
    /// Models the configuration Node for an image to be loaded from data within a DOM Element.
    /// </summary>
    /// <seealso cref="GetThatPic.Data.Configuration.DomElementAccessor" />
    /// <seealso cref="IContentAccessor" />
    public class ImageDownloadFromMarkup : DomElementAccessor, IContentAccessor
    {
    }
}
