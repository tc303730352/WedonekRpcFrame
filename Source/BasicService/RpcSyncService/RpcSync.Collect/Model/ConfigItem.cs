using WeDonekRpc.Model;
using RpcSync.Model;

namespace RpcSync.Collect.Model
{
    public class ConfigItem : IEquatable<ConfigItem>
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
        /// 服务类型
        /// </summary>
        public RpcServerType ServiceType
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
        /// 区域ID
        /// </summary>
        public int RegionId { get; set; }
        /// <summary>
        /// 容器组
        /// </summary>
        public long ContainerGroup { get; set; }
        /// <summary>
        /// Api版本
        /// </summary>
        public long ApiVer { get; set; }

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
        public SysConfigValueType ValueType
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
        /// 权限
        /// </summary>
        public int Prower
        {
            get;
            set;
        }

        /// <summary>
        /// 是否公有配置
        /// </summary>
        public bool IsPublic
        {
            get;
            set;
        }
        /// <summary>
        /// 权重
        /// </summary>
        public int Weight
        {
            get;
            set;
        }

        public override bool Equals (object obj)
        {
            if (obj is ConfigItem i)
            {
                return this.Name == i.Name;
            }
            return false;
        }

        public bool Equals (ConfigItem other)
        {
            if (other == null)
            {
                return false;
            }
            return this.Name == other.Name;
        }

        public override int GetHashCode ()
        {
            return this.Name.GetHashCode();
        }
    }
}
