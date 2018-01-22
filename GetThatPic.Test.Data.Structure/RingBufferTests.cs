using System;
using System.Collections.Generic;
using GetThatPic.Data.Structure;
using Xunit;

namespace GetThatPic.Test.Data.Structure
{
    public class RingBufferTests
    {
        [Fact]
        public void IsEmpty_Empty()
        {
            RingBuffer<int> buffer = new RingBuffer<int>();

            Assert.True(buffer.IsEmpty);
        }

        [Fact]
        public void IsEmpty_Filled()
        {
            RingBuffer<int> buffer = new RingBuffer<int>();
            buffer.Push(1);

            Assert.False(buffer.IsEmpty);
        }

        [Fact]
        public void IsEmpty_Cleaned()
        {
            RingBuffer<int> buffer = new RingBuffer<int>();
            buffer.Push(1);
            buffer.Pop();

            Assert.True(buffer.IsEmpty);
        }

        [Theory]
        [InlineData(0,0,0)]
        [InlineData(0, 1, 0)]
        [InlineData(1, 1, 0)]
        [InlineData(2, 1, 1)]
        [InlineData(2, 0, 2)]
        public void Length(int push, int pop, int length)
        {
            RingBuffer<int> buffer = new RingBuffer<int>();

            for(int i = 0; i < push; i++) { 
                buffer.Push(i + 1);
            }

            for (int i = 0; i < pop; i++)
            {
                buffer.Pop();
            }

            Assert.Equal(length, buffer.Length);
        }

        [Theory]
        [InlineData(new string[] {"a", "b", "c" }, 1, "c")]
        [InlineData(new string[] {"a", "b", "c" }, 2, "b")]
        [InlineData(new string[] {"a", "b", "c" }, 0, "nothing")]
        [InlineData(new string[] {"a", "b", "c" }, 4, null)]
        public void Pop_Empty(string[] input, int pops, string output)
        {
            RingBuffer<string> buffer = new RingBuffer<string>();

            foreach (string s in input)
            {
                buffer.Push(s);
            }

            string popped = "nothing";

            for (int i = 0; i < pops; i++)
            {
                popped = buffer.Pop();
            }

            Assert.Equal(output, popped);
        }

        [Theory]
        [InlineData(new string[]{}, 0)]
        [InlineData(new string[] { "a"}, 1)]
        [InlineData(new string[] { "a", "b" }, 2)]
        [InlineData(new string[] { "a", "b", "c" }, 3)]
        public void Push_Empty(string[] input, int length)
        {
            RingBuffer<string> buffer = new RingBuffer<string>();

            foreach (string s in input)
            {
                buffer.Push(s);
            }
            
            Assert.Equal(length, buffer.Length);
        }

        [Theory]
        [InlineData(new string[] { }, 1, null)]
        [InlineData(new[] { "a" }, 1, null)]
        [InlineData(new[] { "a", "b", "c"}, 2, "a")]
        public void Previous(string[] input, int stepsBack, string output)
        {
            RingBuffer<string> buffer = new RingBuffer<string>();

            foreach (string s in input)
            {
                buffer.Push(s);
            }

            string found = "nothing";

            for (int i = 0; i < stepsBack; i++)
            {
                found = buffer.Previous;
            }

            Assert.Equal(output, found);
        }

        [Fact]
        public void Next_FromFirstInCollection()
        {
            RingBuffer<int> buffer = new RingBuffer<int>();

            for (int i  = 1; i < 10; i++)
            {
                buffer.Push(i);
                // ReSharper disable once UnusedVariable
                int bufferPrevious = buffer.Previous;
            }

            Assert.Equal(2, buffer.Next);
        }

        [Fact]
        public void Next_FromWithinInCollection()
        {
            RingBuffer<int> buffer = new RingBuffer<int>();

            for (int i  = 1; i < 10; i++)
            {
                buffer.Push(i);
                // ReSharper disable once UnusedVariable
                int bufferPrevious = buffer.Previous;
            }

            // ReSharper disable once UnusedVariable
            // ReSharper disable once NotAccessedVariable
            int bufferNext = buffer.Next;
            // ReSharper disable once RedundantAssignment
            bufferNext = buffer.Next;
            // ReSharper disable once RedundantAssignment
            bufferNext = buffer.Next;
            Assert.Equal(5, buffer.Next);
        }


        [Fact]
        public void Next_Overflow()
        {
            RingBuffer<int> buffer = new RingBuffer<int>(3);

            for (int i = 1; i <= 10; i++)
            {
                buffer.Push(i);
                // ReSharper disable once UnusedVariable
                int bufferPrevious = buffer.Previous;
            }
            
            Assert.Equal(9, buffer.Next);
        }

