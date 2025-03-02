
using System;
using System.Text;
using WeDonekRpc.IOSendInterface;

namespace WeDonekRpc.TcpClient.Model
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


        public byte[] Content
        {
            get
            {
                if (this.DataLen == 0)
                {
                    return null;
                }
                return this._Content.AsSpan(this.TypeLen, this.DataLen).ToArray();
            }
        }
        /// <summary>
        /// 包的类型(用户自定义)
        /// </summary>
        public string Type
        {
            get
            {
                return Encoding.UTF8.GetString(this._Content, 0, this.TypeLen);
            }
        }

        public int TypeLen = 0;

        public int HeadLen = 0;
        public int BodyLen = 0;
        public int ContentNum = 0;
        public PageLoadProgress LoadProgress = PageLoadProgress.加载中;

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
                    this.HeadLen = len;
                    len = 0;
                    return false;
                }
                else
                {
                    int num = ConstDicConfig.HeadLen - this.HeadLen;
                    if (num > len)
                    {
                        num = len;
                    }
                    Buffer.BlockCopy(data, index, this.Head, this.HeadLen, num);
                    len -= num;
                    index += num;
                    this.HeadLen = (this.HeadLen + num);
                    if (this.HeadLen != ConstDicConfig.HeadLen)
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
            this.TypeLen = this.Head[3];
            this.DataId = BitConverter.ToUInt32(this.Head, 4);
            this.DataLen = BitConverter.ToInt32(this.Head, 8);
            this.BodyLen = this.DataLen + this.TypeLen;
            this.LoadProgress = PageLoadProgress.头部加载完成;
        }
        private bool _LoadContent(byte[] data, ref int len, ref int index)
        {
            if (len >= this.BodyLen && this._Content == null)
            {
                this._Content = data.AsSpan(index, this.BodyLen).ToArray();
                len -= this.BodyLen;
                index += this.BodyLen;
                this.LoadProgress = PageLoadProgress.包待校验;
                return true;
            }
            else if (this._Content == null)
            {
                this._Content = new byte[this.BodyLen];
                Buffer.BlockCopy(data, index, this._Content, 0, len);
                this.ContentNum = len;
                len = 0;
                return false;
            }
            else
            {
                int num = this.BodyLen - this.ContentNum;
                if (num > len)
                {
                    num = len;
                }
                Buffer.BlockCopy(data, index, this._Content, this.ContentNum, num);
                this.ContentNum = this.ContentNum + num;
                index += num;
                len -= num;
                if (this.ContentNum == this.BodyLen)
                {
                    this.LoadProgress = PageLoadProgress.包待校验;
                    return true;
                }
            }
            return false;
        }
        private bool _CheckPage(byte[] data, ref int len, ref int index)
        {
            byte cs = SocketTools.CS(this.Head, this._Content);
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
            if (this.LoadProgress == PageLoadProgress.包待校验)
            {
                return this._CheckPage(data, ref len, ref index);
            }
            else if (this._LoadHead(data, ref len, ref index))
            {
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
