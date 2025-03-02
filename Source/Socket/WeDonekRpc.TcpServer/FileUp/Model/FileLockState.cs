using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
namespace WeDonekRpc.TcpServer.FileUp.Model
{
        internal class FileLockState
        {
                /// <summary>
                /// 文件大小
                /// </summary>
                public long FileSize
                {
                        get;
                        set;
                }
                private long _AlreadyUpNum = 0;

                /// <summary>
                /// 块大小
                /// </summary>
                public int BlockSize
                {
                        get;
                        set;
                }
                /// <summary>
                /// 块数量
                /// </summary>
                public ushort BlockNum
                {
                        get;
                        set;
                }

                /// <summary>
                /// 块状态
                /// </summary>
                public bool[] BlockState
                {
                        get;
                        set;
                }
                /// <summary>
                /// 是否是MD5的校验方式
                /// </summary>
                public bool IsMd5
                {
                        get;
                        set;
                }
                /// <summary>
                /// 文件Sign
                /// </summary>
                public string FileSign
                {
                        get;
                        set;
                }
                private static readonly byte _OKByte = 1;
                private static readonly int _HeadLen = 47;
                /// <summary>
                /// 加载
                /// </summary>
                /// <param name="stream"></param>
                public void Load(Stream stream)
                {
                        byte[] myByte = new byte[stream.Length];
                        stream.Position = 0;
                        stream.Read(myByte, 0, myByte.Length);
                        this.FileSize = BitConverter.ToInt64(myByte, 0);
                        this.BlockSize = BitConverter.ToInt32(myByte, 8);
                        this.BlockNum = BitConverter.ToUInt16(myByte, 12);
                        this.IsMd5 = myByte[14] == _OKByte;
                        this.FileSign = Encoding.UTF8.GetString(myByte, 15, 32);
                        this.BlockState = new bool[this.BlockNum];
                        long num = 0;
                        for (int i = _HeadLen, k = 0; i < myByte.Length; i++, k++)
                        {
                                if (myByte[i] == _OKByte)
                                {
                                        this.BlockState[k] = true;
                                        num += this.BlockSize;
                                }
                        }
                        if (num >= this.FileSize)
                        {
                                this._AlreadyUpNum = this.FileSize;
                        }
                        else
                        {
                                this._AlreadyUpNum = num;
                        }
                }
                /// <summary>
                /// 初始化
                /// </summary>
                /// <param name="stream"></param>
                public void Init(Stream stream)
                {
                        stream.SetLength(_HeadLen + this.BlockNum);
                        stream.Position = 0;
                        byte[] myByte = BitConverter.GetBytes(this.FileSize);
                        stream.Write(myByte, 0, myByte.Length);
                        myByte = BitConverter.GetBytes(this.BlockSize);
                        stream.Write(myByte, 0, myByte.Length);
                        myByte = BitConverter.GetBytes(this.BlockNum);
                        stream.Write(myByte, 0, myByte.Length);
                        if (this.IsMd5)
                        {
                                stream.WriteByte(1);
                        }
                        else
                        {
                                stream.WriteByte(0);
                        }
                        myByte = Encoding.UTF8.GetBytes(this.FileSign);
                        stream.Write(myByte, 0, myByte.Length);
                        stream.Flush();
                        this.BlockState = new bool[this.BlockNum];
                }

                public FileUpState GetUpState()
                {
                        int begin = Array.FindLastIndex(this.BlockState, a => a);
                        FileUpState state = new FileUpState
                        {
                                AlreadyUpNum = this._AlreadyUpNum,
                                BlockSize = this.BlockSize,
                                BlockNum = this.BlockNum,
                                BeginBlock = begin
                        };
                        if (begin != -1)
                        {
                                List<int> list = new List<int>(begin + 1);
                                for (int i = 0; i < begin; i++)
                                {
                                        if (!this.BlockState[i])
                                        {
                                                list.Add(i);
                                        }
                                }
                                state.NullBlock = list.ToArray();
                        }
                        return state;
                }
                internal void _Save(Stream file, int begin, byte val)
                {
                        file.Position = begin;
                        file.WriteByte(val);
                }
                internal bool BeginWrite(ushort blockId, out int position)
                {
                        if (!this.BlockState[blockId])
                        {
                                position = blockId * this.BlockSize;
                                return true;
                        }
                        position = 0;
                        return false;
                }

                internal bool EndWrite(Stream file, ushort blockId, int count)
                {
                        if (!this.BlockState[blockId])
                        {
                                this.BlockState[blockId] = true;
                                long sum = Interlocked.Add(ref this._AlreadyUpNum, count);
                                this._Save(file, _HeadLen + blockId, _OKByte);
                                return sum >= this.FileSize;
                        }
                        return false;
                }
        }
}
