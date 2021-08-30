using System;

namespace RpcClient.Interface
{
        public interface ITcpRouteCollect : IDisposable
        {
                IRoute[] GetRoutes();
                event Action<IRoute> RegRouteEvent;
                bool CheckIsExists(string route);
                void AddRoute(string name, RpcAction action);
                void AddRoute(string name, RpcOut action);

                void AddRoute(IRoute route);
                void AddRoute<T>(string name, RpcActionSource<T> action);
                void AddRoute<Result>(string name, RpcActionReturn<Result> action);
                void AddRoute<Result>(string name, RpcOutReturn<Result> action);
                void AddRoute<T, Result>(string name, RpcAction<T, Result> action);
                void AddRoute<T, Result>(string name, RpcActionPaging<T, Result> action);
                void AddRoute<T, Result>(string name, RpcActionPagingSource<T, Result> action);
                void AddRoute<T, Result>(string name, RpcActionSource<T, Result> action);
                void AddRoute<T, Result>(string name, RpcFunc<T, Result> action);
                void AddRoute<T, Result>(string name, RpcFuncPaging<T, Result> action);
                void AddRoute<T, Result>(string name, RpcFuncPagingSource<T, Result> action);
                void AddRoute<T, Result>(string name, RpcFuncSource<T, Result> action);
                void AddRoute<T, Result>(string name, RpcOut<T, Result> action);
                void AddRoute<T, Result>(string name, RpcOutPaging<T, Result> action);
                void AddRoute<T, Result>(string name, RpcOutPagingSource<T, Result> action);
                void AddRoute<T, Result>(string name, RpcOutSource<T, Result> action);
                void AddRoute<T>(string name, RpcAction<T> action);
                void AddRoute<T>(string name, RpcFunc<T> action);
                void AddRoute<T>(string name, RpcOut<T> action);
                void AddRoute<T>(string name, RpcOutSource<T> action);

                void AddRoute<T>(RpcAction<T> action);
                void AddRoute<T>(RpcFunc<T> action);
                void AddRoute<T>(RpcOut<T> action);
                void AddRoute<T>(RpcOutSource<T> action);
                void RemoveRoute(string name);
        }
}