using System;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Tran.Model;

namespace WeDonekRpc.Client.Tran
{
    internal class TranStateVal : ICurTranState
    {
        private IDisposable _Ass;

        public TranStateVal ( long tranId, ITranTemplate template, string body )
        {
            this.Template = template;
            this.Body = new TranSource(body);
            this.Source = new CurTranState
            {
                TranId = tranId,
                RegionId = RpcStateCollect.ServerConfig.RegionId,
                RpcMerId = RpcStateCollect.RpcMerId,
                OverTime = DateTime.Now.AddSeconds(WebConfig.RpcConfig.TranOverTime).ToLong()
            };
        }
        public TranStateVal ( long tranId, ITranTemplate template, CurTranState state, string body )
        {
            this.Body = new TranSource(body);
            this.Template = template;
            this.Source = new CurTranState
            {
                TranId = tranId,
                RegionId = state.RegionId,
                RpcMerId = state.RpcMerId,
                OverTime = state.OverTime
            };
        }
        public TranStateVal ( long tranId, ITranTemplate template, CurTranState state, ITranSource source )
        {
            this.Body = source;
            this.Template = template;
            this.Source = new CurTranState
            {
                TranId = tranId,
                RegionId = state.RegionId,
                RpcMerId = state.RpcMerId,
                OverTime = state.OverTime
            };
        }
        public TranStateVal ( CurTranState state )
        {
            this.Source = state;
            this.Body = new TranSource();
        }
        public void BeginTran ()
        {
            if ( this.Template.TranMode == RpcTranMode.TwoPC )
            {
                this._Ass = this.Template.BeginTran(this);
            }
        }
        public CurTranState Source { get; }
        /// <summary>
        /// 事务Id
        /// </summary>
        public long TranId => this.Source.TranId;

        /// <summary>
        /// 事务协调服务所在区
        /// </summary>
        public int RegionId => this.Source.RegionId;
        /// <summary>
        /// 事务协调服务所在集群
        /// </summary>
        public long RpcMerId => this.Source.RpcMerId;
        /// <summary>
        /// 事务超时时间
        /// </summary>
        public long OverTime => this.Source.OverTime;

        public ITranTemplate Template { get; }

        public ITranSource Body { get; }

        public CurTranState TranState => this.Source;

        public override bool Equals ( object obj )
        {
            if ( obj is ICurTran i )
            {
                return i.TranId == this.Source.TranId;
            }
            return false;
        }

        public bool Equals ( ICurTran other )
        {
            if ( other == null )
            {
                return false;
            }
            return other.TranId == this.Source.TranId;
        }

        public override int GetHashCode ()
        {
            return this.Source.TranId.GetHashCode();
        }

        public void Dispose ()
        {
            this._Ass?.Dispose();
        }
    }
}