        [Fact]
        public void Next_Overflow_IndexTooGreat()
        {
            RingBuffer<int> buffer = new RingBuffer<int>(3);

            for (int i = 1; i <= 10; i++)
            {
                buffer.Push(i);
            }

            Assert.Equal(0, buffer.Next);
        }

        [Fact]
        public void Next_Empty()
        {
            RingBuffer<string> buffer = new RingBuffer<string>();

            Assert.Null(buffer.Next);
        }

        [Fact]
        public void LastWritten_Empty()
        {
            RingBuffer<int> buffer = new RingBuffer<int>();
            
            Assert.Equal(0, buffer.LastWritten);
        }

        [Fact]
        public void LastWritten_Written()
        {
            RingBuffer<int> buffer = new RingBuffer<int>();

            for (int i = 1; i <= 10; i++)
            {
                buffer.Push(i);
            }
            Assert.Equal(10, buffer.LastWritten);
        }

        [Fact]
        public void LastWritten_WrittenAndDeleted()
        {
            RingBuffer<int> buffer = new RingBuffer<int>();

            for (int i = 1; i <= 10; i++)
            {
                buffer.Push(i);
            }

            buffer.Pop();
            buffer.Pop();
            Assert.Equal(8, buffer.LastWritten);
        }

        [Fact]
        public void Item_Empty()
        {
            RingBuffer<int> buffer = new RingBuffer<int>();

            Assert.Equal(0, buffer[0]);
        }

        [Fact]
        public void Item_First()
        {
            RingBuffer<int> buffer = new RingBuffer<int>();

            for (int i = 1; i <= 10; i++)
            {
                buffer.Push(i);
            }

            Assert.Equal(1, buffer[0]);
        }

        [Fact]
        public void Item_Within()
        {
            RingBuffer<int> buffer = new RingBuffer<int>();

            for (int i = 1; i <= 10; i++)
            {
                buffer.Push(i);
            }

            Assert.Equal(4, buffer[3]);
        }

        [Fact]
        public void Item_Last()
        {
            RingBuffer<int> buffer = new RingBuffer<int>();

            for (int i = 1; i <= 10; i++)
            {
                buffer.Push(i);
            }

            Assert.Equal(10, buffer[9]);
        }

        [Fact]
        public void Item_Overflow()
        {
            RingBuffer<int> buffer = new RingBuffer<int>(4);

            for (int i = 1; i <= 10; i++)
            {
                buffer.Push(i);
                // ReSharper disable once UnusedVariable
                int bufferPrevious = buffer.Previous;
            }

            Assert.Equal(8, buffer[1]);
        }

        [Fact]
        public void Item_IndexTooGreat()
        {
            RingBuffer<int> buffer = new RingBuffer<int>();

            for (int i = 1; i <= 10; i++)
            {
                buffer.Push(i);
            }

            Assert.Equal(4, buffer[13]);
        }

        [Fact]
        public void Item_NegativeIndex()
        {
            RingBuffer<int> buffer = new RingBuffer<int>();

            for (int i = 1; i < 10; i++)
            {
                buffer.Push(i);
            }

            Assert.Equal(9, buffer[-2]);
        }

        [Theory]
        [InlineData(new int[]{}, 0)]
        [InlineData(new []{1,2,3,4}, 4)]
        [InlineData(new []{1,2,3,4,5,6,7}, 7)]
        public void Current_Overflow(int[] input, int last)
        {
            RingBuffer<int> buffer = new RingBuffer<int>(4);

            foreach (int i in input)
            {
                buffer.Push(i);
            }

            Assert.Equal(last, buffer.Current);
        }

        [Theory]
        [InlineData(new int[]{}, 0)]
        [InlineData(new []{1,2,3,4}, 4)]
        [InlineData(new []{1,2,3,4,5,6,7}, 4)]
        public void Length_Overflow(int[] input, int length)
        {
            RingBuffer<int> buffer = new RingBuffer<int>(4);

            foreach (int i in input)
            {
                buffer.Push(i);
            }

            Assert.Equal(length, buffer.Length);
        }

        [Theory]
        [InlineData(new int[]{}, 0)]
        [InlineData(new []{1,2,3,4}, 3)]
        [InlineData(new []{1,2,3,4,5,6,7}, 6)]
        public void Previous_Overflow(int[] input, int previous)
        {
            RingBuffer<int> buffer = new RingBuffer<int>(4);

            foreach (int i in input)
            {
                buffer.Push(i);
            }
            
            Assert.Equal(previous, buffer.Previous);
        }
    }
}
