using System.Collections.Generic;

using WeDonekRpc.Helper;

namespace WeDonekRpc.Model
{
    public class RefreshEventParam : Dictionary<string, string>
    {
        public RefreshEventParam ()
        {
        }

        public RefreshEventParam (Dictionary<string, string> param) : base(param)
        {
        }

        public new string this[string name]
        {
            get => this.TryGetValue(name, out string val) ? val : null;
            set => this.AddOrSet(name, value);
        }
    }
    public class RefreshRpc
    {
        /// <summary>
        /// 授权的Token
        /// </summary>
        public string TokenId
        {
            get;
            set;
        }
        /// <summary>
        /// 事件Key
        /// </summary>
        public string EventKey
        {
            get;
            set;
        }
        public RefreshEventParam Param
        {
            get;
            set;
        }
    }
}
