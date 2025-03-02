namespace WeDonekRpc.HttpWebSocket.Model
{

        internal class DataPage
        {
                /// <summary>
                /// 包头
                /// </summary>
                public byte[] Head
                {
                        get;
                        set;
                }
                /// <summary>
                /// 包头体
                /// </summary>
                public byte[] HeadBody
                {
                        get;
                        set;
                }


                /// <summary>
                /// 数据内容
                /// </summary>
                public byte[] Content
                {
                        get;
                        set;
                }
                /// <summary>
                /// 是否为最后一个
                /// </summary>
                public bool Fin
                {
                        get;
                        set;
                }
                /// <summary>
                /// 包类型
                /// </summary>
                public PageType PageType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 这个客户端必须是1
                /// </summary>
                public bool IsMask
                {
                        get;
                        set;
                }
              
        }
}
