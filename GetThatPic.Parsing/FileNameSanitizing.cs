// <copyright file="FileNameSanitizing.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace GetThatPic.Parsing
{
    /// <summary>
    /// Handles sanitization of file names.
    /// </summary>
    public static class FileNameSanitizing
    {
        /*------------------------------Swear word Replacements------------------------------*/

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

        /*------------------------------Contraction Replacements------------------------------*/

        /// <summary>
        /// The will replacement.
        /// </summary>
        private const string WillReplacement = "$1$2 will";

        /// <summary>
        /// The is replacement.
        /// </summary>
        private const string IsReplacement = "$1$2 is";

        /// <summary>
        /// The am replacement.
        /// </summary>
        private const string AmReplacement = "$1$2 am";

        /// <summary>
        /// The have replacement.
        /// </summary>
        private const string HaveReplacement = "$1$2 have";

        /// <summary>
        /// The are replacement.
        /// </summary>
        private const string AreReplacement = "$1$2 are";

        /// <summary>
        /// The not replacement.
        /// </summary>
        private const string NotReplacement = "$1$2 not";

        /// <summary>
        /// The will not replacement.
        /// </summary>
        private const string WillNotReplacement = "$1$2ill not";

        /// <summary>
        /// The can not replacement.
        /// </summary>
        private const string CannotReplacement = "$1$2annot";

        /*------------------------------Misc Replacements------------------------------*/

        /// <summary>
        /// The replacement for illegal characters.
        /// </summary>
        private const string CharactersNeedingSubstitutionReplacement = "_";

        /*------------------------------Swear word Patterns------------------------------*/

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

        /*------------------------------Contraction Patterns------------------------------*/

        /// <summary>
        /// The will pattern.
        /// </summary>
        private static readonly Regex WillPattern = new Regex(@"(\s|^)((?:[Ss]?[Hh]e|[Tt]hey|[Ww]e|I))[`´'’‘]ll");

        /// <summary>
        /// The have pattern.
        /// </summary>
        private static readonly Regex HavePattern = new Regex(@"(\s|^)((?:[Tt]hey|[Ww]e|I|[Ss]hould))[`´'’‘]ve");

        /// <summary>
        /// The is pattern.
        /// </summary>
        private static readonly Regex IsPattern = new Regex(@"(\s|^)((?:[Ss]?[Hh]e))[`´'’‘]s");

        /// <summary>
        /// The am pattern.
        /// </summary>
        private static readonly Regex AmPattern = new Regex(@"(\s|^)(I)[`´'’‘]m");

        /// <summary>
        /// The are pattern.
        /// </summary>
        private static readonly Regex ArePattern = new Regex(@"(\s|^)([Tt]hey|[Ww]e|[Yy]ou)[`´'’‘]re");

        /// <summary>
        /// The not pattern.
        /// </summary>
        private static readonly Regex NotPattern = new Regex(@"(\s|^)([Ii]s|[Dd]oes|[Dd]o|[Ww]ere|[Ww]ould|[Cc]ould|[Ss]hould)n[`´'’‘]t");

        /// <summary>
        /// The will not pattern.
        /// </summary>
        private static readonly Regex WillNotPattern = new Regex(@"(\s|^)([Ww])on[`´'’‘]t");

        /// <summary>
        /// The cannot pattern.
        /// </summary>
        private static readonly Regex CannotPattern = new Regex(@"(\s|^)([Cc])an[`´'’‘]t");

        /*------------------------------Misc Patterns------------------------------*/

        /// <summary>
        /// The illegal character pattern.
        /// </summary>
        private static readonly Regex CharactersNeedingSubstitutionPattern = new Regex("[^a-zA-Z0-9_-]");

        /// <summary>
        /// The characters to drop pattern.
        /// </summary>
        private static readonly Regex CharactersToDropPattern = new Regex("[´`'\"’.‘]");

        /// <summary>
        /// Superfluous replacement character pattern..
        /// </summary>
        private static readonly Regex SubstituteGroupsPattern = new Regex(CharactersNeedingSubstitutionReplacement + "{2,}");

        /// <summary>
        /// The edge substitute pattern.
        /// </summary>
        private static readonly Regex EdgeSubstitutePattern = new Regex("^" + CharactersNeedingSubstitutionReplacement + "+|" + CharactersNeedingSubstitutionReplacement + "+$");

        /// <summary>
        /// The unix epoch point in time.
        /// </summary>
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);

        /// <summary>
        /// Gets the current unix time.
        /// </summary>
        /// <value>
        /// The current unix time.
        /// </value>
        public static string CurrentUnixTime => DateTime.UtcNow.Subtract(UnixEpoch).TotalSeconds.ToString(CultureInfo.InvariantCulture);

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
                output = RecreateSwearing(output);
                output = RecreateContractions(output);
                output = ReplaceUmlauts(output);
                
                output = RemoveDiacritics(output);

                output = ReplaceIllegalCharacters(output);

                output = StripSuperfluousReplacementCharacters(output);

                output = Crop(output);
            }

            return !string.IsNullOrWhiteSpace(output) ? output : CurrentUnixTime;
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
                ? WhitespacePattern.Replace(input, CharactersNeedingSubstitutionReplacement)
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

        /// <summary>
        /// Recreates the contractions in an input string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Input with it's contractions recreated.</returns>
        public static string RecreateContractions(string input)
        {
            string output = string.Empty;
            if (!string.IsNullOrWhiteSpace(input))
            {
                output = WillPattern.Replace(input, WillReplacement);
                output = IsPattern.Replace(output, IsReplacement);
                output = AmPattern.Replace(output, AmReplacement);
                output = HavePattern.Replace(output, HaveReplacement);
                output = ArePattern.Replace(output, AreReplacement);
                output = NotPattern.Replace(output, NotReplacement);
                output = WillNotPattern.Replace(output, WillNotReplacement);
                output = CannotPattern.Replace(output, CannotReplacement);
            }

            return output;
        }

        /// <summary>
        /// Removes the diacritics.
        /// Based on code found at https://weblogs.asp.net/fmarguerie/removing-diacritics-accents-from-strings
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// Input with all diacritics removed.
        /// </returns>
        public static string RemoveDiacritics(string input)
        {
            string output = string.Empty;
            if (!string.IsNullOrWhiteSpace(input))
            {
                string normalizedString = input.Normalize(NormalizationForm.FormD);
                StringBuilder stringBuilder = new StringBuilder();

                foreach (var c in normalizedString)
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    { 
                        stringBuilder.Append(c);
                    }
                }

                output = stringBuilder.ToString();
            }

            return output;
        }

        /// <summary>
        /// Replaces the umlauts by their legal replacement.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Input with umlauts replaced by corresponding combination of latin letters.</returns>
        public static string ReplaceUmlauts(string input)
        {
            string output = string.Empty;
            if (!string.IsNullOrWhiteSpace(input))
            {
                output = input.Replace("ö", "oe")
                                .Replace("ä", "ae")
                                .Replace("ü", "ue")
                                .Replace("Ö", "Oe")
                                .Replace("Ä", "Ae")
                                .Replace("Ü", "Ue")
                                .Replace("Æ", "Ae")
                                .Replace("æ", "ae")
                                .Replace("ø", "oe")
                                .Replace("Ø", "Oe");
            }

            return output;
        }

        /// <summary>
        /// Replaces and/or drops the illegal characters.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Input without illegal characters.</returns>
        public static string ReplaceIllegalCharacters(string input)
        {
            string output = string.Empty;
            if (!string.IsNullOrWhiteSpace(input))
            {
                output = CharactersToDropPattern.Replace(input, string.Empty);
                output = CharactersNeedingSubstitutionPattern.Replace(output, CharactersNeedingSubstitutionReplacement);
            }

            return output;
        }

        /// <summary>
        /// Strips the superfluous replacement characters.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Input without superfluous substitution characters.</returns>
        public static string StripSuperfluousReplacementCharacters(string input)
        {
            string output = string.Empty;
            if (!string.IsNullOrWhiteSpace(input))
            {
                output = SubstituteGroupsPattern.Replace(input, CharactersNeedingSubstitutionReplacement);
                output = EdgeSubstitutePattern.Replace(output, string.Empty);
            }

            return output;
        }

        /// <summary>
        /// Crops the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The input shortened to the maximum legal length.</returns>
        public static string Crop(string input)
        {
            string output = string.Empty;
            if (!string.IsNullOrWhiteSpace(input))
            {
                const int NtfsNameLimit = 255;
                const int UnixTimeStampWithSeprarator = 11;
                const int NormalFileEndingLength = 4;

                const int MaxLength = NtfsNameLimit - UnixTimeStampWithSeprarator - NormalFileEndingLength;

                output = input.Substring(0, MaxLength);
            }

            return output;
        }
    }
}
