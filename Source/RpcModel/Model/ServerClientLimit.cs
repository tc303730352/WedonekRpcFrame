namespace RpcModel.Model
{
        [System.Serializable]
        public class ServerClientLimit
        {
                /// <summary>
                /// 是否启用
                /// </summary>
                public bool IsEnable
                {
                        get;
                        set;
                }
                /// <summary>
                /// 限制类型
                /// </summary>
                public ServerLimitType LimitType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 限制发送数量
                /// </summary>
                public int LimitNum
                {
                        get;
                        set;
                }
                /// <summary>
                /// 限定窗口时间
                /// </summary>
                public short LimitTime
                {
                        get;
                        set;
                }
                /// <summary>
                /// 限定令牌数
                /// </summary>
                public short TokenNum
                {
                        get;
                        set;
                }
                /// <summary>
                /// 每秒写入令牌数量
                /// </summary>
                public short TokenInNum
                {
                        get;
                        set;
                }
        }
}
