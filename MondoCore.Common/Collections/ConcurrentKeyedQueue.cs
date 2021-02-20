using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MondoCore.Common
{
    /// <summary>
    /// A thread safe queue that can also be accessed via a key (like a dictionary)
    /// </summary>
    public class ConcurrentKeyedQueue<K, V> 
    {
        private readonly Dictionary<K, Entry<V>> _dict  = new Dictionary<K, Entry<V>>();
        private readonly Dictionary<ulong, K>    _queue = new Dictionary<ulong, K>();
        private readonly object                  _lock  = new object();
        private ulong                            _last  = 0;
        private ulong                            _first = 0;

        public ConcurrentKeyedQueue()
        {
        }

        public int Count
        {
            get
            {
                lock(_lock)
                { 
                    return _dict.Count;
                }
            }
        }
 
        public void Enqueue(K key, V item)
        {
            lock(_lock)
            { 
                var entry = new Entry<V> { Value = item, Index = _last++};

                _dict.Add(key, entry);
                _queue.Add(entry.Index, key);
            }
        }

        public V Peek()
        {        
            lock(_lock)
            { 
                while(_queue.Count > 0 && !_queue.ContainsKey(_first))
                    ++_first;

                if(_queue.Count > 0)
                    return _dict[_queue[_first]].Value;

                return default(V);
            }
        }

        public V Dequeue(int count = 1)
        {
            V last = default(V);

            lock(_lock)
            { 
                var dictCount = _dict.Count;

                count = Math.Min(count, dictCount);

                while(count > 0)
                { 
                    if(dictCount == 0)
                    {
                        InternalClear();

                        return default(V);
                    }
                    
                    while(!_queue.ContainsKey(_first) && _first < _last)
                        ++_first;

                    if(_first >= _last)
                    {
                        last = Peek();
                        InternalClear();
                        break;
                    }

                    var key = _queue[_first];
                    last = _dict[key].Value;

                    _dict.Remove(key);
                    _queue.Remove(_first);

                    ++_first;
                    --count;
                }

                if(_dict.Count == 0)
                    InternalClear();
            }

            return last;
        }

        public V this[K key] 
        { 
            get
            {
                lock(_lock) 
                { 
                    return _dict[key].Value; 
                }
            }

            set
            {
                throw new NotSupportedException(); 
            }
        }

        public void Clear()
        {
            lock(_lock)
            { 
                InternalClear();
            }
        }

        private void InternalClear()
        {
            _dict.Clear();
            _queue.Clear();
            _last = 0;
            _first = 0;
        }

        public bool ContainsKey(K key)
        {
            lock(_lock)
            { 
                return _dict.ContainsKey(key);
            }
        }

        public bool Remove(K key)
        {
            lock(_lock)
            { 
                if(_dict.ContainsKey(key))
                { 
                    var entry = _dict[key];

                    _dict.Remove(key);
                    _queue.Remove(entry.Index);

                    if(entry.Index == _first)
                        ++_first;

                    if(_dict.Count == 0)
                        InternalClear();

                    return true;
                }
            }

            return false;
        }

        public bool TryGetValue(K key, out V value)
        {
            lock(_lock)
            { 
                if(_dict.TryGetValue(key, out Entry<V> entry))
                {
                    value = entry.Value;
                    return true;
                }
            }

            value = default(V);
            return false;
        }

        private class Entry<VAL>
        {
            internal VAL   Value { get; set; }
            internal ulong Index { get; set; }
        }
    }
}
