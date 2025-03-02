using System;
using System.Text;

using WeDonekRpc.HttpWebSocket.Config;

using WeDonekRpc.Helper;

namespace WeDonekRpc.HttpWebSocket.Model
{
        public class HttpHead
        {
                private static readonly byte _EndByte = 10;

                public HttpHead(byte[] myByte, int len)
                {
                        this._Stream = new byte[len];
                        Buffer.BlockCopy(myByte, 0, _Stream, 0, len);
                        this._EndIndex = len - 1;
                }
                private byte[] _Stream = null;
                private int _EndIndex = 0;
                private volatile bool _IsEnd = false;

                internal bool CheckHead()
                {
                        return this._Stream[0] == 71;
                }

                internal void LoadPage()
                {
                        string str = Encoding.UTF8.GetString(this._Stream, 0, this._EndIndex);
                        string[] heads = str.Replace("\0", string.Empty).Replace("\r", string.Empty).Split('\n');
                        if (!heads.IsExists(a => a.StartsWith("Upgrade")))
                        {
                                return;
                        }
                        string[] t = heads[0].Remove(0, 4).Split(' ');
                        this.Path = t[0].Trim();
                        this.HttpVer = t[1].Remove(0, 5).Trim();
                        heads.ForEach(a =>
                        {
                                t = a.Split(':');
                                string name = t[0];
                                if (name == "User-Agent")
                                {
                                        this.UserAgent = t[1].Trim();
                                }
                                else if (name == "Sec-WebSocket-Version")
                                {
                                        this.WebSocketVer = t[1].Trim();
                                }
                                else if (name == "Origin")
                                {
                                        this.Origin = t[1].Trim();
                                }
                                else if (name == "Sec-WebSocket-Version")
                                {
                                        this.WebSocketVer = t[1].Trim();
                                }
                                else if (name == "Sec-WebSocket-Key")
                                {
                                        this.WebSocketKey = t[1].Trim();
                                }
                        });
                        this._Stream = null;
                }
                /// <summary>
                /// 路径
                /// </summary>
                public string Path
                {
                        get;
                        private set;
                }
                /// <summary>
                /// HTTP版本号
                /// </summary>
                public string HttpVer { get; private set; }
                /// <summary>
                /// WebSocket版本
                /// </summary>
                public string WebSocketVer { get; private set; }
                /// <summary>
                /// 来源URI
                /// </summary>
                public string Origin { get; private set; }

                /// <summary>
                /// 用户头
                /// </summary>
                public string UserAgent { get; private set; }
                public string WebSocketKey { get; private set; }
                public bool IsEnd => this._IsEnd;

                internal bool AppendStream(byte[] myByte, int len)
                {
                        if ((this._Stream.Length + myByte.Length) >= PublicConfig.MaxHeadSize)
                        {
                                return false;
                        }
                        this._Stream = this._Stream.Add(myByte, len);
                        this._EndIndex = this._Stream.Length - 1;
                        return true;
                }

                internal bool CheckIsEnd()
                {
                        if(this._Stream[_EndIndex] != _EndByte)
                        {
                                this._EndIndex = this._Stream.FindLastIndex(a => a == _EndByte);
                        }
                        if (this._EndIndex == -1 || this._Stream[this._EndIndex - 2] != _EndByte)
                        {
                                return false;
                        }
                        else
                        {
                                this._IsEnd = true;
                                return true;
                        }
                }
        }
}
