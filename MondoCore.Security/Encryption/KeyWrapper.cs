using System;
using System.Collections.Generic;
using System.Text;

using MondoCore.Common;

namespace MondoCore.Security.Encryption
{
	/// <remarks>
	/// An implementation of the AES Key Wrapper from the NIST Key Wrap
	/// Specification as described in RFC 3394.
	/// <p/>
	/// For further details see: <a href="http://www.ietf.org/rfc/rfc3394.txt">http://www.ietf.org/rfc/rfc3394.txt</a>
	/// and  <a href="http://csrc.nist.gov/encryption/kms/key-wrap.pdf">http://csrc.nist.gov/encryption/kms/key-wrap.pdf</a>.
	/// </remarks>
	//public class Rfc3394WrapEngine
    public class KeyWrapper
    {
		private readonly IEncryptor _encryptor;

		private static byte[] _iv = { 0xa6, 0xa6, 0xa6, 0xa6, 0xa6, 0xa6, 0xa6, 0xa6 };

		public KeyWrapper()
		{
			_encryptor = new SymmetricEncryptor(null);
		}

        public virtual byte[] Wrap(byte[] input, int inOff, int inLen)
		{
			int n = inLen / 8;

			if ((n * 8) != inLen)
			{
				throw new DataLengthException("wrap data must be a multiple of 8 bytes");
			}

			byte[] block = new byte[inLen + iv.Length];
			byte[] buf = new byte[8 + iv.Length];

			Array.Copy(iv, 0, block, 0, iv.Length);
			Array.Copy(input, inOff, block, iv.Length, inLen);

			engine.Init(true, param);

			for (int j = 0; j != 6; j++)
			{
				for (int i = 1; i <= n; i++)
				{
					Array.Copy(block, 0, buf, 0, iv.Length);
					Array.Copy(block, 8 * i, buf, iv.Length, 8);
					engine.ProcessBlock(buf, 0, buf, 0);

					int t = n * j + i;
					for (int k = 1; t != 0; k++)
					{
						byte v = (byte)t;

						buf[iv.Length - k] ^= v;
						t = (int) ((uint)t >> 8);
					}

					Array.Copy(buf, 0, block, 0, 8);
					Array.Copy(buf, 8, block, 8 * i, 8);
				}
			}

			return block;
		}

        public virtual byte[] Unwrap(
			byte[]  input,
			int     inOff,
			int     inLen)
		{
			if (forWrapping)
			{
				throw new InvalidOperationException("not set for unwrapping");
			}

			int n = inLen / 8;

			if ((n * 8) != inLen)
			{
				throw new InvalidCipherTextException("unwrap data must be a multiple of 8 bytes");
			}

			byte[]  block = new byte[inLen - iv.Length];
			byte[]  a = new byte[iv.Length];
			byte[]  buf = new byte[8 + iv.Length];

			Array.Copy(input, inOff, a, 0, iv.Length);
            Array.Copy(input, inOff + iv.Length, block, 0, inLen - iv.Length);

			engine.Init(false, param);

			n = n - 1;

			for (int j = 5; j >= 0; j--)
			{
				for (int i = n; i >= 1; i--)
				{
					Array.Copy(a, 0, buf, 0, iv.Length);
					Array.Copy(block, 8 * (i - 1), buf, iv.Length, 8);

					int t = n * j + i;
					for (int k = 1; t != 0; k++)
					{
						byte v = (byte)t;

						buf[iv.Length - k] ^= v;
						t = (int) ((uint)t >> 8);
					}

					engine.ProcessBlock(buf, 0, buf, 0);
					Array.Copy(buf, 0, a, 0, 8);
					Array.Copy(buf, 8, block, 8 * (i - 1), 8);
				}
			}

			if (!Arrays.ConstantTimeAreEqual(a, iv))
				throw new InvalidCipherTextException("checksum failed");

			return block;
		}
	}
}