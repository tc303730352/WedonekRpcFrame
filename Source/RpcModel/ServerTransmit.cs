namespace RpcModel
{
        /// <summary>
        /// 负载配置
        /// </summary>
        public class ServerTransmit
        {
                /// <summary>
                /// 负载类型
                /// </summary>
                public TransmitType TransmitType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 负载ID
                /// </summary>
                public int TransmitId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 固定值
                /// </summary>
                public string Value
                {
                        get;
                        set;
                }
                /// <summary>
                /// 负载范围值列表
                /// </summary>
                public TransmitRange[] Range
                {
                        get;
                        set;
                }
        }
}
