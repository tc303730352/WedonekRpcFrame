using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Helper;
using System.Collections.Concurrent;
using System.Linq;
namespace WeDonekRpc.ApiGateway
{
    /// <summary>
    /// 网关模块集
    /// </summary>
    public class ModularService
    {
        /// <summary>
        /// 模块列表
        /// </summary>
        private static readonly ConcurrentDictionary<string, IModular> _ModularList = new ConcurrentDictionary<string, IModular>();

        /// <summary>
        /// 获取指定模块
        /// </summary>
        /// <typeparam name="T">网关模块</typeparam>
        /// <param name="name">模块名</param>
        /// <returns>网关模块</returns>
        internal static T GetModular<T>(string name) where T : IModular
        {
            if (_ModularList.TryGetValue(name, out IModular modular))
            {
                return (T)modular;
            }
            return default;
        }
        internal static IModular GetModular(string name)
        {
            if (_ModularList.TryGetValue(name, out IModular modular))
            {
                return modular;
            }
            return null;
        }
        /// <summary>
        /// 注册网关模块
        /// </summary>
        /// <param name="modular"></param>
        /// <exception cref="ErrorException"></exception>
        internal static IModular AddModular(IModular modular)
        {
            if (_ModularList.ContainsKey(modular.ServiceName))
            {
                throw new ErrorException("http.modular.repeat");
            }
            else if (!_ModularList.TryAdd(modular.ServiceName, modular))
            {
                throw new ErrorException("http.modular.reg.fail");
            }
            return modular;
        }
        public static void TryAddModular(IModular modular)
        {
            if (!_ModularList.ContainsKey(modular.ServiceName) && !_ModularList.TryAdd(modular.ServiceName, modular))
            {
                throw new ErrorException("http.modular.reg.fail");
            }
        }
        /// <summary>
        /// 开始提供服务
        /// </summary>
        internal static void Start()
        {
            if (_ModularList.IsEmpty)
            {
                return;
            }
            string[] keys = _ModularList.Keys.ToArray();
            keys.ForEach(a =>
            {
                _ModularList[a].Start();
            });
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        internal static void Close()
        {
            if (_ModularList.IsEmpty)
            {
                return;
            }
            string[] keys = _ModularList.Keys.ToArray();
            keys.ForEach(a =>
            {
                if (_ModularList.TryRemove(a, out IModular modular))
                {
                    modular.Dispose();
                }
            });
        }

        internal static void ModularInit()
        {
            if (_ModularList.IsEmpty)
            {
                return;
            }
            string[] keys = _ModularList.Keys.ToArray();
            keys.ForEach(a =>
            {
                _ModularList[a].InitModular();
            });
        }
    }
}
