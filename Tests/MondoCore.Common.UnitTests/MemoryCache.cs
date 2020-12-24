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
    public class MemoryCacheTests
    {
        [TestMethod]
        public async Task MemoryCache_Add()
        {
            var store = new MemoryCache();

            await store.Add("bob",   "bob");
            await store.Add("fred",  "fred");
            await store.Add("wilma", "wilma");
            await store.Add("linda", "linda");

            Assert.AreEqual("bob",   await store.Get("bob"));
            Assert.AreEqual("fred",  await store.Get("fred"));
            Assert.AreEqual("wilma", await store.Get("wilma"));
            Assert.AreEqual("linda", await store.Get("linda"));
            Assert.IsNull(await store.Get("barney"));
        }

        [TestMethod]
        public async Task MemoryCache_Get_absolute_expiration()
        {
            var store = new MemoryCache();

            await store.Add("bob", "bob", DateTime.UtcNow);
            await store.Add("fred", "fred", DateTime.Now);

            Assert.IsNull(await store.Get("bob"));
            Assert.IsNull(await store.Get("fred"));
        }

        [TestMethod]
        public async Task MemoryCache_Get_sliding_expiration()
        {
            var store = new MemoryCache();

            await store.Add("bob", "bob", TimeSpan.FromMilliseconds(10));

            await Task.Delay(11);
            Assert.IsNull(await store.Get("bob"));
        }

        [TestMethod]
        public async Task MemoryCache_Get_sliding_expiration_succeeds()
        {
            var store = new MemoryCache();

            await store.Add("bob", "bob", TimeSpan.FromMilliseconds(30));

            await Task.Delay(11);
            Assert.AreEqual("bob",   await store.Get("bob"));
        }

        [TestMethod]
        public async Task MemoryCache_Get_sliding_expiration_succeeds2()
        {
            var store = new MemoryCache();

            await store.Add("bob", "bob", TimeSpan.FromMilliseconds(50));

            await Task.Delay(20);
            await store.Get("bob");
            await Task.Delay(20);
            await store.Get("bob");
            await Task.Delay(20);
            await store.Get("bob");
            await Task.Delay(10);
            Assert.AreEqual("bob",   await store.Get("bob"));
        }
    }
}
