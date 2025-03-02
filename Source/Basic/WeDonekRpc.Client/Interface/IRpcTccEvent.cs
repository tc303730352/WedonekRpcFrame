namespace WeDonekRpc.Client.Interface
{
    [Attr.IgnoreIoc]
    public interface IRpcTccEvent
    {
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="tran"></param>
        void Commit(ICurTran tran);

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="tran"></param>
        void Rollback(ICurTran tran);
    }
}
