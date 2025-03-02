using RpcSync.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Visit;
namespace RpcSync.Service.Event
{
    /// <summary>
    /// 节点指令API统计
    /// </summary>
    internal class RpcDicateVisticEvent : IRpcApiService
    {
        private readonly IDicateVisitCollect _Service;

        public RpcDicateVisticEvent (IDicateVisitCollect service)
        {
            this._Service = service;
        }


        /// <summary>
        /// 添加统计日志
        /// </summary>
        /// <param name="add"></param>
        /// <param name="source"></param>
        public void AddVisitLog (AddVisitLog add, MsgSource source)
        {
            this._Service.AddLog(add.Logs, source.ServerId);
        }
        /// <summary>
        /// 重置今日统计
        /// </summary>
        public void ClearVisit ()
        {
            this._Service.ClearVisit();
        }
        /// <summary>
        /// 批量注册统计节点
        /// </summary>
        /// <param name="add"></param>
        /// <param name="source"></param>
        public void RegVisitNode (RegVisitNode add, MsgSource source)
        {
            this._Service.AddNode(add.Visits, source.ServerId);
        }
        /// <summary>
        /// 注册统计节点
        /// </summary>
        /// <param name="add"></param>
        /// <param name="source"></param>
        public void AddVisitNode (AddVisitNode add, MsgSource source)
        {
            this._Service.AddNode(add.Visit, source.ServerId);
        }
    }
}
