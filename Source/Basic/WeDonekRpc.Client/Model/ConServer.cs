using System;

namespace WeDonekRpc.Client.Model
{
    internal struct ConServer : IEquatable<ConServer>
    {
        public string ip;
        public int port;

        public override int GetHashCode ()
        {
            return this.ToString().GetHashCode();
        }
        public override bool Equals (object obj)
        {
            if (obj is ConServer i)
            {
                return i.ip == this.ip && i.port == this.port;
            }
            return false;
        }
        public override string ToString ()
        {
            return this.ip + ":" + this.port;
        }

        public bool Equals (ConServer other)
        {
            return other.ip == this.ip && this.port == other.port;
        }
    }
}
