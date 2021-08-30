using RpcModel.Server;

namespace RpcService.Model.DAL_Model
{
        internal class ServerRunState : ProcessDatum
        {
                /// <summary>
                /// 进程Pid
                /// </summary>
                public long ServerId
                {
                        get;
                        set;
                }
                public int ConNum
                {
                        get;
                        set;
                }
                /// <summary>
                /// CPU运行时间
                /// </summary>
                public long CpuRunTime
                {
                        get;
                        set;
                }
        }
}
