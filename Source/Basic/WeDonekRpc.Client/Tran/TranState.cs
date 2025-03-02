using System;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Tran.Model;

namespace WeDonekRpc.Client.Tran
{
    internal class TranStateVal : ICurTranState
    {
        private readonly CurTranState _State;
        private IDisposable _Ass;

        public TranStateVal (long tranId, ITranTemplate template, string body)
        {
            this.Template = template;
            this.Body = new TranSource(body);
            this._State = new CurTranState
            {
                TranId = tranId,
                RegionId = RpcStateCollect.ServerConfig.RegionId,
                RpcMerId = RpcStateCollect.RpcMerId,
                OverTime = DateTime.Now.AddSeconds(WebConfig.RpcConfig.TranOverTime).ToLong()
            };
        }
        public TranStateVal (long tranId, ITranTemplate template, CurTranState state, string body)
        {
            this.Body = new TranSource(body);
            this.Template = template;
            this._State = new CurTranState
            {
                TranId = tranId,
                RegionId = state.RegionId,
                RpcMerId = state.RpcMerId,
                OverTime = state.OverTime
            };
        }
        public TranStateVal (long tranId, ITranTemplate template, CurTranState state, ITranSource source)
        {
            this.Body = source;
            this.Template = template;
            this._State = new CurTranState
            {
                TranId = tranId,
                RegionId = state.RegionId,
                RpcMerId = state.RpcMerId,
                OverTime = state.OverTime
            };
        }
        public TranStateVal (CurTranState state)
        {
            this._State = state;
            this.Body = new TranSource();
        }
        public void BeginTran ()
        {
            if (this.Template.TranMode == RpcTranMode.TwoPC)
            {
                try
                {
                    this._Ass = this.Template.BeginTran(this);
                }
                catch (Exception e)
                {
                    RpcLogSystem.AddErrorLog("启动事务错误", e);
                    throw new ErrorException("rpc.tran.begin.fail");
                }
            }
        }
        public CurTranState Source => this._State;
        /// <summary>
        /// 事务Id
        /// </summary>
        public long TranId => this._State.TranId;

        /// <summary>
        /// 事务协调服务所在区
        /// </summary>
        public int RegionId => this._State.RegionId;
        /// <summary>
        /// 事务协调服务所在集群
        /// </summary>
        public long RpcMerId => this._State.RpcMerId;
        /// <summary>
        /// 事务超时时间
        /// </summary>
        public long OverTime => this._State.OverTime;

        public ITranTemplate Template { get; }

        public ITranSource Body { get; }

        public CurTranState TranState => this._State;

        public override bool Equals (object obj)
        {
            if (obj is ICurTran i)
            {
                return i.TranId == this._State.TranId;
            }
            return false;
        }

        public bool Equals (ICurTran other)
        {
            if (other == null)
            {
                return false;
            }
            return other.TranId == this._State.TranId;
        }

        public override int GetHashCode ()
        {
            return this._State.TranId.GetHashCode();
        }

        public void Dispose ()
        {
            this._Ass?.Dispose();
        }
    }
}
