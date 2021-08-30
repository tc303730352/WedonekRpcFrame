namespace RpcModularModel.Accredit.Model
{
        /// <summary>
        /// 授权信息
        /// </summary>
        public class AccreditDatum
        {
                /// <summary>
                /// 申请的ID
                /// </summary>
                public string ApplyId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 角色类型
                /// </summary>
                public string RoleType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 授权版本号
                /// </summary>
                public long StateVer { get; set; }
                /// <summary>
                /// 状态值
                /// </summary>
                public string State { get; set; }
                /// <summary>
                /// 授权信息
                /// </summary>
                public AccreditRes Accredit
                {
                        get;
                        set;
                }
        }
}
