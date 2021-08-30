using System.Data;

using RpcSyncService.Model;

using SqlExecHelper;
using SqlExecHelper.Insert;

using RpcHelper;

namespace RpcSyncService.DAL
{
        internal class SystemLogDAL : SqlBasicClass
        {
                public SystemLogDAL() : base("SystemErrorLog", "RpcExtendService")
                {

                }

                public bool InsertSysLog(SysLog[] logs)
                {
                        IInsertTable table = this.InsertTable(logs.Length, 15);
                        table.Column = new TableColumn[]
                        {
                                new TableColumn("Id", SqlDbType.UniqueIdentifier),
                                new TableColumn("RpcMerId", SqlDbType.BigInt),
                                new TableColumn("TraceId", SqlDbType.VarChar,64),
                                new TableColumn("LogTitle", SqlDbType.NVarChar,50),
                                new TableColumn("LogShow", SqlDbType.NVarChar),
                                new TableColumn("GroupId", SqlDbType.BigInt),
                                new TableColumn("SystemTypeId", SqlDbType.BigInt),
                                new TableColumn("ServerId", SqlDbType.BigInt),
                                new TableColumn("LogGroup", SqlDbType.VarChar,50),
                                new TableColumn("LogType", SqlDbType.SmallInt),
                                new TableColumn("LogGrade", SqlDbType.SmallInt),
                                new TableColumn("ErrorCode", SqlDbType.VarChar,200),
                                new TableColumn("Exception", SqlDbType.NText),
                                new TableColumn("AttrList", SqlDbType.NText),
                                new TableColumn("AddTime", SqlDbType.DateTime)
                        };
                        logs.ForEach(a =>
                        {
                                table.AddRow(Tools.NewGuid(),
                                        a.RpcMerId,
                                        a.TraceId,
                                        a.Title,
                                        a.Content,
                                        a.GroupId,
                                        a.SystemTypeId,
                                        a.ServerId,
                                        a.LogGroup,
                                        a.LogType,
                                        a.LogGrade,
                                        a.ErrorCode,
                                        a.Exception,
                                        a.AttrList,
                                        a.AddTime);
                        });
                        return table.Save();
                }
        }
}
