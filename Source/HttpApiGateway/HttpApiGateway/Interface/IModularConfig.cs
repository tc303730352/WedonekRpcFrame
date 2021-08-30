
using RpcModular;
using RpcModular.Model;

namespace HttpApiGateway.Interface
{
        /// <summary>
        /// 模块配置
        /// </summary>
        public interface IModularConfig
        {
                /// <summary>
                ///   Api 接口地址生成格式
                /// </summary>
                string ApiRouteFormat { get; set; }
                /// <summary>
                /// 注册用户状态对象
                /// </summary>
                /// <typeparam name="T"></typeparam>
                void RegUserState<T>() where T : UserState;
                /// <summary>
                /// 获取用户授权状态
                /// </summary>
                /// <param name="accreditId">状态吗</param>
                /// <returns>授权状态</returns>
                IUserState GetAccredit(string accreditId);
        }
}