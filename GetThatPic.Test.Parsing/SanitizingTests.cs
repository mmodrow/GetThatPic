// <copyright file="SanitizingTests.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

using GetThatPic.Parsing;
using Xunit;

namespace GetThatPic.Test.Parsing
{
    /// <summary>
    /// Tests the functionality of the Sanitizing class.
    /// </summary>
    public class SanitizingTests
    {
        /// <summary>
        /// Tests Whitespaces with whitespaces in the middle.
        /// </summary>
        [Fact]
        public void Whitespace_Middle()
        {
            Assert.Equal("There_are_things_in_this_world", Sanitizing.Whitespace("There are things  in this world"));
        }

        /// <summary>
        /// Tests Whitespaces with whitespaces in the front.
        /// </summary>
        [Fact]
        public void Whitespace_Space_Front()
        {
            Assert.Equal("_Happiness", Sanitizing.Whitespace(" Happiness"));
        }

        /// <summary>
        /// Tests Whitespaces with whitespaces in the end.
        /// </summary>
        [Fact]
        public void Whitespace_Space_End()
        {
            Assert.Equal("Happiness_", Sanitizing.Whitespace("Happiness "));
        }

        /// <summary>
        /// Tests Whitespaces with whitespaces in the end.
        /// </summary>
        [Fact]
        public void Whitespace_Tab()
        {
            Assert.Equal("Some_easily_read_text_for_testing_line_breaks", Sanitizing.Whitespace("Some	easily	read	text	for	testing	line	breaks"));
        }

        /// <summary>
        /// Tests Whitespaces with whitespaces in the end.
        /// </summary>
        [Fact]
        public void Whitespace_MixedRepetition()
        {
            Assert.Equal("Some_easily_read_text_for_testing_line_breaks", Sanitizing.Whitespace("Some	easily  read 	text	 	for	testing		line     breaks"));
        }

        /// <summary>
        /// Tests Whitespaces with no whitespaces.
        /// </summary>
        [Fact]
        public void Whitespace_None()
        {
            Assert.Equal("Supercalifragilisticexpialidocious", Sanitizing.Whitespace("Supercalifragilisticexpialidocious"));
        }

        /// <summary>
        /// Tests Whitespaces with Linebreaks.
        /// </summary>
        [Fact]
        public void Whitespace_LineBreak()
        {
            string input = @"Some 
easily 
read 
text
for 
 testing 
line 
breaks";
            Assert.Equal(
                "Some_easily_read_text_for_testing_line_breaks",
                Sanitizing.Whitespace(input));
        }

        /// <summary>
        /// Tests Whitespaces with empty input.
        /// </summary>
        /// <param name="input">The input.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Whitespace_Empty(string input)
        {
            Assert.Empty(Sanitizing.Whitespace(input));
        }

        /// <summary>
        /// Tests RecreateSwearing for different inputs.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="expectedOutput">The expected output.</param>
        [Theory]
        [InlineData("b%tch", "bitch")]
        [InlineData("B!tch", "Bitch")]
        [InlineData("B!TCH", "BITCH")]
        [InlineData("F__K", "FUCK")]
        [InlineData("F_ck", "Fuck")]
        [InlineData(",!_$", "shit")]
        [InlineData("-,!_$", "-,!_$")]
        [InlineData("$4!7", "shit")]
        [InlineData("S4!7", "Shit")]
        [InlineData("S4!T", "SHIT")]
        public void RecreateSwearing(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, Sanitizing.RecreateSwearing(input));
        }

