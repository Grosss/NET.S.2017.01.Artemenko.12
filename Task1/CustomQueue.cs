using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    public class CustomQueue<T> : IEnumerable<T>, IEnumerable
    {
        private T[] array;
        private int first;
        private int last;
        private int size;

        private const int defaultCapacity = 4;
        private const int minimumGrowth = 4;

        public int Count { get { return size; } }

        public CustomQueue()
        {
            array = new T[0];
        }

        #region Constructors

        public CustomQueue(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException();
            
            array = new T[capacity];
        }

        public CustomQueue(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException();

            array = new T[defaultCapacity];
            size = 0;

            foreach (var element in collection)
                Enqueue(element);
        }

        #endregion

        #region Public methods

        public void Enqueue(T element)
        {
            if (size == array.Length)
            {
                int capacity = (int)(array.Length * 2L);

                if (capacity < (array.Length + minimumGrowth))
                {
                    capacity = array.Length + minimumGrowth;
                }

                SetCapacity(capacity);
            }
            array[last] = element;
            last = (last + 1) % array.Length;
            size++;
        }

        public T Dequeue()
        {
            if (size == 0)
                throw new InvalidOperationException();

            T temp = array[first];
            array[first] = default(T);
            first = (first + 1) % array.Length;
            size--;
            return temp;
        }

        public T Peek()
        {
            if (size == 0)
                throw new InvalidOperationException();

            return array[first];
        }

        public void Clear()
        {
            if (first < last)
            {
                Array.Clear(array, first, size);
            }
            else
            {
                Array.Clear(array, first, array.Length - first);
                Array.Clear(array, 0, last);
            }
            first = last = size = 0;
        }

        public bool Contains(T element)
        {
            int index = first;
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            
            for (int i = 0; i < size; i++)
            {
                if (ReferenceEquals(element, null) && ReferenceEquals(array[index], null))
                {
                    return true;
                }
                else if (!ReferenceEquals(element, null) && comparer.Equals(array[index], element))
                {
                    return true;
                }
                index = (index + 1) % array.Length;
            }
            return false;
        }

        public void TrimExcess()
        {
            if (size < (int)(array.Length * 0.9))
                SetCapacity(size);
        }

        public T[] ToArray()
        {
            T[] newArray = new T[size];
            if (size != 0)
            {
                if (first < last)
                {
                    Array.Copy(array, first, newArray, 0, size);
                    return newArray;
                }
                Array.Copy(array, first, newArray, 0, array.Length - first);
                Array.Copy(array, 0, newArray, array.Length - first, last);
            }
            return newArray;
        }

        #endregion

        #region Interfaces implementations

        public IEnumerator<T> GetEnumerator()
        {
            return new CustomEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CustomEnumerator();
        }

        #endregion

        #region Private methods        

        private void SetCapacity(int capacity)
        {
            T[] newArray = new T[capacity];
            if (size > 0)
            {
                if (first < last)
                {
                    Array.Copy(array, first, newArray, 0, size);
                }
                else
                {
                    Array.Copy(array, first, newArray, 0, array.Length - first);
                    Array.Copy(array, 0, newArray, array.Length - first, last);
                }
            }
            array = newArray;
            first = 0;
            last = (size == array.Length) ? 0 : size;
        }

        T GetElement(int index)
        {
            return array[(first + index) % array.Length];
        }

        #endregion

        #region Enumerator

        private struct CustomEnumerator : IEnumerator<T>, IDisposable
        {
            private readonly CustomQueue<T> queue;
            private int index;
            private T currentElement;

            public CustomEnumerator(CustomQueue<T> queue)
            {
                index = -1;
                this.queue = queue;
                currentElement = default(T);
            }

            public bool MoveNext()
            {
                if (index == -2)
                {
                    return false;
                }

                index++;

                if (index == queue.size)
                {
                    index = -2;
                    currentElement = default(T);
                    return false;
                }

                currentElement = queue.GetElement(index);
                return true;
            }

            public T Current
            {
                get
                {
                    if (index == -1)
                    {
                        throw new InvalidOperationException();
                    }
                    return currentElement;
                }
            }            

            object IEnumerator.Current
            {
                get
                {
                    if (index == -1)
                    {
                        throw new InvalidOperationException();
                    }
                    return currentElement;
                }
            }

            void IEnumerator.Reset()
            {
                index = -1;
                currentElement = default(T);
            }            

            public void Dispose()
            {
                index = -2;
                currentElement = default(T);
            }
        }

        #endregion
    }
}
