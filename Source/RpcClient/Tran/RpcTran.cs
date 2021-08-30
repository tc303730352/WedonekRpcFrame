using System;

using RpcClient.Collect;
using RpcClient.Interface;

using RpcModel;
using RpcModel.Tran.Model;

namespace RpcClient.Tran
{
        public enum TranStatus
        {
                提交=0,
                回滚=1,
                超时=2
        }
        [Attr.IgnoreIoc]
        internal class RpcTran : IRpcTran
        {
                private readonly CurTranState _Tran = null;
                public RpcTran(CurTranState tran, TranResult result)
                {
                        this._Tran = tran;
                        this.BeginTime = result.BeginTime;
                        this.TranLogs = result.Logs;
                }
                public TranLog[] TranLogs
                {
                        get;
                }

                public Guid TranId => this._Tran.TranId;

                public bool TranIsEnd
                {
                        get;
                        private set;
                }

                public TranStatus TranStatus
                {
                        get;
                        private set;
                }
                public DateTime BeginTime
                {
                        get;
                }

                public void RollbackTran()
                {
                        if (this.TranIsEnd)
                        {
                                return;
                        }
                        this.TranIsEnd = true;
                        RpcTranCollect.ForceRollbackTran(this._Tran);
                        this.TranStatus = TranStatus.回滚;
                }



                public void Commit()
                {
                        if (this.TranIsEnd)
                        {
                                return;
                        }
                        this.TranIsEnd = true;
                        RpcTranCollect.EndTran(this._Tran);
                        this.TranStatus = TranStatus.提交;
                }
        }
}
