namespace RpcModel
{
        /// <summary>
        /// 系统配置项
        /// </summary>
        public class SysConfigData
        {
                /// <summary>
                /// 配置名
                /// </summary>
                public string Name
                {
                        get;
                        set;
                }
                /// <summary>
                /// 是否为JSON格式
                /// </summary>
                public bool IsJson
                {
                        get;
                        set;
                }
                /// <summary>
                /// 配置值
                /// </summary>
                public string Value
                {
                        get;
                        set;
                }

                public override bool Equals(object obj)
                {
                        if (obj is SysConfigData i)
                        {
                                return this.Name == i.Name;
                        }
                        return false;
                }
                public override int GetHashCode()
                {
                        return this.Name.GetHashCode();
                }
        }
}
