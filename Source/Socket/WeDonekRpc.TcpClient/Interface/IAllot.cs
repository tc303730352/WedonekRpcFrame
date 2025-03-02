using System;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.TcpClient.Model;

namespace WeDonekRpc.TcpClient.Interface
{
    public abstract class IAllot : ICloneable
    {
        private GetDataPage _Page;
        internal void InitInfo (GetDataPage page)
        {
            this._Page = page;
        }
        /// <summary>
        /// 数据内容
        /// </summary>
        public byte[] Content
        {
            get => this._Page.Content;
        }
        protected uint PageId
        {
            get => this._Page.PageId;
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        protected byte DataType
        {
            get => this._Page.DataType;
        }
        /// <summary>
        /// 客户端信息
        /// </summary>
        internal IClient ClientInfo
        {
            get => this._Page.Client;
        }

        /// <summary>
        /// 完成授权
        /// </summary>
        internal void ComplateAuthorization ()
        {
            this.ClientInfo?.AuthorizationComplate();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public string GetData ()
        {
            if (this._Page.Content == null)
            {
                return string.Empty;
            }
            return this.DataType == ConstDicConfig.StringSendType ? ToolsHelper.DeserializeStringData(this._Page.Content) : null;
        }

        public T GetData<T> ()
        {
            if (this._Page.Content == null)
            {
                return default;
            }
            return ToolsHelper.DeserializeData<T>(this._Page.Content);
        }

        public object Clone ()
        {
            return this.MemberwiseClone();
        }

        internal virtual object Action (ref string type)
        {
            return this.ActionInfo(type);
        }
        public virtual object ActionInfo (string type)
        {
            return null;
        }
        protected void Close ()
        {
            this.ClientInfo?.CloseClientCon();
        }
        protected void Close (int time)
        {
            this.ClientInfo?.CloseClientCon(time);
        }
    }
}
