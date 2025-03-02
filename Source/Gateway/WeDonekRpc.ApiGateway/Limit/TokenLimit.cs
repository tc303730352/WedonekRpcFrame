using System.Threading;
using WeDonekRpc.ApiGateway.Config;
using WeDonekRpc.ApiGateway.Interface;

namespace WeDonekRpc.ApiGateway.Limit
{
        /// <summary>
        /// 令牌桶
        /// </summary>
        internal class TokenLimit : ILimit
        {

                private readonly short _InNum = 0;
                private readonly int _TokenSize = 0;

                private int _SurplusNum = 0;

                public GatewayLimitType LimitType => GatewayLimitType.令牌桶;

                public TokenLimit (TokenConfig config)
                {
                        this._InNum = config.TokenInNum;
                        this._TokenSize = config.TokenNum;
                        this._SurplusNum = config.TokenNum;
                }

                public bool IsLimit ()
                {
                        return Interlocked.Decrement (ref this._SurplusNum) < 0;
                }
                public void Refresh (int time)
                {
                        int num = Interlocked.CompareExchange (ref this._SurplusNum, 0, 0);
                        if (num == this._TokenSize)
                        {
                                return;
                        }
                        int add = this._InNum;
                        if (num > 0)
                        {
                                add += num;
                        }
                        if (add > this._TokenSize)
                        {
                                add = this._TokenSize;
                        }
                        if (Interlocked.CompareExchange (ref this._SurplusNum, add, num) != num)
                        {
                                this.Refresh (time);
                        }
                }

        }
}
