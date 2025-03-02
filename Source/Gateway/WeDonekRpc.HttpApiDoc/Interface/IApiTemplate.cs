using WeDonekRpc.HttpApiDoc.Model;

namespace WeDonekRpc.HttpApiDoc.Interface
{
    public interface IApiTemplate
    {
        /// <summary>
        /// 错误说明
        /// </summary>
        ApiAttrShow ErrorMsg
        {
            get;
            set;
        }
        /// <summary>
        /// 错误码
        /// </summary>
        ApiAttrShow ErrorCode
        {
            get;
            set;
        }
        /// <summary>
        /// 数据
        /// </summary>
        ApiAttrShow Data
        {
            get;
            set;
        }

        /// <summary>
        /// 数据总量
        /// </summary>
        ApiAttrShow Count
        {
            get;
            set;
        }
        /// <summary>
        /// 分页数据
        /// </summary>
        ApiAttrShow PagingData
        {
            get;
            set;
        }
    }
}
