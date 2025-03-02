using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WeDonekRpc.ModularModel.Resource.Model
{
    public class ResourceDatum : IEqualityComparer<ResourceDatum>
    {
        /// <summary>
        /// 资源路径
        /// </summary>
        public string ResourcePath
        {
            get;
            set;
        }
        /// <summary>
        /// 完整路径
        /// </summary>
        public string FullPath
        {
            get;
            set;
        }
        /// <summary>
        /// 资源路径
        /// </summary>
        public string ResourceShow
        {
            get;
            set;
        }
        public override bool Equals (object obj)
        {
            if (obj is ResourceDatum i)
            {
                return i.ResourcePath == this.ResourcePath;
            }
            return false;
        }
        public override int GetHashCode ()
        {
            return this.ResourcePath.GetHashCode();
        }

        public bool Equals (ResourceDatum x, ResourceDatum y)
        {
            return x.ResourcePath == y.ResourcePath;
        }

        public int GetHashCode ([DisallowNull] ResourceDatum obj)
        {
            return obj.ResourcePath.GetHashCode();
        }
    }
}
