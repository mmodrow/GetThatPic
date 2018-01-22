// <copyright file="Link.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GetThatPic.Data.Configuration;

namespace GetThatPic.Parsing
{
    /// <summary>
    /// Parses an url for the correct domain configuration node.
    /// From Domain and url other data can be retreived.
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Link"/> class.
        /// </summary>
        public Link()
        {
            InitializeConfig();
        }

        /// <summary>
        /// Gets the domain configruation nodes.
        /// </summary>
        /// <value>
        /// The domains.
        /// </value>
        public IList<Domain> Domains { get; } = new List<Domain>();

        /// <summary>
        /// Initializes the configuration.
        /// </summary>
        /// <param name="clearFirst">if set to <c>true</c> [clear first].</param>
        public void InitializeConfig(bool clearFirst = true)
        {
            if (clearFirst)
            {
                Domains.Clear();
            }

            Domains.Add(new Domain
            {
                Name = "dilbert.com",
                Url = "http://dilbert.com",
                Path = new Regex("^/strip/((?:[0-9]+-?)+)$")
            });
        }

        /// <summary>
        /// Identifies the domain fro a given uri.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <returns>The identified Domain configuration node or null if none applies.</returns>
        public Domain IdentifyDomain(Uri link)
        {
            return Domains.FirstOrDefault(domain => domain.Url == link.Host);
        }
    }
}
