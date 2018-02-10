// <copyright file="Logger.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>
namespace GetThatPic.Data.IO
{
    /// <summary>
    /// Global logger.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Log(string message, bool newLine = true)
        {
            if (newLine) { 
                System.Console.WriteLine(message);
            }
            else
            {
                System.Console.Write(message);
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
    }
}
