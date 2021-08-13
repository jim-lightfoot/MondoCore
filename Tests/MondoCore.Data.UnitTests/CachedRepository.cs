using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

using MondoCore.Common;
using MondoCore.Data;
using MondoCore.Repository.TestHelper;

namespace MondoCore.Data.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class CachedRepositoryTests : RepositoryTestBase<string>
    {
        public CachedRepositoryTests() :

           base(new MemoryDatabase(),
                "cars",
                ()=> Guid.NewGuid().ToString())
        {
        }

        internal class MemoryDatabase : IDatabase
        {
            private object _repo    = null;
            private object _primary = null;
            private object _cache   = null;

            public IReadRepository<TID, TValue> GetRepositoryReader<TID, TValue>(string repoName, IIdentifierStrategy<TID> strategy = null) where TValue : IIdentifiable<TID>  
            {
                if(_repo == null)
                { 
                     var primary = new MemoryRepository<TID, TValue>();
                     var cache = new MemoryRepository<TID, TValue>();

                    _repo = new CachedRepository<TID, TValue>(primary, cache, cache);

                    _cache = cache;
                    _primary = primary;
                }

                return _repo as IReadRepository<TID, TValue>;
            }

            public IWriteRepository<TID, TValue> GetRepositoryWriter<TID, TValue>(string repoName, IIdentifierStrategy<TID> strategy = null) where TValue : IIdentifiable<TID> 
            {
                return _primary as IWriteRepository<TID, TValue>;
            }
        }
    }
}
