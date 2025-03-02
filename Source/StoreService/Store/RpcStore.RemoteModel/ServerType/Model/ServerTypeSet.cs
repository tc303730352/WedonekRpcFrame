using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.ServerType.Model
{
    /// <summary>
    /// 服务类型修改信息
    /// </summary>
    public class ServerTypeSet
    {

        /// <summary>
        /// 服务名
        /// </summary>
        [NullValidate("rpc.store.sys.name.null")]
        [LenValidate("rpc.store.sys.name.len", 2, 50)]
        [FormatValidate("rpc.store.sys.name.error", ValidateFormat.中文字母数字)]
        public string SystemName
        {
            get;
            set;
        }

        /// <summary>
        /// 默认端口
        /// </summary>
        [NumValidate("rpc.store.sys.def.port.error", 1)]
        public int DefPort
        {
            get;
            set;
        }
        /// <summary>
        /// 服务类型
        /// </summary>
        [EnumValidate("rpc.store.service.type.error", typeof(RpcServerType))]
        public RpcServerType ServiceType
        {
            get;
            set;
        }
    }
}
