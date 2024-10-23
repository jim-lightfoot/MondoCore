using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using MondoCore.Collections;
using MondoCore.Common;

namespace MondoCore.Data
{
    public class MemoryRepository<TID, TValue> : IReadRepository<TID, TValue>, IWriteRepository<TID, TValue> where TValue : IIdentifiable<TID>
    {
        private readonly ConcurrentDictionary<TID, Entry> _items = new ConcurrentDictionary<TID, Entry>();
        private readonly ConcurrentQueue<Entry>           _queue = new ConcurrentQueue<Entry>();

        private readonly TimeSpan? _absoluteExpiration;
        private readonly TimeSpan? _relativeExpiration;
        private readonly int       _maxItems;
 
        public MemoryRepository(TimeSpan? absoluteExpiration = null, TimeSpan? relativeExpiration = null, int maxItems = int.MaxValue)
        {
            _absoluteExpiration = absoluteExpiration;
            _relativeExpiration = relativeExpiration;
            _maxItems           = maxItems;
        }

        public int DequeueCount { get; set; } = 10;

        #region IReadRepository

        public Task<TValue> Get(TID id)
        {
            try
            { 
                var entry = _items[id];

                if(_absoluteExpiration.HasValue && (entry.Timestamp + _absoluteExpiration.Value) <= DateTime.UtcNow)
                {
                    RemoveEntry(entry);
                    throw new NotFoundException("Item not found");
                }

                if(_relativeExpiration.HasValue && (entry.Timestamp + _relativeExpiration.Value) <= DateTime.UtcNow)
                {
                    RemoveEntry(entry);
                    throw new NotFoundException("Item not found");
                }

                return Task.FromResult(entry.Value);
            }
            catch(KeyNotFoundException ex)
            {
                throw new NotFoundException("Item not found", ex);
            }
        }

        public async IAsyncEnumerable<TValue> Get(IEnumerable<TID> ids)
        {            
            foreach(var id in ids)
                if(_items.ContainsKey(id))
                    yield return await Task.FromResult(_items[id].Value);
        }

        public async IAsyncEnumerable<TValue> Get(Expression<Func<TValue, bool>> query)
        {
            var fnGuard = query.Compile();  
            var result = _items.Where( kv=> fnGuard(kv.Value.Value)).Select( kv=> kv.Value.Value );

            foreach(var item in result)
                yield return await Task.FromResult(item);
        }

        #region IQueryable<>

        #region IQueryable

        public Type             ElementType => typeof(TValue);
        public Expression       Expression  => _items.Values.Select( v=> v.Value ).AsQueryable<TValue>().Expression;
        public IQueryProvider   Provider    => _items.Values.Select( v=> v.Value ).AsQueryable<TValue>().Provider;

        #endregion

        #region IEnumerable<>

        public IEnumerator<TValue> GetEnumerator() => _items.Values.Select( v=> v.Value ).GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
          => _items.Values.Select( v=> v.Value ).GetEnumerator();

        #endregion

        #endregion

        #endregion

        #region IWriteRepository

        public Task<TValue> Insert(TValue item)
        {
            var id = item.GetValue<TID>("Id");

            if(id.Equals(default(TID)))
                throw new ArgumentException("Item must have a valid id to add to collection");

                AddEntry(id, item);

            return Task.FromResult(item);
        }

        public Task Insert(IEnumerable<TValue> items)
        {
            foreach(var item in items)
            { 
                var id = item.GetValue<TID>("Id");

                AddEntry(id, item);
            }

            return Task.CompletedTask;
        }

        public Task<bool> Update(TValue item, Expression<Func<TValue, bool>> guard = null)
        {
            var id = item.GetValue<TID>("Id");
            var current = _items[id];

            if(guard != null)
               if(!guard.Compile()(current.Value))
                   return Task.FromResult(false);

           AddEntry(id, item);

            return Task.FromResult(true);
        }

        public Task<long> Update(object properties, Expression<Func<TValue, bool>> guard)
        {
            var fnGuard = guard.Compile();
            var matched = _items.Where( kv=> fnGuard(kv.Value.Value));

            if(!matched.Any())
                return Task.FromResult(0L);

            var dProps      = properties.ToDictionary();
            long numUpdated = 0;

            foreach(var kv in matched)
            {
                if(kv.Value.Value.SetValues(dProps))
                    ++numUpdated;
            }

            return Task.FromResult(numUpdated);
        }

        public Task<bool> Delete(TID id)
        {
            return Task.FromResult(_items.TryRemove(id, out Entry _));
        }

        public Task<long> Delete(Expression<Func<TValue, bool>> guard)
        {
            var  fnGuard = guard.Compile();
            var  matched = _items.Where( kv=> fnGuard(kv.Value.Value));
            long numDeleted = 0;

            foreach(var kv in matched)
                if(_items.TryRemove(kv.Key, out Entry _))
                    ++numDeleted;

            return Task.FromResult(numDeleted);
        }

        #endregion

        #region Private

        private class Entry
        {
            internal TID      Id        { get; set; }
            internal TValue   Value     { get; set; }
            internal DateTime Timestamp { get; set; }
            internal bool     Deleted   { get; set; }
        }

        private void RemoveEntry(Entry entry)
        {
            _items.TryRemove(entry.Id, out Entry _);
            entry.Deleted = true;
        }

        private void AddEntry(TID id, TValue item)
        {
            if(_items.Count >= _maxItems)
            {
                int count = this.DequeueCount;

                // Remove a batch of items to make room for new entries
                while(--count >= 0 && _queue.TryDequeue(out Entry dequeue))
                {
                    _items.TryRemove(dequeue.Id, out Entry _);
                }

                // Dequeue any items already deleted
                while(_queue.TryPeek(out Entry dqEentry) && dqEentry.Deleted)
                { 
                    if(_queue.TryDequeue(out Entry _))
                        break;
                }
            }

            var entry = new Entry { Id = id, Value = item, Timestamp = DateTime.UtcNow };

            _items[id] = entry;
            _queue.Enqueue(entry);
        }



        #endregion
    }
}
