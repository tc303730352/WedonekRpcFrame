
using System;
using System.Text;

using SocketTcpClient.Enum;

using RpcHelper;

namespace SocketTcpClient.Model
{
        internal class DataPageInfo
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
                public PageType PageType;


                /// <summary>
                /// 数据类型（1 字符串类型 , 2 实体对象 3 字节流）
                /// </summary>
                public Enum.SendType DataType;

                /// <summary>
                /// 包头
                /// </summary>
                public byte[] Head;

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
                public string Type;
                /// <summary>
                /// 头的体接收的数目
                /// </summary>
                private int _HeadLen = 0;

                /// <summary>
                /// 头部体长度
                /// </summary>
                private int _HeadBodyLen = 0;


                private int _ValueLen = 0;

                /// <summary>
                /// 内容接收的数目
                /// </summary>
                private int _ContentNum = 0;

                private byte[] _HeadBody = null;

                private int _TypeStrLen = 0;

                private PageLoadProgress _LoadProgress = PageLoadProgress.加载中;

                /// <summary>
                /// 数据包加载进度
                /// </summary>
                public PageLoadProgress LoadProgress => this._LoadProgress;

                /// <summary>
                /// 错误信息
                /// </summary>
                public string ErrorCode
                {
                        get;
                        private set;
                }
                /// <summary>
                /// 加载数据
                /// </summary>
                /// <param name="data"></param>
                /// <returns></returns>
                public bool LoadHead(byte[] data, int len, ref int index)
                {
                        if (this._LoadProgress == PageLoadProgress.加载中)
                        {
                                if (this.Head == null)
                                {
                                        this.Head = new byte[SocketTools.HeadLen];
                                }
                                if (this._HeadLen != SocketTools.HeadLen)
                                {
                                        int num = len - index;
                                        if (num > SocketTools.HeadLen - this._HeadLen)
                                        {
                                                num = SocketTools.HeadLen - this._HeadLen;
                                        }
                                        Buffer.BlockCopy(data, index, this.Head, this._HeadLen, num);
                                        this._HeadLen += num;
                                        index += num;
                                        if (this._HeadLen != SocketTools.HeadLen)
                                        {
                                                return false;
                                        }
                                        this.InitHead();
                                        this._LoadProgress = PageLoadProgress.头部加载完成;
                                        return true;
                                }
                        }
                        return true;
                }
                public void InitHead()
                {
                        this.PageType = (PageType)this.Head[1];
                        this.DataType = (SendType)this.Head[2];
                        this._TypeStrLen = this.Head[8];
                        int len = this._TypeStrLen + 1;
                        this._ValueLen = this.Head[3];
                        if (this._ValueLen % 2 == 1)
                        {
                                this.IsCompression = true;
                                this._ValueLen -= 1;
                                len += (this._ValueLen * 2);
                        }
                        else
                        {
                                len += this._ValueLen;
                        }
                        this.DataId = BitConverter.ToInt32(this.Head, 4);
                        this._HeadBody = new byte[len];
                }
                private bool _InitHeadBody()
                {
                        int index = this._ValueLen;
                        if (this._ValueLen == 2)
                        {
                                this.DataLen = BitConverter.ToInt16(this._HeadBody, 0);
                                if (this.IsCompression)
                                {
                                        this.OriginalSize = BitConverter.ToInt16(this._HeadBody, index);
                                        index = 4;
                                }
                        }
                        else if (this._ValueLen == 4)
                        {
                                this.DataLen = BitConverter.ToInt32(this._HeadBody, 0);
                                if (this.IsCompression)
                                {
                                        this.OriginalSize = BitConverter.ToInt32(this._HeadBody, index);
                                        index = 8;
                                }
                        }
                        if (this._HeadBody[index] != Tools.CS(this.Head, this._HeadBody, index))
                        {
                                this._LoadProgress = PageLoadProgress.包校验错误;
                                return false;
                        }
                        index += 1;
                        this.Content = new byte[this.DataLen];
                        this.Type = Encoding.UTF8.GetString(this._HeadBody, index, this._TypeStrLen);
                        this._LoadProgress = PageLoadProgress.包体加载完成;
                        return true;
                }

                private bool _LoadHeadBody(byte[] data, int len, ref int index)
                {
                        if (index >= len)
                        {
                                return false;
                        }
                        if (this._LoadProgress == PageLoadProgress.头部加载完成)
                        {
                                int num = len - index;
                                if (num > this._HeadBody.Length - this._HeadBodyLen)
                                {
                                        num = this._HeadBody.Length - this._HeadBodyLen;
                                }
                                Buffer.BlockCopy(data, index, this._HeadBody, this._HeadBodyLen, num);
                                this._HeadBodyLen += num;
                                index += num;
                                if (this._HeadBodyLen != this._HeadBody.Length)
                                {
                                        return false;
                                }
                                return this._InitHeadBody();
                        }
                        return true;
                }
                public bool LoadData(byte[] data, int len, ref int index)
                {
                        if (this.LoadHead(data, len, ref index))
                        {
                                if (!this._LoadHeadBody(data, len, ref index))
                                {
                                        return false;
                                }
                                else if (this.DataLen == 0)
                                {
                                        this._LoadProgress = PageLoadProgress.加载完成;
                                        return true;
                                }
                                else if (index != len)
                                {
                                        int num = len - index;
                                        if (num > this.DataLen - this._ContentNum)
                                        {
                                                num = this.DataLen - this._ContentNum;
                                        }
                                        Buffer.BlockCopy(data, index, this.Content, this._ContentNum, num);
                                        this._ContentNum += num;
                                        index += num;
                                        if (this._ContentNum == this.DataLen)
                                        {
                                                this._LoadProgress = PageLoadProgress.加载完成;
                                                return true;
                                        }
                                }
                        }
                        return false;
                }
        }
}
