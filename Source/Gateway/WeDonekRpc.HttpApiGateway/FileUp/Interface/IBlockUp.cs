using WeDonekRpc.HttpApiGateway.FileUp.Model;

namespace WeDonekRpc.HttpApiGateway.FileUp.Interface
{
    public interface IBlockUp
    {
        /// <summary>
        /// POST数据
        /// </summary>
        UpFileData PostData { get; }
        /// <summary>
        /// 开始上传
        /// </summary>
        void BeginUp ( string taskKey, object extend );
        /// <summary>
        /// 上传完成
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        void UpComplate<T> ( T result );
    }
    public interface IBlockUp<T>
    {
        /// <summary>
        /// POST的数据
        /// </summary>
        UpFileData<T> PostData { get; }

        /// <summary>
        /// 开始上传
        /// </summary>
        void BeginUp ( string taskKey, object extend );
        /// <summary>
        /// 上传完成
        /// </summary>
        /// <typeparam name="Result"></typeparam>
        /// <param name="result"></param>
        void UpComplate<Result> ( Result result );
    }
}