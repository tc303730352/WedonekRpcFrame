using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.ResourceShield.Model
{
    public class ShieldAddDatum
    {
        [NumValidate("rpc.store.mer.id.error", 1)]
        public long RpcMerId { get; set; }
        [NullValidate("rpc.store.server.type.null")]
        public string SystemType { get; set; }

        public long[] ServerId { get; set; }

        public int? VerNum { get; set; }

        /// <summary>
        /// 指令类型
        /// </summary>
        [EnumValidate("rpc.store.shield.type.error")]
        public ShieldType ShieldType { get; set; }

        /// <summary>
        /// 指令名或接口相对路径
        /// </summary>
        [NullValidate("rpc.store.resource.path.null")]
        [LenValidate("rpc.store.resource.path.len", 1, 100)]
        public string ResourcePath { get; set; }

        public DateTime? BeOverdueTime { get; set; }

        [LenValidate("rpc.store.shieId.show.len", 0, 100)]
        public string ShieIdShow { get; set; }
    }
}
