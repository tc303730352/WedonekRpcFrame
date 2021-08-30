namespace RpcService.Model.DAL_Model
{
        internal class RpcMerInfo
        {
                public long Id
                {
                        get;
                        set;
                }
                public string SystemName
                {
                        get;
                        set;
                }
                public string AppSecret
                {
                        get;
                        set;
                }
                public string[] AllowServerIp
                {
                        get;
                        set;
                }
        }
}
