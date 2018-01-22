// <copyright file="DomElementAccessor.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GetThatPic.Data.Configuration
{
    /// <summary>
    /// Models a Dom Element's Accessor.
    /// </summary>
    public class  DomElementAccessor
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

        /// <summary>
        /// Gets or sets the replacement to manipulate the found string.
        /// </summary>
        /// <value>
        /// The replace.
        /// </value>
        public string Replace { get; set; } = "$1";
    }
}
