using System;

namespace RpcClient.Track.Config
{
        public class ZipkinConfig
        {
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
                public override int GetHashCode()
                {
                        return this.PostUri.GetHashCode();
                }
        }
}
