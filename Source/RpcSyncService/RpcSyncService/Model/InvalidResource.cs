using System;

using SqlExecHelper;
namespace RpcSyncService.Model
{
        internal class InvalidResource
        {
                /// <summary>
                /// 模块Id
                /// </summary>
                public Guid ModularId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 版本号
                /// </summary>
                public string VerNum
                {
                        get;
                        set;
                }
                /// <summary>
                /// 最大版本号
                /// </summary>
                [SqlColumnType("ResourceVer", SqlFuncType.max)]
                public int MaxVer
                {
                        get;
                        set;
                }
                /// <summary>
                /// 最小版本号
                /// </summary>
                [SqlColumnType("ResourceVer", SqlFuncType.min)]
                public int MinVer
                {
                        get;
                        set;
                }

                /// <summary>
                /// 总量
                /// </summary>
                //[SqlColumnType("ResourceVer", SqlFuncType.count)]
                //public int Num
                //{
                //        get;
                //        set;
                //}
                public bool CheckIsInvalid()
                {
                        return this.MaxVer - this.MinVer >= 2;
                }
        }
}
