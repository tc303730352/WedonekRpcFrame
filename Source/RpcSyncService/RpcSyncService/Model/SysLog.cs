using RpcModel.SysError;

namespace RpcSyncService.Model
{
        public class SysLog : SysErrorLog
        {
                public long RpcMerId
                {
                        get;
                        set;
                }
                public long GroupId
                {
                        get;
                        set;
                }
                public long SystemTypeId
                {
                        get;
                        set;
                }
                public long ServerId
                {
                        get;
                        set;
                }
        }
}
