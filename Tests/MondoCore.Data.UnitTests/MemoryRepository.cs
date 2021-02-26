using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using MondoCore.Common;
using MondoCore.Data;

namespace MondoCore.Common.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class MemoryRepositoryTests
    {

        [TestMethod]
        public async Task MemoryRepository_Get()
        {
            var store = CreateRepository();

            await store.Insert("bob", "fred");

            Assert.AreEqual("fred", await store.Get("bob"));
        }

        [TestMethod]
        public async Task MemoryRepository_Get_notfound()
        {
            var store = CreateRepository();

            await store.Insert("bob", "fred");

            await Assert.ThrowsExceptionAsync<NotFoundException>( async ()=> await store.Get("george"));
        }

        [TestMethod]
        public async Task MemoryRepository_Delete()
        {
            var store = CreateRepository();

            await store.Insert("bob", "fred");

            Assert.AreEqual("fred", await store.Get("bob"));

            await store.Delete("bob");

            await Assert.ThrowsExceptionAsync<NotFoundException>( async ()=> await store.Get("bob"));
        }

        private MemoryRepository CreateRepository(string folder = "")
        { 
            return new MemoryRepository();
        }
    }
}
