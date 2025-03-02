using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.SqlSugar;

namespace RpcSync.DAL
{
    internal class RpcExtendDAL : IRpcInitModular
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ioc"></param>
        public void Init (IIocService ioc)
        {

        }
        /// <summary>
        /// 初始化结束
        /// </summary>
        /// <param name="ioc"></param>
        public void InitEnd (IIocService ioc, IRpcService service)
        {
        }
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="option"></param>
        public void Load (RpcInitOption option)
        {
            _ = option.Ioc.RegisterType(typeof(IRpcExtendResource<>), typeof(Repository.RpcExtendResource<>));
            SqlSugarService.Init(new SqlSugarUnity(option.Ioc));
        }

    }
}
