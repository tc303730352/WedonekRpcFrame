using System;

namespace RpcModel.Tran.Model
{
        public class CurTranState
        {
                /// <summary>
                /// 事务Id
                /// </summary>
                public Guid TranId { get; set; }
                /// <summary>
                /// 主事务Id
                /// </summary>
                public Guid? MainTranId { get; set; }
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

                public override bool Equals(object obj)
                {
                        if(obj is CurTranState i)
                        {
                                return i.TranId == this.TranId;
                        }
                        return false;
                }
                public override int GetHashCode()
                {
                        return this.TranId.GetHashCode();
                }
        }
}
