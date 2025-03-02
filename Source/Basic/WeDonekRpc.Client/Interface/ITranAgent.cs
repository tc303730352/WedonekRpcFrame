using System;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 事务作用域(当前节点事务方法执行结束时销毁)
    /// </summary>
    public interface ITranScope : IDisposable
    {
    }
}