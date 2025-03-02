using System;
using System.Collections.Generic;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
    public delegate void ReceiveMsgEvent (IMsg msg);
    public delegate void ReceiveEndEvent (IMsg msg, TcpRemoteReply reply);

    public delegate void SendIng (ref SendBody send, int sendNum);
    public delegate void SendEnd (ref SendBody send, IRemoteResult result);
    public delegate void LoadExtend (string dictate, ref Dictionary<string, string> extend);
    public delegate void NoServerEvent (IRemoteConfig config, string sysType, object model);
    public delegate void ServerNodeStateChange (IRemote remote, UsableState oldState, UsableState state);
    public interface IRpcService
    {
        /// <summary>
        /// 服务关闭事件
        /// </summary>
        event Action Closing;
        /// <summary>
        /// 消息发送前
        /// </summary>
        event SendIng SendIng;
        /// <summary>
        /// 消息发送结束
        /// </summary>
        event SendEnd SendComplate;
        /// <summary>
        /// 回复消息前
        /// </summary>
        event ReceiveMsgEvent ReceiveMsg;
        /// <summary>
        /// 发送数据前加载附带参数值
        /// </summary>
        event LoadExtend LoadExtend;
        /// <summary>
        /// 启动完成
        /// </summary>
        event Action StartUpComplate;
        /// <summary>
        /// 初始化组件前
        /// </summary>
        event Action InitComplating;
        /// <summary>
        /// 中控服务连通后开始初始化
        /// </summary>
        event Action BeginIniting;
        /// <summary>
        /// 启动前(基础组件初始化完成时调用)
        /// </summary>
        event Action StartUpComplating;
        /// <summary>
        /// 回复结束
        /// </summary>
        event ReceiveEndEvent ReceiveEnd;
        /// <summary>
        /// 远程状态变更事件
        /// </summary>
        event ServerNodeStateChange RemoteState;
        /// <summary>
        /// 无远程节点事件
        /// </summary>
        event NoServerEvent NoServerEvent;
    }
}
