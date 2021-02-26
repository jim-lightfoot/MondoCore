using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MondoCore.Data
{
    public class MemoryRepository : IReadRepository<string>, IWriteRepository<string>
    {
        private readonly ConcurrentDictionary<string, object> _items = new ConcurrentDictionary<string, object>();

        public MemoryRepository()
        {
        }

        #region IReadRepository

        public Task<object> Get(string id)
        {
            return Task.FromResult(_items[id]);
        }

        public Task<IEnumerable<object>> Get(IEnumerable<string> ids)
        {
            var dict = ids.ToDictionary( id=> id, id=> id);

            return Task.FromResult( _items.Where( i=> dict.ContainsKey(i.Key)).Select(kv=> kv.Value) );
        }

        #endregion

        #region IWriteRepository

        public Task<object> Insert(string id, object item)
        {
            _items[id] = item;

            return Task.FromResult(item);
        }

        public Task Update(string id, object item)
        {
            _items[id] = item;

            return Task.CompletedTask;
        }

        public Task Delete(string id)
        {
            _items.TryRemove(id, out object _);

            return Task.CompletedTask;
        }

        #endregion
    }
}
