using RpcModel.Server;

namespace RpcService.Model
{
        /// <summary>
        /// 服务器登陆参数
        /// </summary>
        internal class ServiceLoginParam
        {
                public long SystemType
                {
                        get;
                        set;
                }
                public string Mac
                {
                        get;
                        set;
                }
                public string RemoteIp
                {
                        get;
                        set;
                }
                public int ServerIndex
                {
                        get;
                        set;
                }
                public string ApiVer
                {
                        get;
                        set;
                }
                public ProcessDatum Process { get; internal set; }
        }
}
