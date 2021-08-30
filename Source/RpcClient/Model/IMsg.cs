using System;
using System.Collections.Generic;

using RpcModel;
namespace RpcClient.Model
{
        public interface IMsg
        {
                Dictionary<string, string> Extend { get; }
                string MsgKey
                {
                        get;
                }

                string MsgBody
                {
                        get;
                }
                T GetMsgBody<T>();

                object GetMsgBody(Type type);

                /// <summary>
                /// 消息来源
                /// </summary>
                MsgSource Source
                {
                        get;
                }

        }
}
