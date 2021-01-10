using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using MondoCore.Common;
using System.Reflection;

namespace MondoCore.Common.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class FileStoreTests
    {

        [TestMethod]
        public async Task FileStore_Put_string()
        {
            var store = CreateStorage();

            await store.Put("bob", "fred");

            Assert.AreEqual("fred", await store.Get("bob"));

            await store.Delete("bob");
        }

        [TestMethod]
        public async Task FileStore_Put_stream()
        {
            var store = CreateStorage();
            var encoding = UTF8Encoding.UTF8;

            using(var stream = new MemoryStream(encoding.GetBytes("fred")))
            { 
                await store.Put("bob", stream);
            }

            Assert.AreEqual("fred", await store.Get("bob"));

            await store.Delete("bob");
        }

        [TestMethod]
        public async Task FileStore_GetBytes()
        {
            var store = CreateStorage();
            var encoding = UTF8Encoding.UTF8;

            await store.Put("bob", "fred");

            Assert.AreEqual("fred", encoding.GetString(await store.GetBytes("bob")));

            await store.Delete("bob");
        }

        [TestMethod]
        public async Task FileStore_Get()
        {
            var store = CreateStorage();

            await store.Put("bob", "fred");

            Assert.AreEqual("fred", await store.Get("bob"));

            await store.Delete("bob");
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public async Task FileStore_Get_notfound()
        {
            var store = CreateStorage();

            await store.Delete("bob");
            await store.Put("bob", "fred");

            Assert.AreEqual("fred", await store.Get("george"));

            await store.Delete("bob");
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public async Task FileStore_GetBytes_notfound()
        {
            var store = CreateStorage();
            var encoding = UTF8Encoding.UTF8;

            await store.Delete("bob");
            await store.Put("bob", "fred");

            Assert.AreEqual("fred", encoding.GetString(await store.GetBytes("george")));

            await store.Delete("bob");
        }

        [TestMethod]
        public async Task FileStore_Get_stream()
        {
            var store = CreateStorage();

            var uid = Guid.NewGuid().ToString();

            await store.Put(uid, "fred");

            using(var strm = new MemoryStream())
            { 
                await store.Get(uid, strm);

                Assert.AreEqual("fred", await strm.ReadStringAsync());
            }

            await store.Delete(uid);
        }

        [TestMethod]
        public async Task FileStore_OpenRead()
        {
            var store = CreateStorage();
            var uid   = Guid.NewGuid().ToString();

            await store.Put(uid, "fred");

            using(var strm = await store.OpenRead(uid))
            { 
                Assert.AreEqual("fred", await strm.ReadStringAsync());
            }

            await store.Delete(uid);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public async Task FileStore_Delete()
        {
            var store = CreateStorage();

            await store.Delete("bob");
            await store.Put("bob", "fred");

            Assert.AreEqual("fred", await store.Get("bob"));

            await store.Delete("bob");

            await Task.Delay(100);
            Assert.AreEqual("fred", await store.Get("bob"));
        }

        [TestMethod]
        public async Task FileStore_FindAll()
        {
            var store = CreateStorage();

            await store.Put("bio.doc",       "fred");
            await store.Put("photo.jpg",     "flintstone");
            await store.Put("resume.pdf",    "bedrock");
            await store.Put("portfolio.pdf", "stuff");

            var result = await store.Find("*.*");

            Assert.AreEqual(4, result.Count());

            result = await store.Find("*.*");

            Assert.AreEqual(4, result.Count());

            await store.Delete("bio.doc");
            await store.Delete("photo.jpg");
            await store.Delete("resume.pdf");
            await store.Delete("portfolio.pdf");
        }

        [TestMethod]
        public async Task FileStore_Find()
        {
            var store = CreateStorage();

            await store.Put("bio.doc",       "fred");
            await store.Put("photo.jpg",     "flintstone");
            await store.Put("resume.pdf",    "bedrock");
            await store.Put("portfolio.pdf", "stuff");

            var result = await store.Find("*.*");

            Assert.AreEqual(4, result.Count());

            result = await store.Find("*.pdf");

            Assert.AreEqual(2, result.Count());

            await store.Delete("bio.doc");
            await store.Delete("photo.jpg");
            await store.Delete("resume.pdf");
            await store.Delete("portfolio.pdf");
        }

        [TestMethod]
        public async Task FileStore_Enumerate()
        {
            var store = CreateStorage();

            await store.Put("docs/bio.doc",       "fred");
            await store.Put("photos/photo.jpg",     "flintstone");
            await store.Put("resumes/resume.pdf",    "bedrock");
            await store.Put("stuff/portfolio.pdf", "stuff");

            var result = new List<string>();

            await store.Enumerate("*.*", async (blob)=>
            {
                result.Add(blob.Name);

                await Task.CompletedTask;
            }, 
            false);

            Assert.AreEqual(4, result.Count());

            Assert.IsTrue(result.Contains("docs\\bio.doc"));
            Assert.IsTrue(result.Contains("photos\\photo.jpg"));
            Assert.IsTrue(result.Contains("resumes\\resume.pdf"));
            Assert.IsTrue(result.Contains("stuff\\portfolio.pdf"));

            await store.Delete("docs\\bio.doc");
            await store.Delete("photos\\photo.jpg");
            await store.Delete("resumes\\resume.pdf");
            await store.Delete("stuff\\portfolio.pdf");
        }

        [TestMethod]
        public async Task FileStore_Enumerate_folder()
        {
            var store  = CreateStorage("cars\\chevy");
            var store2 = CreateStorage("cars\\pontiac");

            await store2.Put("firebird.tiff",       "fred");
            await store.Put("docs/bio.doc",       "fred");
            await store.Put("photos/photo.jpg",     "flintstone");
            await store.Put("resumes/resume.pdf",    "bedrock");
            await store.Put("stuff/portfolio.pdf", "stuff");

            var result = new List<string>();

            await store.Enumerate("*.*", async (blob)=>
            {
                result.Add(blob.Name);

                await Task.CompletedTask;
            }, 
            false);

            Assert.AreEqual(4, result.Count());

            Assert.IsTrue(result.Contains("docs\\bio.doc"));
            Assert.IsTrue(result.Contains("photos\\photo.jpg"));
            Assert.IsTrue(result.Contains("resumes\\resume.pdf"));
            Assert.IsTrue(result.Contains("stuff\\portfolio.pdf"));

            await store.Delete("docs/bio.doc");
            await store.Delete("photos/photo.jpg");
            await store.Delete("resumes/resume.pdf");
            await store.Delete("stuff/portfolio.pdf");
            await store2.Delete("firebird.tiff");
        }

        private IBlobStore CreateStorage(string folder = "")
        { 
            var uid        = Guid.NewGuid().ToString();
            var path       = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).SubstringBefore("\\bin").Replace("/", "\\");
            var folderPath = Path.Combine(path, "Test Files", uid, folder);

            return new FileStore(folderPath);
        }
    }
}
