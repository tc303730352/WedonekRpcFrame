using ApiGateway.Attr;
using ApiGateway.Interface;

namespace HttpApiGateway.Interface
{
        /// <summary>
        /// HttpApi模块
        /// </summary>
        public interface IApiModular : IModular
        {
                /// <summary>
                /// 模块配置
                /// </summary>
                IModularConfig Config { get; }
        }
}
