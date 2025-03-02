using System;

using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Interface;

namespace WeDonekRpc.Client.Track
{
    /// <summary>
    /// 默认采样器
    /// </summary>
    internal class DefaultSampler : ISampler
    {
        private readonly long _salt;
        private const int _RatePrecision = 1000000;
        private readonly bool _IsSampler = false;
        private readonly int _LimitVal = 0;
        public DefaultSampler(long salt, int rate)
        {
            if (rate > _RatePrecision)
            {
                rate = _RatePrecision;
            }
            this._IsSampler = rate != _RatePrecision;
            this._LimitVal = rate;
            this._salt = salt;
        }


        public bool Sample(out long spanId)
        {
            spanId = RandomUtils.NextLong();
            if (this._IsSampler)
            {
                return Math.Abs(spanId ^ this._salt) % _RatePrecision < this._LimitVal;
            }
            return true;
        }
    }
}
