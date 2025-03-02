using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.Model.Tran.Model;
using WeDonekRpc.SqlSugar;

namespace WeDonekRpc.SqlSugarDbTran
{
    internal class DbTran
    {
        private readonly ILocalTran _Tran;
        public readonly CurTranState tanState;
        private bool _IsLock = false;
        private int _RetryNum = 0;
        public int timeOut;

        public DbTran ( ILocalTran tran, ICurTran cur )
        {
            this.tanState = cur.TranState;
            this.timeOut = HeartbeatTimeHelper.GetTime(cur.OverTime.ToDateTime()) + 3;
            this._Tran = tran;
        }
        public bool Lock ( int now )
        {
            if ( this._IsLock == false && now >= this.timeOut )
            {
                this._IsLock = true;
                return true;
            }
            return false;
        }
        public void Commit ()
        {
            this._Tran.CommitTran();
        }
        public void Callback ()
        {
            this._Tran.RollbackTran();
        }

        internal bool CheckFail ( string error )
        {
            this._RetryNum++;
            if ( this._RetryNum > 3 )
            {
                new ErrorLog(error, "事务超时状态检查失败!", "DBTran")
                {
                    {"TranId",this._Tran.TranId }
                }.Save();
                return true;
            }
            this._IsLock = false;
            return false;
        }

    }
}
