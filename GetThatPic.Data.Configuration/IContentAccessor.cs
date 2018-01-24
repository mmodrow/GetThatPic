// <copyright file="IContentAccessor.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

using System.Collections.Generic;
using HtmlAgilityPack;

namespace GetThatPic.Data.Configuration
{
    /// <summary>
    /// Interface for any kind of Image Download Instruction.
    /// </summary>
    public interface IContentAccessor
    {        
        /// <summary>
        /// Gets the content specified by a DomElementAccessor from a given HtmlDocument.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <returns>The desired Content.</returns>
        IList<string> GetContent(HtmlDocument doc);
    }
}