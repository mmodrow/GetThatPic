using System;

namespace GetThatPic.Data.Structure
{
    /// <summary>
    /// Implements a generic RingBuffer.
    /// Act base on the first in last out principle.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RingBuffer<T>
    {

        private int _length;

        /// <summary>
        /// The buffer.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private readonly T[] buffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RingBuffer{T}"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        public RingBuffer(int size = 50)
        {
            BufferSize = size;
            buffer = new T[BufferSize];
        }

        /// <summary>
        /// The buffer size.
        /// Using a ring buffer to make caching superfluous.
        /// </summary>
        public int BufferSize { get; }

        /// <summary>
        /// Gets or sets the writeIndex of the ring buffer.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        private int StartIndex { get; set; } = 0;

        /// <summary>
        /// Gets or sets the readIndex of the ring buffer.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        private int ReadIndex { get; set; }

        /// <summary>
        /// Gets the <see cref="T"/> with the specified i.
        /// </summary>
        /// <value>
        /// The <see cref="T"/>.
        /// </value>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public T this[int i]
        {
            get
            {
                if(i < 0 | IsEmpty)
                {
                    i = Math.Max(Length - 1, 0);
                }
                else if (i >= Length)
                {
                    i = i % (Length);
                }

                return buffer[(StartIndex + i) % BufferSize];
            }
        }


        public int Length
        {
            get => _length;
            private set
            {
                if (_length >= 0 && _length <= BufferSize)
                {
                    _length = value;
                }
            }
        }

        public bool IsEmpty => 0 >= Length;

        /// <summary>
        /// Adds a new entry.
        /// </summary>
        /// <param name="newEntry">The new entry.</param>
        public void Push(T newEntry)
        {
            // TODO: Reenable overflow.
            bool updateReadIndex = Length - 1 == ReadIndex && Length < BufferSize;

            int writeIndex = (StartIndex + Length) % BufferSize;
            buffer[writeIndex] = newEntry;
            if (Length == BufferSize)
            {
                StartIndex = (StartIndex + 1) % BufferSize;
            }

            if (Length < BufferSize)
            {
                Length++;
            }

            if (updateReadIndex)
            {
                ReadIndex = Length - 1;
            }
        }

        /// <summary>
        /// Pops this instance's last written entry.
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            if (Length == 0)
            {
                return default(T);
            }

            int index = (StartIndex + Length + BufferSize - 1) % BufferSize;

            T output = buffer[index];
            buffer[index] = default(T);

            Length--;
            
            return output;
        }

        /// <summary>
        /// Gets the entry pointed at by the current read index.
        /// </summary>
        /// <value>
        /// The current entry.
        /// </value>
        public T LastWritten => buffer[(StartIndex + Length - 1) % BufferSize];

        /// <summary>
        /// Gets the entry pointed at by the current read index.
        /// </summary>
        /// <value>
        /// The current entry.
        /// </value>
        public T Current => -1 < ReadIndex ? buffer[(StartIndex + ReadIndex) % BufferSize] : default(T);

        /// <summary>
        /// Gets the previous element.
        /// </summary>
        /// <value>
        /// The previous.
        /// </value>
        public T Previous
        {
            get
            {
                if (
                    IsEmpty
                    ||null == buffer[(ReadIndex + StartIndex + BufferSize - 1) % BufferSize]
                    || buffer[(ReadIndex + StartIndex + BufferSize - 1) % BufferSize].Equals(default(T)))
                {
                    return default(T);
                }

                ReadIndex = (ReadIndex + BufferSize - 1) % BufferSize;
                return Current;
            }
        }

        /// <summary>
        /// Gets the next element.
        /// </summary>
        /// <value>
        /// The next.
        /// </value>
        public T Next
        {
            get
            {
                if (
                    null == buffer[(ReadIndex + StartIndex + 1) % BufferSize]
                    || buffer[(ReadIndex + StartIndex + 1) % BufferSize].Equals(default(T)))
                {
                    return Current;
                }

                ReadIndex = (ReadIndex + 1) % BufferSize;
                return Current;
            }
        }

    }
}