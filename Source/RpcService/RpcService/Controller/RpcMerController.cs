using RpcService.Model.DAL_Model;

using RpcHelper;

namespace RpcService.Controller
{
        internal class RpcMerController : DataSyncClass
        {
                public RpcMerController(string appId)
                {
                        this.AppId = appId;
                }

                public string AppId { get; }
                public string SystemName
                {
                        get;
                        private set;
                }
                public long OAuthMerId { get; private set; }

                public string AppSecret
                {
                        get;
                        private set;
                }
                public string[] AllowServerIp
                {
                        get;
                        private set;
                }
                protected override bool SyncData()
                {
                        if (!new DAL.RpcMerDAL().GetRpcMer(this.AppId, out RpcMerInfo mer))
                        {
                                this.Error = "rpc.mer.query.error";
                                return false;
                        }
                        else if (mer == null)
                        {
                                this.Error = "oath.mer.not.find";
                                return false;
                        }
                        this.SystemName = mer.SystemName;
                        this.AllowServerIp = mer.AllowServerIp;
                        this.AppSecret = mer.AppSecret;
                        this.OAuthMerId = mer.Id;
                        return true;
                }
        }
}
