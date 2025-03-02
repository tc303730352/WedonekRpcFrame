using System;

namespace WeDonekRpc.Client.Track.Config
{
    /// <summary>
    /// Zipkin配置
    /// </summary>
    public class ZipkinConfig : IEquatable<ZipkinConfig>
    {
        /// <summary>
        /// 提交的URI地址
        /// </summary>
        public Uri PostUri
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            if (obj is ZipkinConfig i)
            {
                return i.PostUri == this.PostUri;
            }
            return false;
        }

        public bool Equals(ZipkinConfig other)
        {
            if (other == null)
            {
                return false;
            }
            return other.PostUri == this.PostUri;
        }

        public override int GetHashCode()
        {
            return this.PostUri.GetHashCode();
        }
    }
}
