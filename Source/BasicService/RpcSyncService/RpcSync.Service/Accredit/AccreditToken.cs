namespace RpcSync.Service.Accredit
{
    /// <summary>
    /// 授权码信息
    /// </summary>
    public class AccreditToken
    {
        /// <summary>
        /// 授权ID
        /// </summary>
        public string AccreditId { get; set; }

        /// <summary>
        /// 申请唯一键
        /// </summary>
        public string ApplyId { get; set; }
        /// <summary>
        /// 检验Key
        /// </summary>
        public string CheckKey { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime? Expire { get; set; }
        /// <summary>
        /// 父级授权码
        /// </summary>
        public string PAccreditId { get; set; }
        /// <summary>
        /// 角色类型
        /// </summary>
        public string RoleType { get; set; }
        /// <summary>
        /// 所属集群ID
        /// </summary>
        public long RpcMerId { get; set; }
        /// <summary>
        /// 授权状态值
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 状态值版本
        /// </summary>
        public int StateVer { get; set; }
        /// <summary>
        /// 注册的服务组
        /// </summary>
        public string SysGroup { get; set; }
        /// <summary>
        /// 注册的服务类别
        /// </summary>
        public string SystemType { get; set; }
    }
}
