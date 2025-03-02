using System;
namespace WeDonekRpc.Model.Model
{
        [Serializable]
        public class LimitConfigRes
        {
                /// <summary>
                /// 限流配置
                /// </summary>
                public ServerLimitConfig LimitConfig { get; set; }

                /// <summary>
                /// 节点限制
                /// </summary>
                public ServerDictateLimit[] DictateLimit { get; set; }
        }
}
