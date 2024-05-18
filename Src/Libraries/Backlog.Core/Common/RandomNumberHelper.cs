﻿using System.Security.Cryptography;

namespace Backlog.Core.Common
{
    public class RandomNumberHelper : RandomNumberGenerator
    {
        #region Field

        protected bool _disposed;
        protected readonly RandomNumberGenerator _rng;

        #endregion

        #region Ctor

        public RandomNumberHelper()
        {
            _rng = Create();
        }

        #endregion

        #region Methods

        public int Next()
        {
            var data = new byte[sizeof(int)];
            _rng.GetBytes(data);
            return BitConverter.ToInt32(data, 0) & (int.MaxValue - 1);
        }

        public int Next(int maxValue)
        {
            return Next(0, maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException();
            }
            return (int)Math.Floor(minValue + ((double)maxValue - minValue) * NextDouble());
        }

        public double NextDouble()
        {
            var data = new byte[sizeof(uint)];
            _rng.GetBytes(data);
            var randUint = BitConverter.ToUInt32(data, 0);
            return randUint / (uint.MaxValue + 1.0);
        }

        public override void GetBytes(byte[] data)
        {
            _rng.GetBytes(data);
        }

        public override void GetNonZeroBytes(byte[] data)
        {
            _rng.GetNonZeroBytes(data);
        }

        /// <summary>
        /// Dispose secure random
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _rng?.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
