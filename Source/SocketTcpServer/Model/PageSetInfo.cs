using System;

namespace SocketTcpServer.Model
{
        public class PageSetInfo : ICloneable
        {
                /// <summary>
                /// 包的类型(用户自定义)
                /// </summary>
                public string Type
                {
                        get;
                        set;
                }

                private short tailNum = 0;

                /// <summary>
                /// 包尾的长度
                /// </summary>
                public short TailNum
                {
                        get => this.tailNum;
                        set => this.tailNum = value;
                }

                /// <summary>
                /// 是否压缩
                /// </summary>
                public bool IsCompression
                {
                        get;
                        set;
                }

                /// <summary>
                /// 数据包的类型
                /// </summary>
                public Enum.PageType PageType
                {
                        get;
                        set;
                }

                /// <summary>
                /// 数据的长度
                /// </summary>
                public uint DataLen
                {
                        get;
                        set;
                }

                /// <summary>
                /// 数据包的原始大小(解压时使用)
                /// </summary>
                public uint OriginalSize
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
                private ushort pageIndex = 0;


                /// <summary>
                /// 数据包的索引号(用于合包)
                /// </summary>
                public ushort PageIndex
                {
                        get => this.pageIndex;
                        set => this.pageIndex = value;
                }
                private ushort pageSum = 1;

                /// <summary>
                /// 包的总数
                /// </summary>
                public ushort PageSum
                {
                        get => this.pageSum;
                        set => this.pageSum = value;
                }
                /// <summary>
                /// 包的数据总长度(如果包数为1 值为0)
                /// </summary>
                public uint PageSumLen
                {
                        get;
                        set;
                }

                private Guid _ParentId = Guid.Empty;
                /// <summary>
                /// 包的父ID(无为空)
                /// </summary>
                public Guid ParentId
                {
                        get => this._ParentId;
                        set => this._ParentId = value;
                }

                public object Clone()
                {
                        return this.MemberwiseClone();
                }
        }
}
