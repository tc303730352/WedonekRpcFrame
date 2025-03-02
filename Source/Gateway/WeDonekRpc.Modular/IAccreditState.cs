namespace WeDonekRpc.Modular
{
    public interface IAccreditState : IUserState
    {
        /// <summary>
        /// 绑定授权码
        /// </summary>
        /// <param name="accreditId">授权码</param>
        /// <param name="groupVal">系统组</param>
        /// <param name="rpcMerId">集群Id</param>
        void BindAccreditId (string accreditId, string groupVal, long rpcMerId);

    }
}
