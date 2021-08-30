using RpcModel;

namespace RpcClient
{
        #region 无数据返回的委托
        public delegate bool RpcOut<T>(T data, out string error);
        public delegate bool RpcOut(out string error);
        public delegate bool RpcOutSource<T>(T data, MsgSource source, out string error);

        public delegate void RpcAction();
        public delegate void RpcAction<T>(T data);
        public delegate void RpcActionSource<T>(T data, MsgSource source);
        #endregion

        #region 返回数据的委托
        public delegate bool RpcOut<T, Result>(T data, out Result result, out string error);
        public delegate bool RpcOutSource<T, Result>(T data, MsgSource source, out Result result, out string error);
        public delegate bool RpcOutReturn<Result>(out Result result, out string error);

        public delegate void RpcActionSource<T, Result>(T data, MsgSource source, out Result result);
        public delegate void RpcAction<T, Result>(T data, out Result result);
        public delegate void RpcActionReturn<Result>(out Result result);

        public delegate Result RpcFunc<T, Result>(T data);

        public delegate Result RpcFunc<Result>();

        public delegate Result RpcFuncSource<T, Result>(T data, MsgSource source);

        #endregion

        #region 返回分页数据的委托
        public delegate bool RpcOutPagingSource<T, Result>(T data, MsgSource source, out Result[] results, out long count, out string error);
        public delegate bool RpcOutPaging<T, Result>(T data, out Result[] results, out long count, out string error);


        public delegate void RpcActionPaging<T, Result>(T data, out Result[] results, out long count);
        public delegate void RpcActionPagingSource<T, Result>(T data, MsgSource source, out Result[] results, out long count);


        public delegate Result[] RpcFuncPaging<T, Result>(T data, out long count);
        public delegate Result[] RpcFuncPagingSource<T, Result>(T data, MsgSource source, out long count);
        #endregion
}
