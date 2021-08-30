using System;
using System.Collections.Generic;

using RpcModel.Tran.Model;

namespace RpcClient.Interface
{
        public interface IRpcTranCollect
        {
                /// <summary>
                /// 是否处在事务中
                /// </summary>
                bool IsExecTran { get; }
                /// <summary>
                /// 获取事务状态
                /// </summary>
                /// <returns></returns>
                RpcTranState GetTranState();
                /// <summary>
                /// 获取事务状态
                /// </summary>
                /// <param name="tranId">事务</param>
                /// <returns>事务状态</returns>
                RpcTranState GetTranState(ICurTran tran);

                /// <summary>
                /// 注册事务
                /// </summary>
                /// <typeparam name="T">事务发起参数</typeparam>
                /// <param name="name">事务名</param>
                /// <param name="rollback">回滚委托</param>
                void RegTran<T>(string name, Action<T, string> rollback) where T : class;
                /// <summary>
                /// 注册事务
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="rollback"></param>
                void RegTran<T>(Action<T, string> rollback) where T : class;
                /// <summary>
                /// 注册事务
                /// </summary>
                /// <param name="name"></param>
                /// <param name="rollback"></param>
                void RegTran(string name, Action<string> rollback);
                /// <summary>
                /// 注册事务
                /// </summary>
                /// <typeparam name="Extend"></typeparam>
                /// <param name="name"></param>
                /// <param name="rollback"></param>
                void RegTran<Extend>(string name, Action<Extend> rollback);
                /// <summary>
                /// 注册事务
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <typeparam name="Extend"></typeparam>
                /// <param name="name"></param>
                /// <param name="rollback"></param>
                void RegTran<T, Extend>(string name, Action<T, Extend> rollback) where T : class;
                /// <summary>
                /// 注册事务
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <typeparam name="Extend"></typeparam>
                /// <param name="rollback"></param>
                void RegTran<T, Extend>(Action<T, Extend> rollback) where T : class;
                /// <summary>
                /// 设置事务扩展
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="extend"></param>
                void SetTranExtend<T>(T extend) where T : class;
                /// <summary>
                /// 设置事务扩展
                /// </summary>
                /// <param name="extend"></param>
                void SetTranExtend(string extend);
                /// <summary>
                /// 设置事务扩展
                /// </summary>
                /// <param name="extend"></param>
                void SetTranExtend(Dictionary<string, object> extend);
        }
}