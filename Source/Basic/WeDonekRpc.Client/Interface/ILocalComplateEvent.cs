using WeDonekRpc.Client.Model;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 本地完成事件
    /// </summary>
    public interface ILocalComplateEvent<T>
    {
        void Completed (T sender, LocalEventArgs e);
    }
}