        /// <summary>
        /// TestsRecreates the contractions.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="expectedOutput">The expected output.</param>
        [Theory]
        [InlineData("I'll", "I will")]
        [InlineData("I'm", "I am")]
        [InlineData("I've", "I have")]
        [InlineData("She's", "She is")]
        [InlineData("she's", "she is")]
        [InlineData("He's", "He is")]
        [InlineData("he's", "he is")]
        [InlineData("we're", "we are")]
        [InlineData("We're", "We are")]
        [InlineData("we've", "we have")]
        [InlineData("We've", "We have")]
        [InlineData("we'll", "we will")]
        [InlineData("We'll", "We will")]
        [InlineData("you're", "you are")]
        [InlineData("You're", "You are")]
        [InlineData("they're", "they are")]
        [InlineData("They're", "They are")]
        [InlineData("they'll", "they will")]
        [InlineData("They'll", "They will")]
        [InlineData("they've", "they have")]
        [InlineData("They've", "They have")]
        [InlineData("Isn't", "Is not")]
        [InlineData("isn't", "is not")]
        [InlineData("won't", "will not")]
        [InlineData("wouldn't", "would not")]
        [InlineData("weren't", "were not")]
        [InlineData("don't", "do not")]
        [InlineData("didn't", "did not")]
        [InlineData("doesn't", "does not")]
        [InlineData("couldn't", "could not")]
        [InlineData("Couldn't", "Could not")]
        [InlineData("shouldn't", "should not")]
        [InlineData("Shouldn't", "Should not")]
        [InlineData("should've", "should have")]
        [InlineData("Should've", "Should have")]
        [InlineData("can't", "cannot")]
        [InlineData("Can't", "Cannot")]
        public void RecreateContractions(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, Sanitizing.RecreateContractions(input));
        }

        /// <summary>
        /// Tests RemoveDiacritics.
        /// </summary>
        [Fact]
        public void RemoveDiacritics()
        {
            Assert.Equal("aoUcN", Sanitizing.RemoveDiacritics("àôÛçÑ"));
        }

        /// <summary>
        /// Tests RemoveDiacritics.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="expectedOutput">The expected output.</param>
        [Theory]
        [InlineData("ö", "oe")]
        [InlineData("ä", "ae")]
        [InlineData("ü", "ue")]
        [InlineData("Ö", "Oe")]
        [InlineData("Öl", "Oel")]
        [InlineData("ÖL", "OEL")]
        [InlineData("Ä", "Ae")]
        [InlineData("Ähm", "Aehm")]
        [InlineData("ÄTSCH", "AETSCH")]
        [InlineData("Ü", "Ue")]
        [InlineData("Übel", "Uebel")]
        [InlineData("ÜBEL", "UEBEL")]
        [InlineData("Æ", "Ae")]
        [InlineData("æ", "ae")]
        [InlineData("ø", "oe")]
        [InlineData("Ø", "Oe")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void ReplaceUmlauts(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, Sanitizing.ReplaceUmlauts(input));
        }

        /// <summary>
        /// Tests ReplaceIllegalCharacters..
        /// </summary>
        [Fact]
        public void ReplaceIllegalCharacters()
        {
            Assert.Equal("___-_ab_________________c_", Sanitizing.ReplaceIllegalCharacters("__,-_a´`'b_â__âµ/=&%\"’.‘$<<>µ—_c_"));
        }

        /// <summary>
        /// Tests StripSuperfluousReplacementCharacters.
        /// </summary>
        [Fact]
        public void StripSuperfluousReplacementCharacters()
        {
            Assert.Equal("-_ab_c", Sanitizing.StripSuperfluousReplacementCharacters("___-_ab_________________c_"));
        }

        /// <summary>
        /// Tests Crop for long input.
        /// </summary>
        [Fact]
        public void Crop_Long()
        {
            string expected = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, ";
            string input = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu.";
            Assert.Equal(expected, Sanitizing.Crop(input));
        }

        /// <summary>
        /// Tests Crop for short input.
        /// </summary>
        [Fact]
        public void Crop_Short()
        {
            string expected = "Lorem ipsum";
            string input = "Lorem ipsum";
            Assert.Equal(expected, Sanitizing.Crop(input));
        }

        /// <summary>
        /// Tests Crop for empty input.
        /// </summary>
        /// <param name="input">The input.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Crop_Empty(string input)
        {
            Assert.Equal(string.Empty, Sanitizing.Crop(input));
        }
    }
}
