namespace RpcCentral.Model
{
    public class RunEnvironment
    {

        /// <summary>
        /// 当前用户身份标识
        /// </summary>
        public string RunUserIdentity { get; set; }
        /// <summary>
        /// 是否是管理员身份运行
        /// </summary>
        public bool RunIsAdmin { get; set; }
    }
}
