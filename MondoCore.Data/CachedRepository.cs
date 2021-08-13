using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using MondoCore.Common;

namespace MondoCore.Data
{
    public class CachedRepository<TID, TValue> : IReadRepository<TID, TValue> where TValue : IIdentifiable<TID>
    {
        private readonly IReadRepository<TID, TValue> _primarySource;
        private readonly IReadRepository<TID, TValue> _cache;
        private readonly IWriteRepository<TID, TValue> _cacheWriter;

        public CachedRepository(IReadRepository<TID, TValue> primarySource, IReadRepository<TID, TValue> cache, IWriteRepository<TID, TValue> cacheWriter)
        {
            _primarySource = primarySource;
            _cache         = cache;
            _cacheWriter   = cacheWriter;
        }

        #region IReadRepository

        public async Task<TValue> Get(TID id)
        {
            try
            { 
                return await _cache.Get(id);
            }
            catch(NotFoundException)
            {
                var val = await _primarySource.Get(id);

                try
                { 
                    await _cacheWriter.Insert(val);
                }
                catch
                {

                }

                return val;
            }
        }

        public IAsyncEnumerable<TValue> Get(IEnumerable<TID> ids)
        {
            return _primarySource.Get(ids);
        }

        public  IAsyncEnumerable<TValue> Get(Expression<Func<TValue, bool>> query)
        {
            return _primarySource.Get(query);
        }

        #region IQueryable<>

        #region IQueryable

        public Type             ElementType => typeof(TValue);
        public Expression       Expression  => _primarySource.Expression;
        public IQueryProvider   Provider    => _primarySource.Provider;

        #endregion

        #region IEnumerable<>

        public IEnumerator<TValue> GetEnumerator() => _primarySource.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
          => _primarySource.GetEnumerator();

        #endregion

        #endregion

        #endregion
    }
}
