namespace SqlExecHelper.Config
{
        public class SqlConfig
        {
                static SqlConfig()
                {
                        SqlFactory = new SqlFactory();
                }
                private static volatile SqlExecType _SqlExecType = SqlExecType.close;
                public static SqlExecType SqlExecType
                {
                        get => _SqlExecType;
                        set
                        {
                                if (value != _SqlExecType)
                                {
                                        MyDAL.SetAuditRange(value);
                                        _SqlExecType = value;
                                }
                        }
                }
                public static ISqlFactory SqlFactory
                {
                        get;
                        set;
                }
                private static readonly RunParam _DefDropLockType = new RunParam("rowlock");
                private static readonly RunParam _DefSetLockType = new RunParam("rowlock");
                private static readonly RunParam _DefQueryLockType = new RunParam("nolock");

                internal static RunParam DropLockType => SqlRunParam.GetParam(_DefDropLockType);

                internal static RunParam SetLockType => SqlRunParam.GetParam(_DefSetLockType);

                internal static RunParam QueryLockType => SqlRunParam.GetParam(_DefQueryLockType);

                internal static RunParam DefQueryLockType => _DefQueryLockType;

                internal static int InMaxNum { get; set; } = 5;

                internal static int TempTableLimitNum = 150;

                internal static readonly int MxColumnNum = 2000;
        }
}
