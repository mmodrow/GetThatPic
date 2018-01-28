// <copyright file="DefaultConfig.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GetThatPic.Data.Configuration
{
    /// <summary>
    /// Holds the default configuration.
    /// </summary>
    public static class DefaultConfig
    {
        /// <summary>
        /// The domains.
        /// </summary>
        public static readonly IList<Domain> Domains = new List<Domain>
        {
            new Domain
            {
                Name = "dilbert.com",
                Url = "http://dilbert.com",
                Path = new Regex("^/strip/((?:[0-9]+-?)+)$"),
                Images = new List<IContentAccessor>()
                {
                    new ImageDownloadFromMarkup()
                    {
                        Type = DomElementAccessor.TargetType.Attribute,
                        AttributeName = "src",
                        Selector = ".img-comic"
                    }
                },
                FileNameFragments = new List<IContentAccessor>()
                {
                    new ImageDownloadFromMarkup()
                    {
                        Type = DomElementAccessor.TargetType.Text,
                        Selector = "title",
                        Pattern = new Regex(@"^.*?-  Dilbert Comic Strip on (\d{4}-\d{2}-\d{2}).*$")
                    },
                    new ImageDownloadFromMarkup()
                    {
                        Type = DomElementAccessor.TargetType.Text,
                        Selector = ".comic-title-name"
                    }
                },
                FileNameFragmentDelimiter = "_-_"
            },
            new Domain
            {
                Name = "www.schisslaweng.net",
                Url = "https://www.schisslaweng.net",
                Path = new Regex("^/(.*?)/.*$"),
                Images = new List<IContentAccessor>()
                {
                    new ImageDownloadFromMarkup()
                    {
                        Type = DomElementAccessor.TargetType.Attribute,
                        AttributeName = "src",
                        Selector = ".gallery-item img"
                    }
                },
                FileNameFragments = new List<IContentAccessor>()
                {
                    new ImageDownloadFromMarkup()
                    {
                        Type = DomElementAccessor.TargetType.Attribute,
                        AttributeName = "content",
                        Selector = @"meta[name=""shareaholic:article_published_time""]",
                        Pattern = new Regex(@"^(\d{4}-\d{2}-\d{2}).*$")
                    },
                    new ImageDownloadFromMarkup()
                    {
                        Type = DomElementAccessor.TargetType.Text,
                        Selector = "h1"
                    }
                },
                FileNameFragmentDelimiter = "_-_"
            }
        };
    }
}