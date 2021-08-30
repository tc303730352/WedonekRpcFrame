namespace RpcModel.Server
{
        /// <summary>
        /// 远程系统状态
        /// </summary>
        [System.Serializable]
        public class RunState
        {

                /// <summary>
                /// 链接数
                /// </summary>
                public int ConNum
                {
                        get;
                        set;
                }

                /// <summary>
                /// 占用内存
                /// </summary>
                public long Memory
                {
                        get;
                        set;
                }
                /// <summary>
                /// CPU占用时间
                /// </summary>
                public int CpuRunTime { get; set; }
        }
}
