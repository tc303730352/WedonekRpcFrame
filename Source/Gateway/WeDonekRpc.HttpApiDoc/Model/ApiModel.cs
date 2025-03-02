using System;
using WeDonekRpc.ApiGateway;

namespace WeDonekRpc.HttpApiDoc.Model
{
    internal class ApiModel
    {
        /// <summary>
        /// APIId
        /// </summary>
        public string Id
        {
            get;
            set;
        }
        public GatewayType GatewayType { get; set; }
        /// <summary>
        /// 接口类型
        /// </summary>
        public ApiType ApiType { get; set; }
        /// <summary>
        /// 接口说明
        /// </summary>
        public string Show
        {
            get;
            set;
        }
        /// <summary>
        /// 接口URI
        /// </summary>
        public Uri Uri
        {
            get;
            set;
        }

        /// <summary>
        /// 请求方式
        /// </summary>
        public string RequestMethod
        {
            get;
            set;
        }
        /// <summary>
        /// API组Id
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// 请求头信息
        /// </summary>
        public RequestHeader[] Header { get; set; }
        /// <summary>
        /// 是否需要登陆
        /// </summary>
        public bool IsAccredit { get; set; }
        /// <summary>
        /// 访问权限
        /// </summary>
        public string Prower { get; set; }
        public bool IsIdentity { get; set; }
    }
}
