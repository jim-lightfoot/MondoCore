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
    public class ParallelStreamWriterTests
    {
        [TestMethod]
        public async Task TaskParallelStreamWriter_Write_string()
        {
            var result = "";

            using(var output = new MemoryStream())
            {
                var writer = new ParallelStreamWriter(output);

                using(var subStream = writer.CreateSubStream())
                {
                    await subStream.Object.WriteAsync("Bob's your uncle");
                }

                await writer.WaitComplete();

                output.Flush();

                result = await output.ReadStringAsync();
            }

            Assert.AreEqual("Bob's your uncle", result);
        }
        
        [TestMethod]
        public async Task TaskParallelStreamWriter_Write_100()
        {
            var result = "";
            var list = new List<Guid>();

            for(var i = 0; i < 500; ++i)
                list.Add(Guid.NewGuid());

            var random = new Random();

            var output = new MemoryStream();
            {
                var writer = new ParallelStreamWriter(output);

                foreach(var guid in list)
                { 
                    var subStream = writer.CreateSubStream();

                    _ = Task.Run( async ()=>
                    {
                        await subStream.Object.WriteAsync(guid.ToString());

                        await Task.Delay(random.Next(500));

                        subStream.Dispose();
                    });
                }

                await writer.WaitComplete();

                output.Flush();

                result = await output.ReadStringAsync();
            }

            Assert.AreEqual(string.Join("", list.Select( guid=> guid.ToString())), result);
        }

    }
}
