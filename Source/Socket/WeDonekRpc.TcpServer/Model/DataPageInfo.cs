
using System;
using System.Text;
using WeDonekRpc.IOSendInterface;

namespace WeDonekRpc.TcpServer.Model
{
    internal class DataPageInfo
    {
        /// <summary>
        /// 数据的长度
        /// </summary>
        public int DataLen;

        /// <summary>
        /// 数据包的类型
        /// </summary>
        public byte PageType;


        /// <summary>
        /// 数据类型（1 字符串类型 , 2 实体对象 3 字节流）
        /// </summary>
        public byte DataType;

        /// <summary>
        /// 包头
        /// </summary>
        public byte[] Head;

        /// <summary>
        /// 数据内容
        /// </summary>
        private byte[] _Content;

        /// <summary>
        /// 数据包的唯一标示
        /// </summary>
        public uint DataId;

        /// <summary>
        /// 是否压缩
        /// </summary>
        public bool IsCompression;
        /// <summary>
        /// 包的类型(用户自定义)
        /// </summary>
        public string Type
        {
            get
            {
                return Encoding.UTF8.GetString(this._Content, 0, this._TypeLen);
            }
        }
        public byte[] Content
        {
            get
            {
                if (this.DataLen == 0)
                {
                    return null;
                }
                return this._Content.AsSpan(this._TypeLen, this.DataLen).ToArray();
            }
        }
        /// <summary>
        /// 头的体接收的数目
        /// </summary>
        private int _HeadLen = 0;

        /// <summary>
        /// 内容整长
        /// </summary>
        private int _BodyLen = 0;

        /// <summary>
        /// 内容接收的数目
        /// </summary>
        private int _ContentNum = 0;

        private int _TypeLen = 0;

        public volatile PageLoadProgress LoadProgress = PageLoadProgress.加载中;

        /// <summary>
        /// 加载数据头
        /// </summary>
        /// <param name="data"></param>
        /// <param name="len"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool _LoadHead(byte[] data, ref int len, ref int index)
        {
            if (this.LoadProgress == PageLoadProgress.加载中)
            {
                if (len >= ConstDicConfig.HeadLen && this.Head == null)
                {
                    this.Head = data.AsSpan(index, ConstDicConfig.HeadLen).ToArray();
                    len -= ConstDicConfig.HeadLen;
                    index += ConstDicConfig.HeadLen;
                    this._InitHead();
                    return len != 0;
                }
                else if (this.Head == null)
                {
                    this.Head = new byte[ConstDicConfig.HeadLen];
                    Buffer.BlockCopy(data, index, this.Head, 0, len);
                    this._HeadLen = len;
                    len = 0;
                    return false;
                }
                else
                {
                    int num = ConstDicConfig.HeadLen - this._HeadLen;
                    if (num > len)
                    {
                        num = len;
                    }
                    Buffer.BlockCopy(data, index, this.Head, this._HeadLen, num);
                    len -= num;
                    index += num;
                    this._HeadLen += num;
                    if (this._HeadLen != ConstDicConfig.HeadLen)
                    {
                        return false;
                    }
                    this._InitHead();
                    return len != 0;
                }
            }
            return true;
        }
        private void _InitHead()
        {
            this.PageType = this.Head[1];
            this.DataType = this.Head[2];
            if (this.DataType % 2 == 1)
            {
                this.IsCompression = true;
                this.DataType -= 1;
            }
            this._TypeLen = this.Head[3];
            this.DataId = BitConverter.ToUInt32(this.Head, 4);
            this.DataLen = BitConverter.ToInt32(this.Head, 8);
            this._BodyLen = this.DataLen + this._TypeLen;
            this.LoadProgress = PageLoadProgress.头部加载完成;
        }
        private bool _LoadContent(byte[] data, ref int len, ref int index)
        {
            if (len >= this._BodyLen && this._Content == null)
            {
                this._Content = data.AsSpan(index, this._BodyLen).ToArray();
                len -= this._BodyLen;
                index +=  this._BodyLen;
                this.LoadProgress = PageLoadProgress.包待校验;
                return true;
            }
            else if (this._Content == null)
            {
                this._Content = new byte[this._BodyLen];
                Buffer.BlockCopy(data, index, this._Content, 0, len);
                this._ContentNum = len;
                len = 0;
                return false;
            }
            else
            {
                int num = this._BodyLen - this._ContentNum;
                if (num > len)
                {
                    num = len;
                }
                Buffer.BlockCopy(data, index, this._Content, this._ContentNum, num);
                this._ContentNum = this._ContentNum + num;
                index += num;
                len -= num;
                if (this._ContentNum == this._BodyLen)
                {
                    this.LoadProgress = PageLoadProgress.包待校验;
                    return true;
                }
            }
            return false;
        }
        private bool _CheckPage(byte[] data, ref int len, ref int index)
        {
            byte cs = SocketHelper.CS(this.Head, this._Content);
            if (data[index] != cs)
            {
                this.LoadProgress = PageLoadProgress.包校验错误;
                return false;
            }
            this.LoadProgress = PageLoadProgress.加载完成;
            len--;
            index++;
            return true;
        }
        public bool LoadData(byte[] data, ref int len, ref int index)
        {
            if (this._LoadHead(data, ref len, ref index))
            {
                if(this.LoadProgress == PageLoadProgress.包待校验)
                {
                    return this._CheckPage(data, ref len, ref index);
                }
                if (!this._LoadContent(data, ref len, ref index))
                {
                    return false;
                }
                else if (len != 0)
                {
                    return this._CheckPage(data, ref len, ref index);
                }
            }
            return false;
        }
    }
}
