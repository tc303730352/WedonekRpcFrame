using System.Threading.Tasks;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.RouteDelegate
{
    #region 无数据返回的委托

    public delegate void RpcAction ();
    public delegate void RpcAction<T> ( T data );
    public delegate void RpcActionSource<T> ( T data, MsgSource source );
    #endregion

    #region 返回数据的委托

    public delegate Result RpcFunc<T, Result> ( T data );

    public delegate Result RpcFunc<Result> ();

    public delegate Result RpcFuncSource<T, Result> ( T data, MsgSource source );

    #endregion

    #region 无数据返回的异步委托

    public delegate Task RpcTaskAction ();
    public delegate Task RpcTaskAction<T> ( T data );
    public delegate Task RpcTaskActionSource<T> ( T data, MsgSource source );
    #endregion

    #region 返回数据的异步委托

    public delegate Task<Result> RpcTaskFunc<T, Result> ( T data );

    public delegate Task<Result> RpcTaskFunc<Result> ();

    public delegate Task<Result> RpcTaskFuncSource<T, Result> ( T data, MsgSource source );

    #endregion


}
