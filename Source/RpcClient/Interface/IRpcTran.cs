using System;

using RpcClient.Tran;

using RpcModel.Tran.Model;

namespace RpcClient.Interface
{
        public interface IRpcTran
        {
                Guid TranId
                {
                        get;
                }
                TranStatus TranStatus
                {
                        get;
                }
                DateTime BeginTime
                {
                        get;
                }
                bool TranIsEnd
                {
                        get;
                }
                TranLog[] TranLogs
                {
                        get;
                }

                void RollbackTran();
                void Commit();
        }
}