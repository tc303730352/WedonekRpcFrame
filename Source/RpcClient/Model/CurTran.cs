using System;

using RpcClient.Interface;

using RpcModel;

namespace RpcClient.Model
{
        [Attr.IgnoreIoc]
        public class CurTran : ICurTran
        {
                /// <summary>
                /// 事务Id
                /// </summary>
                public Guid TranId { get; set; }
                /// <summary>
                /// 事务协调服务所在区
                /// </summary>
                public int RegionId { get; set; }
                /// <summary>
                /// 事务协调服务所在集群
                /// </summary>
                public long RpcMerId { get; set; }
                /// <summary>
                /// 事务级别
                /// </summary>
                public TranLevel Level { get; set; }
        }
}
