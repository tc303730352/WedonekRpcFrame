using System.Threading;

using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Balanced
{
    /// <summary>
    /// 平均负载算法
    /// </summary>
    internal class AverageBalanced : IBalanced
    {
        /// <summary>
        /// 负载类型
        /// </summary>
        public BalancedType BalancedType
        {
            get;
        } = BalancedType.avg;
        /// <summary>
        /// 当前已分配的服务索引
        /// </summary>
        private int _ServerIndex = 0;
        /// <summary>
        /// 服务节点列表
        /// </summary>
        private readonly long[] _ServerList;
        /// <summary>
        /// 节点数量
        /// </summary>
        private readonly int _ServerNum;
        /// <summary>
        /// 平均负载
        /// </summary>
        /// <param name="serverId">节点列表</param>
        public AverageBalanced (long[] serverId)
        {
            this._ServerList = serverId;
            this._ServerNum = serverId.Length;
        }
        /// <summary>
        /// 获取分配的节点索引
        /// </summary>
        /// <returns></returns>
        private int _GetIndex ()
        {
            int index = Interlocked.Add(ref this._ServerIndex, 1) % this._ServerNum;
            if (index < 0)
            {
                return -index;
            }
            return index;
        }
        /// <summary>
        /// 检查节点是否可用
        /// </summary>
        /// <param name="config">通讯配置</param>
        /// <param name="begin">起始的索引</param>
        /// <param name="server">分配的服务节点</param>
        /// <returns>是否成功</returns>
        private bool _CheckIsUsable (IRemoteConfig config, int begin, out IRemote server)
        {
            int index = this._GetIndex();
            if (index == begin)
            {
                //无可分配的节点
                server = null;
                return false;
            }
            else
            {
                long serverId = this._ServerList[index];
                if (serverId != config.FilterServerId && RemoteServerCollect.GetUsableServer(serverId, config.IsProhibitTrace, out server))
                {
                    return true;
                }
                else
                {
                    return this._CheckIsUsable(config, begin, out server);
                }
            }
        }
        /// <summary>
        /// 检查节点状态
        /// </summary>
        /// <param name="config"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public bool DistributeNode (IRemoteConfig config, out IRemote server)
        {
            if (this._ServerNum == 1)
            {
                long serverId = this._ServerList[0];
                if (serverId != config.FilterServerId)
                {
                    return RemoteServerCollect.GetUsableServer(serverId, config.IsProhibitTrace, out server);
                }
                server = null;
                return false;
            }
            else
            {
                int index = this._GetIndex();
                long serverId = this._ServerList[index];
                if (serverId != config.FilterServerId && RemoteServerCollect.GetUsableServer(serverId, config.IsProhibitTrace, out server))
                {
                    return true;
                }
                else
                {
                    return this._CheckIsUsable(config, index, out server);
                }
            }
        }

        public IRemoteCursor GetAllNode ()
        {
            return new RemoteCursor(this._ServerList);
        }
    }
}