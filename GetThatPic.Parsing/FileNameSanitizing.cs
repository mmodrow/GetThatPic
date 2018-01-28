// <copyright file="FileNameSanitizing.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GetThatPic.Parsing
{
    /// <summary>
    /// Handles sanitization of file names.
    /// </summary>
    public static class FileNameSanitizing
    {
        /// <summary>
        /// The fuck replacement.
        /// </summary>
        private const string FuckReplacement = "$1uck";

        /// <summary>
        /// The fuck capslock replacement.
        /// </summary>
        private const string FuckCapslockReplacement = "FUCK";

        /// <summary>
        /// The shit replacement.
        /// </summary>
        private const string ShitReplacement = "$1$2hit$3";

        /// <summary>
        /// The shit full scramble replacement.
        /// </summary>
        private const string ShitFullScrambleReplacement = "$1shit$2";

        /// <summary>
        /// The shit capslock replacement.
        /// </summary>
        private const string ShitCapslockReplacement = "SHIT";

        /// <summary>
        /// The bitch replacement.
        /// </summary>
        private const string BitchReplacement = "$1itch";

        /// <summary>
        /// The bitch capslock replacement.
        /// </summary>
        private const string BitchCapslockReplacement = "BITCH";

        /// <summary>
        /// The replacement for illegal characters.
        /// </summary>
        private const string ReplacementForIllegalCharacters = "_";

        /// <summary>
        /// The whitespace pattern.
        /// </summary>
        private static readonly Regex WhitespacePattern = new Regex(@"\s+", RegexOptions.Multiline);

        /// <summary>
        /// The fuck pattern.
        /// </summary>
        private static readonly Regex FuckPattern = new Regex(@"(f|F)[^a-zA-Z](c|[^a-zA-Z])k");

        /// <summary>
        /// The fuck capslock pattern.
        /// </summary>
        private static readonly Regex FuckCapslockPattern = new Regex(@"F[^a-zA-Z](c|[^a-zA-Z])K");
        
        /// <summary>
        /// The shit pattern.
        /// </summary>
        private static readonly Regex ShitPattern = new Regex(@"(\s|^)(s|S)(?:[^a-zA-Z\s]|h)(?:[^a-zA-Z\s]|i)(?:[^a-zA-Z\s]|t)(\s|$)");
        
        /// <summary>
        /// The shit full scramble pattern
        /// </summary>
        private static readonly Regex ShitFullScramblePattern = new Regex(@"(\s|^)(?:[^a-zA-Z\s]{4})(\s|$)");

        /// <summary>
        /// The shit capslock pattern.
        /// </summary>
        private static readonly Regex ShitCapslockPattern = new Regex(@"(S)(?:[^a-zA-Z]|H)(?:[^a-zA-Z]|I)(?:[^a-zA-Z]|T)");

        /// <summary>
        /// The bitch pattern.
        /// </summary>
        private static readonly Regex BitchPattern = new Regex(@"(B|b)[^a-zA-Z]tch");

        /// <summary>
        /// The bitch capslock pattern.
        /// </summary>
        private static readonly Regex BitchCapslockPattern = new Regex(@"(B|b)[^a-zA-Z]TCH");
        
        /// <summary>
        /// Sanititzes the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Sanitized input.</returns>
        public static string Sanititze(string input)
        {
            string output = string.Empty;
            if (!string.IsNullOrWhiteSpace(input))
            {
                output = input.Trim();
                output = Whitespace(output);
            }

            return output;
        }

        /// <summary>
        /// Replaces Whitespaces within the input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// Input with whitespaces replaced.
        /// </returns>
        public static string Whitespace(string input)
        {
            return !string.IsNullOrWhiteSpace(input)
                ? WhitespacePattern.Replace(input, ReplacementForIllegalCharacters)
                : string.Empty;
        }
        
        /// <summary>
        /// Recreates the swearing when scrambled in the input.
        /// Swear words are fun!
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Input with scrambled swears recreated.</returns>
        public static string RecreateSwearing(string input)
        {
            string output = string.Empty;
            if (!string.IsNullOrWhiteSpace(input))
            {
                output = ShitPattern.Replace(input, ShitReplacement);
                output = ShitFullScramblePattern.Replace(output, ShitFullScrambleReplacement);
                output = ShitCapslockPattern.Replace(output, ShitCapslockReplacement);
                output = FuckPattern.Replace(output, FuckReplacement);
                output = FuckCapslockPattern.Replace(output, FuckCapslockReplacement);
                output = BitchPattern.Replace(output, BitchReplacement);
                output = BitchCapslockPattern.Replace(output, BitchCapslockReplacement);
            }

            return output;
        }
    }
}
