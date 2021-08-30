using RpcModel.Server;

namespace RpcModel
{
        /// <summary>
        /// 远程服务器登陆
        /// </summary>
        public class RemoteServerLogin
        {
                /// <summary>
                /// 服务器类型
                /// </summary>
                public string SystemType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务器MAC
                /// </summary>
                public string ServerMac
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务器编号
                /// </summary>
                public int ServerIndex { get; set; }

                /// <summary>
                /// 进程信息
                /// </summary>
                public ProcessDatum Process { get; set; }
        }
}
