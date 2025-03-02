using System;
using System.IO;
using WeDonekRpc.HttpApiGateway.FileUp.Model;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpApiGateway.FileUp.Interface
{
    public interface IBlockUpFile : IBlockFile, IDisposable
    {
        /// <summary>
        /// 超时时间
        /// </summary>
        int TimeOut { get; }

        /// <summary>
        /// 文件MD5
        /// </summary>
        string FileMd5 { get; }
        /// <summary>
        /// 上传状态
        /// </summary>
        BlockUpState UpState { get; }

        /// <summary>
        /// 检查快上传状态
        /// </summary>
        /// <param name="index">块索引</param>
        /// <returns></returns>
        bool CheckIsUp ( int index );

        /// <summary>
        /// 获取数据流
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        Stream GetStream ( int index );

        /// <summary>
        /// 加载
        /// </summary>
        void Load ( UpComplete ev );

        /// <summary>
        /// 写入上传流
        /// </summary>
        /// <param name="file"></param>
        /// <param name="index"></param>
        bool WriteUpFile ( IUpFile file, int index );

        /// <summary>
        /// 获取分块信息
        /// </summary>
        /// <returns></returns>
        BlockDatum GetBlock ();
        void UpTimeOut ();
    }
}