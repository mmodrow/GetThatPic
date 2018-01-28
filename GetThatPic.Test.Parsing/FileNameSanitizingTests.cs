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
        /// Tests Whitespaces with no whitespaces.
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
    }
}
