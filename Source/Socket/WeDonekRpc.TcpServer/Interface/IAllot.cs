using System;
using System.Net;
using System.Threading;
using WeDonekRpc.Helper.Json;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.TcpServer.Model;

namespace WeDonekRpc.TcpServer.Interface
{
    public abstract class IAllot : ICloneable
    {
        private GetDataPage _Page;
        internal void InitInfo (ref GetDataPage page)
        {
            this._Page = page;
            this.Client = new ReplyClient(ref page);
        }
        protected uint PageId => this._Page.PageId;

        protected IPEndPoint ClientIp => this._Page.Client.RempteIp;


        public string Type => this._Page.Type;

        /// <summary>
        /// 数据内容
        /// </summary>
        public byte[] Content => this._Page.Content;

        /// <summary>
        /// 数据类型
        /// </summary>
        protected SendType DataType => (SendType)this._Page.DataType;
        /// <summary>
        /// 客户端信息
        /// </summary>
        internal IClient ClientInfo => this._Page.Client;
        /// <summary>
        /// 客户端
        /// </summary>
        public IIOClient Client { get; private set; }
        /// <summary>
        /// 完成授权
        /// </summary>
        /// <param name="bindParam">绑定参数</param>
        internal void ComplateAuthorization (string bindParam)
        {
            this.ClientInfo.AuthorizationComplate(bindParam);
        }
        private string _Str = null;
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        protected string GetData ()
        {
            if (this._Str == null)
            {
                this._Str = ToolsHelper.DeserializeStringData(this._Page.Content);
            }
            return this._Str;
        }
        protected bool TryGet<T> (out Nullable<T> data) where T : struct
        {
            string json = this.GetData();
            if (json == string.Empty)
            {
                data = null;
                return false;
            }
            data = JsonTools.Json<T>(json);
            return true;
        }
        protected T GetData<T> ()
        {
            string json = this.GetData();
            if (json == string.Empty)
            {
                return default;
            }
            return JsonTools.Json<T>(json);
        }
        protected T GetValue<T> () where T : struct
        {
            if (this.Content == null)
            {
                return default;
            }
            return (T)ToolsHelper.GetValue(typeof(T), this._Page.Content);
        }
        public object Clone ()
        {
            return this.MemberwiseClone();
        }

        public virtual object Action ()
        {
            return null;
        }
        protected void Close ()
        {
            this._Page.Client.CloseClientCon();
        }
        protected void Close (int time)
        {
            new Thread(new ThreadStart(delegate ()
            {
                Thread.Sleep(time);
                this.ClientInfo.CloseClientCon();
            })).Start();
        }
    }
}
