using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using MondoCore.Common;

namespace MondoCore.Security.Encryption
{
    public class PassThruEncryptor : IEncryptor
    {
        public EncryptionPolicy Policy => throw new NotImplementedException();

        public Task<byte[]> Decrypt(byte[] aEncrypted, int offset = 0)
        {
            return Task.FromResult(aEncrypted.DeepClone());
        }

        public async Task Decrypt(Stream input, Stream output)
        {
            await input.CopyToAsync(output);
        }

        public Task<byte[]> Encrypt(byte[] aData)
        {
            return Task.FromResult(aData.DeepClone());
        }

        public async Task Encrypt(Stream input, Stream output)
        {
            await input.CopyToAsync(output);
        }
    }
}
