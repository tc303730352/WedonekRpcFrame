using System;

namespace RpcModel.Server
{
        public class ProcessDatum
        {
                /// <summary>
                /// 进程PID
                /// </summary>
                public int Pid
                {
                        get;
                        set;
                }
                /// <summary>
                /// 进程名
                /// </summary>
                public string PName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 进程启动时间
                /// </summary>
                public DateTime StartTime { get; set; }

                /// <summary>
                /// 占用内存
                /// </summary>
                public long WorkMemory
                {
                        get;
                        set;
                }
        }
}
