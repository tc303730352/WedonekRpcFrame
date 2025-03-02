using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcSync.Model
{
    public class RegTranState
    {
        /// <summary>
        /// 事务ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime OverTime
        {
            get;
            set;
        }
        /// <summary>
        /// 事务状态
        /// </summary>
        public TransactionStatus TranStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLock { get; set; }
        public DateTime AddTime { get; set; }

        public void CheckIsEnd ()
        {
            if (this.TranStatus != TransactionStatus.执行中)
            {
                throw new ErrorException("rpc.tran.state.change");
            }
            else if (this.OverTime <= DateTime.Now)
            {
                throw new ErrorException("rpc.tran.overtime");
            }
        }
        public void CheckState ()
        {
            if (this.TranStatus == TransactionStatus.待回滚)
            {
                throw new ErrorException("rpc.tran.state.change");
            }
        }
        public void CheckIsAllowRollback ()
        {
            if (this.TranStatus != TransactionStatus.已提交)
            {
                throw new ErrorException("rpc.tran.state.change");
            }
            else if (this.OverTime <= DateTime.Now)
            {
                throw new ErrorException("rpc.tran.overtime");
            }
        }
        public void CheckIsExec ()
        {
            if (this.TranStatus != TransactionStatus.已提交)
            {
                throw new ErrorException("rpc.tran.state.error", string.Concat("status=", (int)this.TranStatus));
            }
            else if (this.OverTime <= DateTime.Now)
            {
                throw new ErrorException("rpc.tran.overtime");
            }
        }

    }
}
