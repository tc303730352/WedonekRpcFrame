using System;

using SocketTcpClient.Enum;

namespace SocketTcpClient.Model
{
        internal class GetDataPage
        {
                /// <summary>
                /// 包的ID
                /// </summary>
                internal int PageId
                {
                        get;
                        set;
                }

                /// <summary>
                /// 数据类型（1 字符串类型 , 2 实体对象 3 字节流）
                /// </summary>
                public Enum.SendType DataType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 包的类型(用户自定义)
                /// </summary>
                public string Type
                {
                        get;
                        set;
                }
                public bool IsCompression
                {
                        get;
                        set;
                }
                /// <summary>
                /// 数据包的原始大小(解压时使用)
                /// </summary>
                public int OriginalSize
                {
                        get;
                        set;
                }
                /// <summary>
                /// 数据内容
                /// </summary>
                internal byte[] PageContent
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务端ID
                /// </summary>
                internal string ServerId
                {
                        get;
                        set;
                }

                /// <summary>
                /// 用于处理包的处理接口
                /// </summary>
                internal Interface.IAllot Allot
                {
                        get;
                        set;
                }

                /// <summary>
                /// 当前客户端ID
                /// </summary>
                internal Guid ClientId
                {
                        get;
                        set;
                }

                /// <summary>
                /// 客户端信息
                /// </summary>
                internal Interface.IClient Client
                {
                        get;
                        set;
                }

                /// <summary>
                /// 当前数据流长度(合包时使用)
                /// </summary>
                internal long ContentLen
                {
                        get;
                        set;
                }
                public PageType PageType { get; set; }
        }
}
