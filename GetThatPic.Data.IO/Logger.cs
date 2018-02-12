// <copyright file="Logger.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

using System;
using System.Management;
using System.Net;

namespace GetThatPic.Data.IO
{
    /// <summary>
    /// Global logger.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Occurs when loggin to GUI.
        /// </summary>
        public static event Action<string> LogToGui;

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="newLine">if set to <c>true</c> [new line].</param>
        public static void Log(string message, bool newLine = true)
        {
            if (newLine)
            {
                System.Console.WriteLine(message);
                UpdateGuiLog("\n" + message);
            }
            else
            {
                System.Console.Write(message);
                UpdateGuiLog(message);
            }
        }

        /// <summary>
        /// Logs the specified message as error.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Error(string message)
        {
            System.Console.WriteLine("Error: " + message);
        }

        /// <summary>
        /// Updates the GUI log.
        /// </summary>
        /// <param name="message">The message.</param>
        private static void UpdateGuiLog(string message)
        {
            LogToGui?.Invoke(message);
        }
    }
}
