using System;

using RpcModel;

namespace RpcClient.Interface
{
        public interface ICurTran
        {
                TranLevel Level { get;}
                int RegionId { get; }
                long RpcMerId { get;  }
                Guid TranId { get;  }
        }
}