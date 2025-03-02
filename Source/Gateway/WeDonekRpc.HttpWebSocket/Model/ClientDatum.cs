using System;

namespace WeDonekRpc.HttpWebSocket.Model
{
    /// <summary>
    /// 客户端标识
    /// </summary>
    internal class ClientDatum : IEquatable<ClientDatum>
    {
        public ClientDatum(Guid serverId, Guid clientId)
        {
            this.ServerId = serverId;
            this.ClientId = clientId;
        }
        /// <summary>
        /// 服务器ID
        /// </summary>
        public Guid ServerId
        {
            get;
        }
        /// <summary>
        /// 客户端ID
        /// </summary>
        public Guid ClientId
        {
            get;
        }

        public override bool Equals(object obj)
        {
            if (obj is ClientDatum i)
            {
                return i.ClientId == this.ClientId;
            }
            return false;
        }

        public bool Equals(ClientDatum other)
        {
            if (other == null)
            {
                return false;
            }
            return other.ClientId == this.ClientId;
        }

        public override int GetHashCode()
        {
            return this.ClientId.GetHashCode();
        }
    }
}
