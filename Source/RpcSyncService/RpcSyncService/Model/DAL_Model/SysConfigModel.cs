using System;

namespace RpcSyncService.Model.DAL_Model
{
        /// <summary>
        /// 系统配置项
        /// </summary>
        internal class SysConfigModel
        {
                /// <summary>
                /// 集群ID
                /// </summary>
                public long RpcMerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 所属节点
                /// </summary>
                public long ServerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 配置名
                /// </summary>
                public string Name
                {
                        get;
                        set;
                }
                /// <summary>
                /// 数据类型
                /// </summary>
                public bool ValueType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 值
                /// </summary>
                public string Value
                {
                        get;
                        set;
                }
                /// <summary>
                /// 最后更新时间
                /// </summary>
                public DateTime ToUpdateTime
                {
                        get;
                        set;
                }
        }
}
