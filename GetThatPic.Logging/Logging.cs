// <copyright file="Logging.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

namespace GetThatPic.Logging
{
    using System;

    /// <summary>
    /// Global Logging.
    /// </summary>
    public class Logging
    {
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="newLine">if set to <c>true</c> [new line].</param>
        public void Error(string message, bool newLine = true)
        {
            message = "Error: " + message;
            this.Log(message, newLine);
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="newLine">if set to <c>true</c> [new line].</param>
        public void Log(string message, bool newLine = true)
        {
            if (newLine)
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.Write(message);
            }
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="newLine">if set to <c>true</c> [new line].</param>
        public void Warn(string message, bool newLine = true)
        {
            message = "Warning: " + message;
            this.Log(message, newLine);
        }
    }
}