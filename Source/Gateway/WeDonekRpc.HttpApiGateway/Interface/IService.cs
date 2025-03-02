using System;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    internal interface IService : IApiService
    {
        /// <summary>
        /// IOC域
        /// </summary>
        IocScope Scope { get; }
        /// <summary>
        /// 是否错误
        /// </summary>
        bool IsError { get; }
        /// <summary>
        /// 最新错误信息
        /// </summary>
        string LastError { get; }
        /// <summary>
        /// 初始化服务
        /// </summary>
        /// <param name="modular"></param>
        void InitService ( IApiModular modular );
        /// <summary>
        /// 回复错误
        /// </summary>
        /// <param name="error"></param>
        void ReplyError ( string error );
        /// <summary>
        /// 回复错误
        /// </summary>
        /// <param name="error"></param>
        void ReplyError ( ErrorException error );
        /// <summary>
        /// 回复成功
        /// </summary>
        void Reply ();
        /// <summary>
        /// 回复成功
        /// </summary>
        /// <param name="result"></param>
        void Reply ( object result );
        /// <summary>
        /// 回复成功
        /// </summary>
        /// <param name="response"></param>
        void Reply ( IResponse response );
        /// <summary>
        /// 回复错误
        /// </summary>
        /// <param name="show"></param>
        /// <param name="e"></param>
        void ReplyError ( string show, Exception e );
        /// <summary>
        /// 检查授权码
        /// </summary>
        /// <param name="accreditSet"></param>
        void CheckAccredit ( ApiAccreditSet accreditSet );
        /// <summary>
        /// 检查请求缓存
        /// </summary>
        /// <param name="etag"></param>
        /// <param name="toUpdateTime"></param>
        /// <returns></returns>
        bool CheckCache ( string etag, string toUpdateTime );
        void Dispose ();
        /// <summary>
        /// 请求结束
        /// </summary>
        void End ();
        /// <summary>
        /// 初始化身份标识
        /// </summary>
        void InitIdentity ();
    }
}
