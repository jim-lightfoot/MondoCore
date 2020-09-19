using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.Common;
using MondoCore.Azure.Storage.Queue;
using System.Threading.Tasks;

namespace MondoCore.Azure.Storage.Queue.FunctionalTests
{
    [TestClass]
    [TestCategory("Functional Tests")]
    public class AzureStorageQueueTests
    {
        private string _connectionString = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;";
        private string _queue = "test";

        [TestMethod]
        public async Task AzureStorageQueue_Send()
        {
            var queue = CreateQueue();

            await queue.Send("Chevy Corvette");

            var msg = await queue.Retrieve();

            Assert.AreEqual("Chevy Corvette", msg.Value);

            await queue.Delete(msg);
        }

        private IMessageQueue CreateQueue()
        { 
            return new AzureStorageQueue(_connectionString, _queue);
        }    
    }
}
