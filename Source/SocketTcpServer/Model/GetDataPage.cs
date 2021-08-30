using System;

using SocketTcpServer.Enum;

namespace SocketTcpServer.Model
{
        internal class GetDataPage
        {
                /// <summary>
                /// 包的ID
                /// </summary>
                internal int PageId;

                /// <summary>
                /// 数据类型（1 字符串类型 , 2 实体对象 3 字节流）
                /// </summary>
                public Enum.SendType DataType;
                /// <summary>
                /// 包的类型(用户自定义)
                /// </summary>
                public string Type;
                public bool IsCompression;
                /// <summary>
                /// 数据包的原始大小(解压时使用)
                /// </summary>
                public int OriginalSize;
                /// <summary>
                /// 数据内容
                /// </summary>
                internal byte[] PageContent;
                /// <summary>
                /// 服务端ID
                /// </summary>
                internal Guid ServerId;

                /// <summary>
                /// 用于处理包的处理接口
                /// </summary>
                internal Interface.IAllot Allot;

                /// <summary>
                /// 当前客户端ID
                /// </summary>
                internal Guid ClientId;

                /// <summary>
                /// 客户端信息
                /// </summary>
                internal Interface.IClient Client;

                public PageType PageType;
        }
}
