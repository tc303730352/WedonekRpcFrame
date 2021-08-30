namespace RpcModular
{
        public interface IAccreditState : IUserState
        {
                /// <summary>
                /// 绑定授权码
                /// </summary>
                /// <param name="accreditId">授权码</param>
                /// <param name="groupId">组Id</param>
                /// <param name="rpcMerId">集群Id</param>
                void BindAccreditId(string accreditId, long groupId, long rpcMerId);
                /// <summary>
                /// 转JSON
                /// </summary>
                /// <returns></returns>
                string ToJson();
        }
}
