using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MondoCore.Common;
using MondoCore.Data;
using MondoCore.Repository.TestHelper;

namespace MondoCore.Data.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class MemoryRepositoryTests : RepositoryTestBase<string>
    {
       public MemoryRepositoryTests() :

           base(new MemoryDatabase(),
                "cars",
                ()=> Guid.NewGuid().ToString())
        {
        }

        [TestMethod]
        public async Task MemoryRepository_Insert_maxItems()
        {
            for(var i = 0; i < 10; ++i)
            { 
                await _writer.Insert
                (
                    new Automobile 
                    {
                       Id = _createNewId(),
                       Make = "Pontiac",
                       Model = "GTO" + i.ToString(),
                       Color = "Dark Blue",
                       Year = 1972
                    }
                );
            }

            _reader = _db.GetRepositoryReader<string, Automobile>(_repoName, "Pontiac");

            Assert.AreEqual(9, _reader.Where( r=> r.Make != "").ToList().Count);
        }

        internal class MemoryDatabase : IDatabase
        {
            private object _repo = null;

            public IReadRepository<TID, TValue> GetRepositoryReader<TID, TValue>(string repoName, IIdentifierStrategy<TID> strategy = null) where TValue : IIdentifiable<TID>  
            {
                if(_repo == null)
                { 
                    var repo = new MemoryRepository<TID, TValue>(maxItems: 9);

                    repo.DequeueCount = 1;

                    _repo = repo;
                }

                return _repo as IReadRepository<TID, TValue>;
            }

            public IWriteRepository<TID, TValue> GetRepositoryWriter<TID, TValue>(string repoName, IIdentifierStrategy<TID> strategy = null) where TValue : IIdentifiable<TID> 
            {
                return _repo as IWriteRepository<TID, TValue>;
            }
        }
    }
}
