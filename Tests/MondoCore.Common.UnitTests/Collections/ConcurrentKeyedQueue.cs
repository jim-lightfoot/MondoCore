using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using MondoCore.Common;

namespace MondoCore.Common.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class ConcurrentKeyedQueueTests
    {
        [TestMethod]
        public void ConcurrentKeyedQueue_Enqueue()
        {
            var queue = new ConcurrentKeyedQueue<string, string>();

            queue.Enqueue("Make",  "Chevy");
            queue.Enqueue("Model", "Corvette");
            queue.Enqueue("Color", "Black");

            Assert.AreEqual("Chevy",    queue["Make"]);
            Assert.AreEqual("Corvette", queue["Model"]);
            Assert.AreEqual("Black",    queue["Color"]);

            Assert.AreEqual("Chevy",    queue.Dequeue());
            Assert.AreEqual("Corvette", queue.Dequeue());
            Assert.AreEqual("Black",    queue.Dequeue());

            Assert.AreEqual(0,           queue.Count);
        }

        [TestMethod]
        public void ConcurrentKeyedQueue_Dequeue_2()
        {
            var queue = new ConcurrentKeyedQueue<string, string>();

            queue.Enqueue("Make",  "Chevy");
            queue.Enqueue("Model", "Corvette");
            queue.Enqueue("Color", "Black");
            queue.Enqueue("Year",  "1964");

            Assert.AreEqual("Chevy",   queue.Dequeue());
            Assert.AreEqual("Black",   queue.Dequeue(2));
            Assert.AreEqual(1,         queue.Count);
            Assert.AreEqual("1964",    queue.Dequeue());

            Assert.AreEqual(0,         queue.Count);
        }

        [TestMethod]
        public void ConcurrentKeyedQueue_Dequeue_all()
        {
            var queue = new ConcurrentKeyedQueue<string, string>();

            queue.Enqueue("Make",  "Chevy");
            queue.Enqueue("Model", "Corvette");
            queue.Enqueue("Color", "Black");
            queue.Enqueue("Year",  "1964");

            Assert.AreEqual("1964",   queue.Dequeue(4));
            Assert.AreEqual(0,         queue.Count);
        }


        [TestMethod]
        public void ConcurrentKeyedQueue_Dequeue_too_many()
        {
            var queue = new ConcurrentKeyedQueue<string, string>();

            queue.Enqueue("Make",  "Chevy");
            queue.Enqueue("Model", "Corvette");
            queue.Enqueue("Color", "Black");
            queue.Enqueue("Year",  "1964");

            Assert.AreEqual("1964",   queue.Dequeue(117));
            Assert.AreEqual(0,         queue.Count);
        }

        [TestMethod]
        public void ConcurrentKeyedQueue_Remove()
        {
            var queue = new ConcurrentKeyedQueue<string, string>();

            queue.Enqueue("Make",  "Chevy");
            queue.Enqueue("Model", "Corvette");
            queue.Enqueue("Color", "Black");

            queue.Remove("Model");

            Assert.AreEqual("Chevy",    queue.Dequeue());
            Assert.AreEqual("Black",    queue.Dequeue());

            Assert.AreEqual(0,           queue.Count);
        }

        [TestMethod]
        public void ConcurrentKeyedQueue_Remove_first()
        {
            var queue = new ConcurrentKeyedQueue<string, string>();

            queue.Enqueue("Make",  "Chevy");
            queue.Enqueue("Model", "Corvette");
            queue.Enqueue("Color", "Black");

            queue.Remove("Make");

            Assert.AreEqual("Corvette",  queue.Dequeue());
            Assert.AreEqual("Black",     queue.Dequeue());
            Assert.AreEqual(0,           queue.Count);
        }

        [TestMethod]
        public void ConcurrentKeyedQueue_Remove_last()
        {
            var queue = new ConcurrentKeyedQueue<string, string>();

            queue.Enqueue("Make",  "Chevy");
            queue.Enqueue("Model", "Corvette");
            queue.Enqueue("Color", "Black");
            queue.Enqueue("Year", "1964");

            queue.Remove("Year");

            Assert.AreEqual("Chevy",     queue.Dequeue());
            Assert.AreEqual("Corvette",  queue.Dequeue());
            Assert.AreEqual("Black",     queue.Dequeue());
            Assert.AreEqual(0,           queue.Count);
        }
    }
}
 