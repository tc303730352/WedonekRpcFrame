using SocketTcpServer.Config;
using SocketTcpServer.Enum;

using RpcHelper;

namespace SocketTcpServer.Model
{
        /// <summary>
        /// 数据包
        /// </summary>
        internal class DataPage
        {

                /// <summary>
                /// 数据内容
                /// </summary>
                public byte[] Content;
                /// <summary>
                /// 数据包的唯一标示
                /// </summary>
                public int DataId;

                /// <summary>
                /// 数据的长度
                /// </summary>
                public int DataLen;

                /// <summary>
                /// 数据包的类型
                /// </summary>
                public Enum.PageType PageType;


                /// <summary>
                /// 数据类型（1 字符串类型 , 2 实体对象 3 字节流）
                /// </summary>
                public Enum.SendType DataType;

                /// <summary>
                /// 数据包的原始大小(解压时使用)
                /// </summary>
                public int OriginalSize;

                /// <summary>
                /// 是否压缩
                /// </summary>
                public bool IsCompression;
                /// <summary>
                /// 包的类型(用户自定义)
                /// </summary>
                public char[] Type;
                public byte HeadType;

                private static byte[] _GetSendStream(Page page)
                {
                        if (page.SendData == null)
                        {
                                return new byte[0];
                        }
                        else if (page.SendType == SendType.字节流)
                        {
                                return (byte[])page.SendData;
                        }
                        else if (page.SendType == Enum.SendType.字符串)
                        {
                                return SocketHelper.SerializationString(page.SendData.ToString());
                        }
                        else
                        {
                                return SocketHelper.Json(page.SendData);
                        }
                }
                /// <summary>
                /// 获取数据包的信息
                /// </summary>
                /// <returns></returns>
                public static DataPage GetDataPage(Page page)
                {
                        DataPage temp = new DataPage
                        {
                                DataId = page.PageId
                        };
                        byte[] content = _GetSendStream(page);
                        if (SocketConfig.IsCompression && content.LongLength >= Config.SocketConfig.CompressionSize)
                        {
                                temp.IsCompression = true;
                                temp.OriginalSize = content.Length;
                                byte[] myByte = Tools.CompressionData(content);
                                temp.DataLen = myByte.Length;
                                temp.Content = myByte;
                        }
                        else
                        {
                                temp.OriginalSize = content.Length;
                                temp.Content = content;
                                temp.DataLen = content.Length;
                        }
                        temp.DataType = page.SendType;
                        temp.PageType = page.PageType;
                        temp.Type = page.Type.ToCharArray();
                        return temp;
                }
                public int GetPageSize()
                {
                        if (this.OriginalSize == 0)
                        {
                                return this.Type.Length + SocketHelper.HeadLen + 1;
                        }
                        else
                        {
                                int len = 1;
                                if (this.OriginalSize <= short.MaxValue)
                                {
                                        if (this.IsCompression)
                                        {
                                                len += 4;
                                                this.HeadType = 3;
                                        }
                                        else
                                        {
                                                len += 2;
                                                this.HeadType = 2;
                                        }
                                }
                                else if (this.IsCompression)
                                {
                                        this.HeadType = 5;
                                        len += 8;
                                }
                                else
                                {
                                        this.HeadType = 4;
                                        len += 4;
                                }
                                return this.Type.Length + len + SocketHelper.HeadLen + this.DataLen;
                        }
                }
        }
}
