using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

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

        internal class MemoryDatabase : IDatabase
        {
            private object _repo = null;

            public IReadRepository<TID, TValue> GetRepositoryReader<TID, TValue>(string repoName, IIdentifierStrategy<TID> strategy = null) where TValue : IIdentifiable<TID>  
            {
                if(_repo == null)
                    _repo = new MemoryRepository<TID, TValue>();

                return _repo as IReadRepository<TID, TValue>;
            }

            public IWriteRepository<TID, TValue> GetRepositoryWriter<TID, TValue>(string repoName, IIdentifierStrategy<TID> strategy = null) where TValue : IIdentifiable<TID> 
            {
                if(_repo == null)
                    _repo = new MemoryRepository<TID, TValue>();

                return _repo as IWriteRepository<TID, TValue>;
            }
        }
    }
}
