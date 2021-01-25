/***************************************************************************
 *                                                                          
 *    The MondoCore Libraries  							                    
 *                                                                          
 *        Namespace: MondoCore.Common							            
 *             File: CSVEnumerable.cs					    		            
 *        Class(es): CSVEnumerable				         		                
 *          Purpose: IEnumerable wrapper for csv streams                     
 *                                                                          
 *  Original Author: Jim Lightfoot                                          
 *    Creation Date: 20 Jan 2020                                            
 *                                                                          
 *   Copyright (c) 2020 - Jim Lightfoot, All rights reserved                
 *                                                                          
 *  Licensed under the MIT license:                                         
 *    http://www.opensource.org/licenses/mit-license.php                    
 *                                                                          
 ****************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Dynamic;

namespace MondoCore.Common
{
    /// <summary>
    /// Enumerates over a delimited file (stream) and deserializes each line into an object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CSVEnumerable<T> : IEnumerable<T> where T : class, new()
    {
        private readonly Stream _input;
        private readonly string _delimiter;
        private readonly string _lineDelimiter;

        public CSVEnumerable(Stream input, string delimiter = ",", string lineDelimiter = "\r\n")
        {
            _input          = input;
            _delimiter      = delimiter;
            _lineDelimiter  = lineDelimiter;
        }

        #region IEnumerable<T>

        public IEnumerator<T> GetEnumerator()
        {
            return new CSVEnumerator<T>(_input, _delimiter, _lineDelimiter);
        }

        #endregion

        #region IEnumerable<T>

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CSVEnumerator<T>(_input, _delimiter, _lineDelimiter);
        }

        #endregion

        #region Private

        private class CSVEnumerator<T2> : IEnumerator<T2>, IEnumerator where T : class, new()
        {
            private readonly Stream _stream;
            private readonly string _delimiter;
            private readonly string _lineDelimiter;
            private readonly byte[] _buffer;
            private readonly int    _bufferSize;
            private readonly IList<string> _props;

            private T2 _current = default(T2);
            private int _bufferPosition = 0;

            public CSVEnumerator(Stream stream, string delimiter = ",", string lineDelimiter = "\r\n", int bufferSize = 4096 * 1024)
            {
                _stream         = stream;
                _delimiter      = delimiter;
                _lineDelimiter  = lineDelimiter;
                _buffer         = new byte[bufferSize];
                _bufferSize     = bufferSize;

                var props = ReadLine().Split(new string[] { _delimiter }, StringSplitOptions.None);

                _props= new List<string>(props);
            }

            public T2 Current => _current;

            object IEnumerator.Current => _current;

            public void Dispose()
            {
            }

            private string ReadLine()
            {
                return "";
            }

            public bool MoveNext()
            {
                var line = ReadLine();
                object obj = null;
                var values = line.Split(new string[] { _delimiter }, StringSplitOptions.None);

                if(typeof(T).Name == "object")
                    obj = new ExpandoObject();
                else
                    obj = new T();


                for(int i = 0; i < _props.Count; ++i)
                {
                    obj.SetValue();
                }

                return false;
            }

            public void CreateExpando(IList<string> values)
            {  
                var obj = new ExpandoObject();

                for(int i = 0; i < _props.Count; ++i)
                {
                    obj[] = 
                }
            }

            public void Reset()
            {
                if(!_stream.CanSeek)
                    throw new NotSupportedException("Cannot set position on underlying stream");

                _stream.Seek(0, SeekOrigin.Begin);
                _current = default(T2);
            }
        }

        #endregion
    }
}
