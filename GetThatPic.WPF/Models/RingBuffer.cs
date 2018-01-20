using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetThatPic.WPF.Models
{
    /// <summary>
    /// Implements a generic RingBuffer.
    /// Act base on the first in last out principle.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class RingBuffer<T>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="RingBuffer{T}"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        public RingBuffer(int size = 50)
        {
            bufferSize = size;
            buffer = new T[bufferSize];
        }

        /// <summary>
        /// The buffer size.
        /// Using a ring buffer to make caching superfluous.
        /// </summary>
        public int bufferSize { get; private set; }

        /// <summary>
        /// Gets or sets the writeIndex of the ring buffer.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        private int writeIndex { get; set; } = -1;

        /// <summary>
        /// Gets or sets the readIndex of the ring buffer.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        private int readIndex { get; set; } = 0;

        /// <summary>
        /// The buffer.
        /// </summary>
        private readonly T[] buffer;

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
            get => buffer[i];
            private set => buffer[i] = value;
        }

        /// <summary>
        /// Adds a new entry.
        /// </summary>
        /// <param name="newEntry">The new entry.</param>
        public void Push(T newEntry)
        {
            bool updateReadIndex = writeIndex == readIndex;
            writeIndex = (writeIndex + 1) % bufferSize;
            buffer[writeIndex] = newEntry;

            if (updateReadIndex)
            {
                readIndex = writeIndex;
            }
        }

        /// <summary>
        /// Pops this instance's last written entry.
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            T output = buffer[writeIndex];
            buffer[writeIndex] = default(T);
            writeIndex--;
            return output;
        }

        /// <summary>
        /// Gets the entry pointed at by the current read index.
        /// </summary>
        /// <value>
        /// The current entry.
        /// </value>
        public T LastWritten => buffer[writeIndex];

        /// <summary>
        /// Gets the entry pointed at by the current read index.
        /// </summary>
        /// <value>
        /// The current entry.
        /// </value>
        public T Current => buffer[readIndex];

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
                if(
                    null == buffer[(readIndex + bufferSize - 1) % bufferSize] 
                    || buffer[(readIndex + bufferSize - 1) % bufferSize].Equals(default(T)))
                {
                    return Current;
                }

                readIndex = (readIndex + bufferSize - 1) % bufferSize;
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
                    null == buffer[(readIndex + 1) % bufferSize]
                    || buffer[(readIndex + 1) % bufferSize].Equals(default(T)))
                {
                    return Current;
                }

                readIndex = (readIndex + 1) % bufferSize;
                return Current;
            }
        }

    }
}
