// <copyright file="FileNameSanitizingTests.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using GetThatPic.Parsing;
using Xunit;

namespace GetThatPic.Test.Parsing
{
    /// <summary>
    /// Tests the functionality of the FileNameSanitizing class.
    /// </summary>
    public class FileNameSanitizingTests
    {
        /// <summary>
        /// Tests Whitespaces with whitespaces in the middle.
        /// </summary>
        [Fact]
        public void Whitespace_Middle()
        {
            Assert.Equal("There_are_things_in_this_world", FileNameSanitizing.Whitespace("There are things  in this world"));
        }

        /// <summary>
        /// Tests Whitespaces with whitespaces in the front.
        /// </summary>
        [Fact]
        public void Whitespace_Space_Front()
        {
            Assert.Equal("_Happiness", FileNameSanitizing.Whitespace(" Happiness"));
        }

        /// <summary>
        /// Tests Whitespaces with whitespaces in the end.
        /// </summary>
        [Fact]
        public void Whitespace_Space_End()
        {
            Assert.Equal("Happiness_", FileNameSanitizing.Whitespace("Happiness "));
        }

        /// <summary>
        /// Tests Whitespaces with whitespaces in the end.
        /// </summary>
        [Fact]
        public void Whitespace_Tab()
        {
            Assert.Equal("Some_easily_read_text_for_testing_line_breaks", FileNameSanitizing.Whitespace("Some	easily	read	text	for	testing	line	breaks"));
        }

        /// <summary>
        /// Tests Whitespaces with whitespaces in the end.
        /// </summary>
        [Fact]
        public void Whitespace_MixedRepetition()
        {
            Assert.Equal("Some_easily_read_text_for_testing_line_breaks", FileNameSanitizing.Whitespace("Some	easily  read 	text	 	for	testing		line     breaks"));
        }

        /// <summary>
        /// Tests Whitespaces with no whitespaces.
        /// </summary>
        [Fact]
        public void Whitespace_None()
        {
            Assert.Equal("Supercalifragilisticexpialidocious", FileNameSanitizing.Whitespace("Supercalifragilisticexpialidocious"));
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
                FileNameSanitizing.Whitespace(input));
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
            Assert.Empty(FileNameSanitizing.Whitespace(input));
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
            Assert.Equal(expectedOutput, FileNameSanitizing.RecreateSwearing(input));
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
            Assert.Equal(expectedOutput, FileNameSanitizing.RecreateContractions(input));
        }

        /// <summary>
        /// Tests RemoveDiacritics.
        /// </summary>
        [Fact]
        public void RemoveDiacritics()
        {
            Assert.Equal("aoUcN", FileNameSanitizing.RemoveDiacritics("àôÛçÑ"));
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
        [InlineData("Ä", "Ae")]
        [InlineData("Ü", "Ue")]
        [InlineData("Æ", "Ae")]
        [InlineData("æ", "ae")]
        [InlineData("ø", "oe")]
        [InlineData("Ø", "Oe")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void ReplaceUmlauts(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, FileNameSanitizing.ReplaceUmlauts(input));
        }

        /// <summary>
        /// Tests ReplaceIllegalCharacters..
        /// </summary>
        [Fact]
        public void ReplaceIllegalCharacters()
        {
            Assert.Equal("___-_ab_________________c_", FileNameSanitizing.ReplaceIllegalCharacters("__,-_a´`'b_â__âµ/=&%\"’.‘$<<>µ—_c_"));
        }

        /// <summary>
        /// Tests StripSuperfluousReplacementCharacters.
        /// </summary>
        [Fact]
        public void StripSuperfluousReplacementCharacters()
        {
            Assert.Equal("-_ab_c", FileNameSanitizing.StripSuperfluousReplacementCharacters("___-_ab_________________c_"));
        }

        /// <summary>
        /// Tests Crop.
        /// </summary>
        [Fact]
        public void Crop()
        {
            string expected = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, ";
            string input = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu.";
            Assert.Equal(expected, FileNameSanitizing.Crop(input));
        }
    }
}
