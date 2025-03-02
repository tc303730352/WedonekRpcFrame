﻿using WeDonekRpc.Model;
using SqlSugar;

namespace RpcExtend.Model.DB
{
    [SugarTable("RemoteServerType")]
    public class RemoteServerType
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// 服务组
        /// </summary>
        public long GroupId
        {
            get;
            set;
        }
        /// <summary>
        /// 类型值
        /// </summary>
        public string TypeVal
        {
            get;
            set;
        }
        /// <summary>
        /// 服务名
        /// </summary>
        public string SystemName
        {
            get;
            set;
        }

        /// <summary>
        /// 负载均衡的方式
        /// </summary>
        public BalancedType BalancedType
        {
            get;
            set;
        }
        /// <summary>
        /// 默认端口号
        /// </summary>
        public int DefPort
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            get;
            set;
        }
    }
}
