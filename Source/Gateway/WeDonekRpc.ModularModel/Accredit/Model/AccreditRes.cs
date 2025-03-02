using System;

namespace WeDonekRpc.ModularModel.Accredit.Model
{
    /// <summary>
    /// 授权信息
    /// </summary>
    public class AccreditRes
    {
        /// <summary>
        /// 授权凭证
        /// </summary>
        public string AccreditId
        {
            get;
            set;
        }
        /// <summary>
        /// 唯一码
        /// </summary>
        public string ApplyId { get; set; }
        /// <summary>
        /// 校验KEY（单点）
        /// </summary>
        public string CheckKey { get; set; }
        /// <summary>
        /// 创建者所属服务组
        /// </summary>
        public string SysGroup { get; set; }
        /// <summary>
        /// 创建者所属集群
        /// </summary>
        public long RpcMerId { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? Expire { get; set; }
    }
}
