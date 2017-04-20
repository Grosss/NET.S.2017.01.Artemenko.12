using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.Tests
{

    [TestFixture]
    public class CustomQueueTests
    {
        private CustomQueue<string> queue;

        [SetUp]
        public void CreateCustomQueue()
        {
            queue = new CustomQueue<string>();
            queue.Enqueue("cup");
            queue.Enqueue("mug");
            queue.Enqueue("knife");
            queue.Enqueue("spoon");
            queue.Enqueue("plate");
            queue.Enqueue("dish");
            queue.Enqueue("fork");
            queue.Enqueue("bowl");
            queue.Enqueue("glass");
        }

        [Test]
        public void GetEnumerator_CallsCustomEnumerator_ExpectedPositiveTest()
        {
            var anotherQueue = new CustomQueue<string>(queue);
            var anotherEnumerator = anotherQueue.GetEnumerator();
            using(var enumerator = queue.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    anotherEnumerator.MoveNext();
                    Assert.AreEqual(enumerator.Current, anotherEnumerator.Current);
                }              
            }
        }

        [Test]
        public void Dequeue_RemovesFirstElementAndReturnsIt_ExpectedPositiveTest()
        {
            int count = queue.Count;
            Assert.AreEqual(queue.Dequeue(), "cup");
            Assert.AreEqual(queue.Count, count - 1);
        }

        [Test]
        public void Peek_ReturnsFirstElement_ExpectedPositiveTest()
        {
            Assert.AreEqual(queue.Peek(), "cup");
            Assert.AreEqual(queue.Peek(), "cup");
            queue.Dequeue();
            Assert.AreEqual(queue.Peek(), "mug");
        }

        [Test]
        public void Contains_PassedElementToFind_ExpectedPositiveTest()
        {
            Assert.AreEqual(queue.Contains("spoon"), true);
            Assert.AreEqual(queue.Contains("fork"), true);
            Assert.AreEqual(queue.Contains("teapot"), false);
        }

        [Test]
        public void ToArray_ReturnsArrayOfElemntsFromQueue_ExpectedPositiveTest()
        {
            string[] array =
            {
                "cup", "mug", "knife", "spoon", "plate","dish", "fork", "bowl", "glass"
            };

            Assert.AreEqual(queue.ToArray(), array);
        }
    }
}
