﻿// <copyright file="DomElementAccessor.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GetThatPic.Data.IO;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GetThatPic.Data.Configuration
{
    /// <summary>
    /// Models a Dom Element's Accessor.
    /// </summary>
    public class  DomElementAccessor : IContentAccessor
    {
        /// <summary>
        /// The valid Target Types
        /// </summary>
        public enum TargetType
        {
            /// <summary>
            /// An attribute.
            /// </summary>
            Attribute,

            /// <summary>
            /// The inner HTML.
            /// </summary>
            Html,

            /// <summary>
            /// The inner text.
            /// </summary>
            Text,

            /// <summary>
            /// The full request URL.
            /// </summary>
            Url
        }

        /// <summary>
        /// Gets or sets the selector.
        /// </summary>
        /// <value>
        /// The selector.
        /// </value>
        public string Selector { get; set; }

        /// <summary>
        /// Gets or sets the name of the attribute to read.
        /// </summary>
        /// <value>
        /// The name of the attribute.
        /// </value>
        public string AttributeName { get; set; }

        /// <summary>
        /// Gets or sets the target type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [JsonConverter(typeof(StringEnumConverter))]
        public TargetType Type { get; set; }

        /// <summary>
        /// Gets or sets the pattern to manipulate the found string.
        /// </summary>
        /// <value>
        /// The pattern.
        /// </value>
        public Regex Pattern { get; set; } = new Regex("^(.*)$");

        /// <summary>
        /// Gets or sets the replacement to manipulate the found string.
        /// </summary>
        /// <value>
        /// The replace.
        /// </value>
        public string Replace { get; set; } = "$1";

        /// <summary>
        /// Returns true if all properties are valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if all properties are valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid => Enum.IsDefined(typeof(TargetType), Type) 
                               && null != Pattern 
                               && !string.IsNullOrWhiteSpace(Pattern.ToString()) 
                               && null != Replace 
                               && (TargetType.Url == Type || !string.IsNullOrWhiteSpace(Selector)) 
                               && !(TargetType.Attribute == Type && string.IsNullOrWhiteSpace(AttributeName));

        /// <summary>
        /// Gets the content specified by a DomElementAccessor from a given HtmlDocument.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="url">The URL.</param>
        /// <returns>
        /// The desired Content.
        /// </returns>
        public IList<string> GetContent(HtmlDocument doc, string url = null)
        {
            if (null == doc || !IsValid)
            {
                return new List<string>();
            }

            var nodes = TargetType.Url != Type && !string.IsNullOrWhiteSpace(Selector)
                ? doc.QuerySelectorAll(Selector)
                : null;
            IList<string> output = null;

            switch (Type)
            {
                case TargetType.Html:
                    output = nodes.Select(node => node.InnerHtml).ToList<string>();
                    break;

                case TargetType.Text:
                    output = nodes.Select(node => node.InnerText).ToList<string>();
                    break;

                case TargetType.Attribute:
                    output = nodes.Select(
                        node => node?.
                            Attributes?.
                            FirstOrDefault(attribute => AttributeName == attribute?.Name).Value)
                        .ToList();
                    break;

                case TargetType.Url:
                    output = new List<string>
                    {
                        Pattern.Replace(HttpRequester.PathAfterdomain.Replace(url, "$1"), Replace)
                    };
                    break;
            }

            output = output?.Select(item => Pattern.Replace(item, Replace)).ToList();
            return output ?? new List<string>();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Type + "_" + Selector + "_" + AttributeName;
        }
    }
}
