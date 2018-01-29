// <copyright file="DomElementAccessor.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            Text
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
        public string PatternString
        {
            set => Pattern = new Regex(value);
        }

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
        public bool IsValid => Enum.IsDefined(typeof(TargetType), Type) && null != Pattern && !string.IsNullOrWhiteSpace(Pattern.ToString()) &&
                               !string.IsNullOrWhiteSpace(Replace) && !string.IsNullOrWhiteSpace(Selector) &&
                               !(TargetType.Attribute == Type && string.IsNullOrWhiteSpace(AttributeName));

        /// <summary>
        /// Gets the content specified by a DomElementAccessor from a given HtmlDocument.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <returns>The desired Content.</returns>
        public IList<string> GetContent(HtmlDocument doc)
        {
            if (null == doc || !IsValid)
            {
                return new List<string>();
            }

            IList<HtmlNode> nodes = doc.QuerySelectorAll(Selector);
            IList<string> output = null;

            switch (Type)
            {
                case DomElementAccessor.TargetType.Html:
                    output = nodes.Select(node => node.InnerHtml).ToList<string>();
                    break;

                case DomElementAccessor.TargetType.Text:
                    output = nodes.Select(node => node.InnerText).ToList<string>();
                    break;

                case DomElementAccessor.TargetType.Attribute:
                    output = nodes.Select(
                        node => node.Attributes.First(
                            attribute => AttributeName == attribute.Name).Value).ToList<string>();
                    break;
            }

            output = output?.Select(item => Pattern.Replace(item, Replace)).ToList();
            return output ?? new List<string>();
        }
    }
}
