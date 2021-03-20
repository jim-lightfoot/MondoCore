using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using MondoCore.Common;

namespace MondoCore.Data
{
    public class MemoryRepository<TID, TValue> : IReadRepository<TID, TValue>, IWriteRepository<TID, TValue> where TValue : IIdentifiable<TID>
    {
        private readonly ConcurrentDictionary<TID, TValue> _items = new ConcurrentDictionary<TID, TValue>();

        public MemoryRepository()
        {
        }

        #region IReadRepository

        public Task<TValue> Get(TID id)
        {
            try
            { 
                return Task.FromResult(_items[id]);
            }
            catch(KeyNotFoundException ex)
            {
                throw new NotFoundException("Item not found", ex);
            }
        }

        public async IAsyncEnumerable<TValue> Get(IEnumerable<TID> ids)
        {
            var dict = ids.ToDictionary( id=> id, id=> id);
            
            foreach(var id in ids)
                if(_items.ContainsKey(id))
                    yield return await Task.FromResult(_items[id]);
        }

        public async IAsyncEnumerable<TValue> Get(Expression<Func<TValue, bool>> query)
        {
            var fnGuard = query.Compile();
            
            var result = _items.Where( kv=> fnGuard(kv.Value)).Select( kv=> kv.Value );

            foreach(var item in result)
                yield return await Task.FromResult(item);
        }

        #region IQueryable<>

        #region IQueryable

        public Type             ElementType => typeof(TValue);
        public Expression       Expression  => _items.Values.AsQueryable<TValue>().Expression;
        public IQueryProvider   Provider    => _items.Values.AsQueryable<TValue>().Provider;

        #endregion

        #region IEnumerable<>

        public IEnumerator<TValue> GetEnumerator() => _items.Values.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
          => _items.Values.GetEnumerator();

        #endregion

        #endregion

        #endregion

        #region IWriteRepository

        public Task<TValue> Insert(TValue item)
        {
            var id = item.GetValue<TID>("Id");

            if(id.Equals(default(TID)))
                throw new ArgumentException("Item must have a valid id in to add to collection");

            _items[id] = item;

            return Task.FromResult(item);
        }

        public Task Insert(IEnumerable<TValue> items)
        {
            foreach(var item in items)
            { 
                var id = item.GetValue<TID>("Id");

                _items[id] = item;
            }

            return Task.CompletedTask;
        }

        public Task<bool> Update(TValue item, Expression<Func<TValue, bool>> guard = null)
        {
            var id = item.GetValue<TID>("Id");
            var current = _items[id];

            if(guard != null)
               if(!guard.Compile()(current))
                   return Task.FromResult(false);

            _items[id] = item;

            return Task.FromResult(true);
        }

        public Task<long> Update(object properties, Expression<Func<TValue, bool>> guard)
        {
            var fnGuard = guard.Compile();
            var matched = _items.Where( kv=> fnGuard(kv.Value));

            if(!matched.Any())
                return Task.FromResult(0L);

            var dProps      = properties.ToDictionary();
            long numUpdated = 0;

            foreach(var kv in matched)
            {
                if(kv.Value.SetValues(dProps))
                    ++numUpdated;
            }

            return Task.FromResult(numUpdated);
        }

        public Task<bool> Delete(TID id)
        {
            return Task.FromResult(_items.TryRemove(id, out TValue _));
        }

        public Task<long> Delete(Expression<Func<TValue, bool>> guard)
        {
            var  fnGuard = guard.Compile();
            var  matched = _items.Where( kv=> fnGuard(kv.Value));
            long numDeleted = 0;

            foreach(var kv in matched)
                if(_items.TryRemove(kv.Key, out TValue _))
                    ++numDeleted;

            return Task.FromResult(numDeleted);
        }

        #endregion
    }
}
