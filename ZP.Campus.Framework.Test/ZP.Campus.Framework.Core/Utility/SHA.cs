using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ZP.Campus.Framework.Core.Utility
{
    public class SHA
    {
        private byte[] _sha = null;
        private string _input = string.Empty;
        private bool _igonreCase = false;

        public byte[] Digest
        {
            get
            {
                return _sha;
            }
        }

        public string HexDigest
        {
            get
            {
                if (_sha != null)
                {
                    return BitConverter.ToString(_sha).Replace("-", "").ToLower();
                }
                return string.Empty;
            }
        }

        public UInt16 UInt16
        {
            get
            {
                if (_sha != null)
                {
                    return BitConverter.ToUInt16(_sha, 0);
                }
                return 0;
            }
        }

        public UInt32 UInt32
        {
            get
            {
                if (_sha != null)
                {
                    return BitConverter.ToUInt32(_sha, 0);
                }
                return 0;
            }
        }

        public UInt64 UInt64
        {
            get
            {
                if (_sha != null)
                {
                    return BitConverter.ToUInt64(_sha, 0);
                }
                return 0;
            }
        }

        public string HexUInt64
        {
            get
            {
                if (_sha != null)
                {
                    return BitConverter.ToUInt64(_sha, 0).ToString("x").ToLower();
                }
                return string.Empty;
            }
        }

        public SHA SHA1(string input)
        {
            return SHA1(input, false);
        }

        public SHA SHA1(string input, bool igonreCase)
        {
            _input = input;
            _igonreCase = igonreCase;

            if (string.IsNullOrEmpty(input))
            {
                input = string.Empty;
            }

            if (igonreCase)
            {
                input = input.ToUpper();
            }

            try
            {
                _sha = SHA1CryptoServiceProvider.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
            }
            catch { }

            return this;
        }

        public SHA SHA256(string input)
        {
            return SHA256(input, false);
        }

        public SHA SHA256(string input, bool igonreCase)
        {
            _input = input;
            _igonreCase = igonreCase;

            if (string.IsNullOrEmpty(input))
            {
                input = string.Empty;
            }

            if (igonreCase)
            {
                input = input.ToUpper();
            }

            try
            {
                _sha = SHA256CryptoServiceProvider.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
            }
            catch { }

            return this;
        }
    }
}
