namespace SocketTcpClient.Model
{
        /// <summary>
        /// 数据包
        /// </summary>
        internal class DataPage
        {
                /// <summary>
                /// 链接超时时间
                /// </summary>
                public int ConTimeOut;


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
                /// 数据内容
                /// </summary>
                public byte[] Content;

                /// <summary>
                /// 数据包的唯一标示
                /// </summary>
                public int DataId;

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
                public byte TypeLen;
                public byte HeadType;
                public int GetPageSize()
                {
                        if (this.OriginalSize == 0)
                        {
                                return this.TypeLen + SocketTools.HeadLen + 1;
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
                                return this.TypeLen + len + SocketTools.HeadLen + this.DataLen;
                        }
                }
        }
}
