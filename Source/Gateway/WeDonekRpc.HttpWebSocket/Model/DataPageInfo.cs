using System;

namespace WeDonekRpc.HttpWebSocket.Model
{
    internal class DataPageInfo : DataPage
    {
        /// <summary>
        /// 头的体接收的数目
        /// </summary>
        private int _HeadLen = 0;

        private int _HeadBodyLen = 0;
        private int _ContentNum = 0;
        /// <summary>
        /// 掩码
        /// </summary>
        private byte[] _Mask = null;

        private volatile PageLoadProgress _LoadProgress = PageLoadProgress.加载中;

        /// <summary>
        /// 数据包加载进度
        /// </summary>
        public PageLoadProgress LoadProgress
        {
            get => this._LoadProgress;
            set => this._LoadProgress = value;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool LoadHead (byte[] data, int len, ref int index)
        {
            if (this._LoadProgress == PageLoadProgress.加载中)
            {
                if (this.Head == null)
                {
                    this.Head = new byte[2];
                }
                if (this._HeadLen != this.Head.Length)
                {
                    int num = len - index;
                    if (num > this.Head.Length - this._HeadLen)
                    {
                        num = this.Head.Length - this._HeadLen;
                    }
                    Buffer.BlockCopy(data, index, this.Head, this._HeadLen, num);
                    this._HeadLen += num;
                    index += num;
                    if (this._HeadLen != this.Head.Length)
                    {
                        return false;
                    }
                    this._InitHead();
                    this._LoadProgress = PageLoadProgress.头部加载完成;
                    return true;
                }
            }
            return true;
        }

        internal void InitPage ()
        {
            for (int i = 0; i < this.Content.Length; i++)
            {
                this.Content[i] = (byte)( this.Content[i] ^ this._Mask[i % 4] );
            }
        }

        private void _InitHead ()
        {
            this.Fin = ( this.Head[0] & 128 ) != 0;
            this.PageType = (PageType)( this.Head[0] & 15 );
            this.IsMask = ( this.Head[1] & 128 ) != 0;
            int len = this.Head[1] & 127;
            if (len == 127)
            {
                this.HeadBody = new byte[12];
            }
            else if (len == 126)
            {
                this.HeadBody = new byte[6];
            }
            else
            {
                this.Content = new byte[len];
                this.HeadBody = new byte[4];
            }
        }
        private bool _LoadHeadBody (byte[] data, int size, ref int index)
        {
            if (index >= size)
            {
                return false;
            }
            else if (this._LoadProgress == PageLoadProgress.头部加载完成)
            {
                int num = size - index;
                if (num > this.HeadBody.Length - this._HeadBodyLen)
                {
                    num = this.HeadBody.Length - this._HeadBodyLen;
                }
                Buffer.BlockCopy(data, index, this.HeadBody, this._HeadBodyLen, num);
                this._HeadBodyLen += num;
                index += num;
                if (this._HeadBodyLen != this.HeadBody.Length)
                {
                    return false;
                }
                else if (this._HeadBodyLen == 6)
                {
                    Array.Reverse(this.HeadBody, 0, 2);
                    ushort len = BitConverter.ToUInt16(this.HeadBody, 0);
                    this.Content = new byte[len];
                    this._Mask = new byte[4];
                    Buffer.BlockCopy(this.HeadBody, 2, this._Mask, 0, 4);
                }
                else if (this._HeadBodyLen == 12)
                {
                    Array.Reverse(this.HeadBody, 0, 8);
                    ulong len = BitConverter.ToUInt64(this.HeadBody, 0);
                    this.Content = new byte[len];
                    this._Mask = new byte[4];
                    Buffer.BlockCopy(this.HeadBody, 8, this._Mask, 0, 4);
                }
                else
                {
                    this._Mask = this.HeadBody;
                }
                this._LoadProgress = PageLoadProgress.包体加载完成;
                return true;
            }
            return true;
        }
        public bool LoadData (byte[] data, int len, ref int index)
        {
            if (this.LoadHead(data, len, ref index))
            {
                if (!this._LoadHeadBody(data, len, ref index))
                {
                    return false;
                }
                else if (index != len && this.Content.Length != 0)
                {
                    int num = len - index;
                    if (num > this.Content.Length - this._ContentNum)
                    {
                        num = this.Content.Length - this._ContentNum;
                    }
                    Buffer.BlockCopy(data, index, this.Content, this._ContentNum, num);
                    this._ContentNum += num;
                    index += num;
                    if (this._ContentNum == this.Content.Length)
                    {
                        this._LoadProgress = PageLoadProgress.加载完成;
                        return true;
                    }
                }
                else
                {
                    this._LoadProgress = PageLoadProgress.加载完成;
                    return true;
                }
            }
            return false;
        }
    }
}
