using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.RpcApi;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Server
{
    public class RpcToken : DataSyncClass
    {
        public string Access_Token
        {
            get;
            private set;
        }
        public string AppId => Config.WebConfig.BasicConfig.AppId;
        public long RpcMerId
        {
            get;
            private set;
        }
        public int Effective
        {
            get;
            private set;
        }
        protected override void SyncData ()
        {
            if ( !RpcServiceApi.GetAccessToken(out AccessToken token, out string error) )
            {
                throw new ErrorException(error);
            }
            else
            {
                this.RpcMerId = token.RpcMerId;
                this.Access_Token = token.Access_Token;
                this.Effective = HeartbeatTimeHelper.GetTime(token.Effective) - 10;
            }
        }

        public bool CheckToken ()
        {
            if ( this.Effective <= HeartbeatTimeHelper.HeartbeatTime )
            {
                base.ResetLock();
                return this.Init();
            }
            return this.IsInit;
        }

        public bool ResetToken ()
        {
            base.ResetLock();
            return this.Init();
        }
    }
}
