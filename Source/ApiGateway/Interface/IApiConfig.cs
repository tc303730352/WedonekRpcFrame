using System;

namespace ApiGateway.Interface
{
        public interface IApiConfig
        {
                /// <summary>
                /// 是否需要授权
                /// </summary>
                bool IsAccredit { get; }
                /// <summary>
                /// 路由名
                /// </summary>
                string Name { get;  }
                /// <summary>
                /// 类型
                /// </summary>
                public Type Type { get;  }
                /// <summary>
                /// 权限
                /// </summary>
                string Prower { get; }
        }
}
