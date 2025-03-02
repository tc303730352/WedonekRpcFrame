using System;

namespace WeDonekRpc.HttpApiDoc.Model
{
    internal class ApiGroup : IEquatable<ApiGroup>
    {
        private static int _Id = 0;
        public ApiGroup(string title, Uri uri, string path)
        {
            this.Id = System.Threading.Interlocked.Increment(ref _Id);
            this.Name = title;
            this.RootUri = uri;
            this.Path = path.ToLower();
        }
        public int Id
        {
            get;
        }

        public string Name
        {
            get;
        }
        public Uri RootUri { get; }
        public string Path
        {
            get;
        }

        public bool CheckIsGroup(string uri)
        {
            return uri == this.Path;
        }
        /// <summary>
        /// 获取类的HashCode做比较
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Path.GetHashCode();
        }
        /// <summary>
        /// 比较相等(根据路径URI)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is ApiGroup i && i.Path == this.Path;
        }

        public bool Equals(ApiGroup other)
        {
            if(other == null)
            {
                return false;
            }
            return other.Path == this.Path;
        }
    }
}
