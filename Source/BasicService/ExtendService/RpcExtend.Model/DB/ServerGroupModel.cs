﻿using SqlSugar;

namespace RpcExtend.Model.DB
{
    [SugarTable("ServerGroup")]
    public class ServerGroupModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        public string TypeVal
        {
            get;
            set;
        }
        public string GroupName
        {
            get;
            set;
        }
    }
}
