namespace GetThatPic.Data.Structure
{
    /// <summary>
    /// Implements a generic RingBuffer.
    /// Act base on the first in last out principle.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RingBuffer<T>
    {

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
        private int WriteIndex { get; set; } = -1;

        /// <summary>
        /// Gets or sets the readIndex of the ring buffer.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        private int ReadIndex { get; set; }

        /// <summary>
        /// The buffer.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private readonly T[] buffer;

        /// <summary>
        /// Gets the <see cref="T"/> with the specified i.
        /// </summary>
        /// <value>
        /// The <see cref="T"/>.
        /// </value>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public T this[int i] => buffer[i];

        /// <summary>
        /// Adds a new entry.
        /// </summary>
        /// <param name="newEntry">The new entry.</param>
        public void Push(T newEntry)
        {
            bool updateReadIndex = WriteIndex == ReadIndex;
            WriteIndex = (WriteIndex + 1) % BufferSize;
            buffer[WriteIndex] = newEntry;

            if (updateReadIndex)
            {
                ReadIndex = WriteIndex;
            }
        }

        /// <summary>
        /// Pops this instance's last written entry.
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            T output = buffer[WriteIndex];
            buffer[WriteIndex] = default(T);
            WriteIndex--;
            return output;
        }

        /// <summary>
        /// Gets the entry pointed at by the current read index.
        /// </summary>
        /// <value>
        /// The current entry.
        /// </value>
        public T LastWritten => -1 < WriteIndex ? buffer[WriteIndex] : default(T);

        /// <summary>
        /// Gets the entry pointed at by the current read index.
        /// </summary>
        /// <value>
        /// The current entry.
        /// </value>
        public T Current => -1 < ReadIndex ? buffer[ReadIndex] : default(T);

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
                    null == buffer[(ReadIndex + BufferSize - 1) % BufferSize] 
                    || buffer[(ReadIndex + BufferSize - 1) % BufferSize].Equals(default(T)))
                {
                    return Current;
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
                    null == buffer[(ReadIndex + 1) % BufferSize]
                    || buffer[(ReadIndex + 1) % BufferSize].Equals(default(T)))
                {
                    return Current;
                }

                ReadIndex = (ReadIndex + 1) % BufferSize;
                return Current;
            }
        }

    }
}
