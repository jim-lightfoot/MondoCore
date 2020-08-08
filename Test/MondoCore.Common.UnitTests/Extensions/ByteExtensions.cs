using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.Common;

namespace MondoCore.Common.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class ByteExtensionsTests
    {
        [TestMethod]
        public void ByteExtensions_DeepClone()
        {
            var bytes = new byte[] { 1, 1, 2, 3, 5, 8 };
            var clone = bytes.DeepClone();

            Assert.AreEqual(6, clone.Length);
            Assert.AreEqual(1, clone[0]);
            Assert.AreEqual(1, clone[1]);
            Assert.AreEqual(2, clone[2]);
            Assert.AreEqual(3, clone[3]);
            Assert.AreEqual(5, clone[4]);
            Assert.AreEqual(8, clone[5]);
        }

        [TestMethod]
        public void ByteExtensions_IsEqual_true()
        {
            var b1 = new byte[] { 1, 1, 2, 3, 5, 8 };
            var b2 = new byte[] { 1, 1, 2, 3, 5, 8 };
            var b3 = new byte[] { };
            var b4 = new byte[] { };

            Assert.IsTrue(b1.IsEqual(b2));
            Assert.IsTrue(b3.IsEqual(b4));
        }

        [TestMethod]
        public void ByteExtensions_IsEqual_false()
        {
            var b1 = new byte[] { 1, 1, 2, 3, 5, 8 };
            var b2 = new byte[] { 1, 1, 2, 3, 8 };
            var b3 = new byte[] { 1, 1, 2, 3, 8 };
            var b4 = new byte[] { 1, 1, 2, 2, 8 };

            Assert.IsFalse(b1.IsEqual(b2));
            Assert.IsFalse(b3.IsEqual(b4));
        }
    }
}
