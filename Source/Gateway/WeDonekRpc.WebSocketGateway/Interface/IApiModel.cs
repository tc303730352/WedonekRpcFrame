using System;

namespace WeDonekRpc.WebSocketGateway.Interface
{
    public interface IApiModel
    {
        /// <summary>
        /// API事件处理
        /// </summary>
        Type ApiEventType { get; set; }
        /// <summary>
        /// API地址
        /// </summary>
        string LocalPath { get; set; }
        /// <summary>
        /// 是否检查授权
        /// </summary>
        bool IsAccredit { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        bool IsEnable { get; set; }

        /// <summary>
        /// 访问权限
        /// </summary>
        string[] Prower { get; set; }

    }
}