using RpcStore.RemoteModel;
using RpcStore.RemoteModel.SysConfig.Model;

namespace RpcStore.Model.SysConfig
{
    public class SysConfigSetParam : SysConfigSet
    {
     
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime ToUpdateTime
        {
            get;
            set;
        }
    }
}
