using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MondoCore.Common
{
    /****************************************************************************/
    /****************************************************************************/
    public static class StreamExtensions
    {
        /****************************************************************************/
        public static async Task<string> ReadStringAsync(this Stream stream, Encoding encoder = null)
        {
            if(encoder == null)
                encoder = UTF8Encoding.UTF8;

            if(stream.CanSeek)
                stream.Seek(0, SeekOrigin.Begin);

            try
            { 
                using(var mem = new MemoryStream())
                { 
                    await mem.WriteAsync(stream);

                    return encoder.GetString(mem.ToArray());
                }
            }
            finally
            { 
                if(stream.CanSeek)
                    stream.Seek(0, SeekOrigin.Begin);
            }
        }

        /****************************************************************************/
        public static Task WriteAsync(this Stream stream, string data, Encoding encoder = null)
        {
            if(encoder == null)
                encoder = UTF8Encoding.UTF8;

            byte[] bytes = encoder.GetBytes(data);
       
            return stream.WriteAsync(bytes, 0, bytes.Length);
        }

        /****************************************************************************/
        public static async Task WriteAsync(this Stream dest, Stream src)
        {
            const int BufferSize  = 65536;
            int       numRead     = 0;
            byte[]    buffer      = new byte[BufferSize];
            
            numRead = await src.ReadAsync(buffer, 0, BufferSize);
                
            while(numRead > 0)
            {
                await dest.WriteAsync(buffer, 0, numRead);
                numRead = await src.ReadAsync(buffer, 0, BufferSize);
            }
        }
    }
}
