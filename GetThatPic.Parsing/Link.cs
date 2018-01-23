// <copyright file="Link.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GetThatPic.Data.Configuration;
using GetThatPic.Data.IO;

namespace GetThatPic.Parsing
{
    /// <summary>
    /// Parses an url for the correct domain configuration node.
    /// From Domain and url other data can be retreived.
    /// TODO: Include http://html-agility-pack.net/?z=codeplex
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Link" /> class.
        /// </summary>
        /// <param name="autoInitialize">if set to <c>true</c> the config gets automatically initialized.</param>
        public Link(bool autoInitialize = true)
        {
            if (autoInitialize)
            {
                InitializeConfig();
            }
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
        /// <param name="clearFirst">if set to <c>true</c> clear the config before initializing.</param>
        /// <param name="domains">The domains.</param>
        public void InitializeConfig(bool clearFirst = true, IList<Domain> domains = null)
        {
            if (clearFirst)
            {
                Domains.Clear();
            }

            if (null == domains)
            {
                Domains.Add(new Domain
                {
                    Name = "dilbert.com",
                    Url = "http://dilbert.com",
                    Path = new Regex("^/strip/((?:[0-9]+-?)+)$")
                });
            }
            else
            {
                foreach (Domain domain in domains)
                {
                    Domains.Add(domain);
                }
            }
        }

        /// <summary>
        /// Identifies the domain fro a given uri.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <returns>The identified Domain configuration node or null if none applies.</returns>
        public Domain IdentifyDomain(string link)
        {
            if (string.IsNullOrWhiteSpace(link) || !Domains.Any())
            {
                return null;
            }

            string domain = HttpRequester.ProtocolAndDomain.Replace(link, "$1");
            string path = HttpRequester.PathAfterdomain.Replace(link, "$1");
            IList<Domain> matchingDomains = Domains.Where(d => d.Url == domain && d.Path.IsMatch(path)).ToList();

            return matchingDomains.FirstOrDefault();
        }
    }
}
