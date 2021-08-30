using System;

using RpcModel;

namespace RpcSyncService.Model
{
        internal class TransactionList
        {
                /// <summary>
                /// 事务Id
                /// </summary>
                public Guid Id
                {
                        get;
                        set;
                }
                /// <summary>
                /// 主事务Id
                /// </summary>
                public Guid MainTranId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 父级事务Id
                /// </summary>
                public Guid ParentId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 集群Id
                /// </summary>
                public long RpcMerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务节点Id
                /// </summary>
                public long ServerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 节点类别
                /// </summary>
                public string SystemType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 节点类别id
                /// </summary>
                public long SystemTypeId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 区域Id
                /// </summary>
                public int RegionId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 事务名
                /// </summary>
                public string TranName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 提交的数据
                /// </summary>
                public string SubmitJson
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
                /// 事务级别
                /// </summary>
                public TranLevel Level { get; set; }
                /// <summary>
                /// 是否为注册事务
                /// </summary>
                public bool IsRegTran { get; set; }

                /// <summary>
                /// 是否结束
                /// </summary>
                public bool IsEnd
                {
                        get;
                        set;
                }
                /// <summary>
                ///超时时间
                /// </summary>
                public DateTime OverTime
                {
                        get;
                        set;
                }
                /// <summary>
                /// 失败时间
                /// </summary>
                public DateTime? FailTime
                {
                        get;
                        set;
                }
                /// <summary>
                /// 添加时间
                /// </summary>
                public DateTime AddTime
                {
                        get;
                        set;
                }
                public bool IsMainTran { get; internal set; }
        }
}
