using RpcClient.Interface;

using RpcHelper;

namespace RpcSyncService.Config
{
        internal class SyncConfig
        {
                static SyncConfig()
                {
                        RpcClient.RpcClient.Config.AddRefreshEvent(_InitConfig);
                }
                private static void _InitConfig(IConfigServer config, string name)
                {
                        if (name.StartsWith("tran") || name == string.Empty)
                        {
                                SyncConfig.TranRollbackRetryNum = config.GetConfigVal<short>("tran:TranRollbackRetryNum", 3);
                        }
                        if (name.StartsWith("error") || name == string.Empty)
                        {
                                SyncConfig.InfoLimitGrade = config.GetConfigVal("error:InfoLimitGrade", LogGrade.Information);
                                SyncConfig.ErrorLimitGrade = config.GetConfigVal("error:ErrorLimitGrade", LogGrade.ERROR);
                                string email = config.GetConfigVal("error:Reciver");
                                if (!email.IsNull())
                                {
                                        SyncConfig.Reciver = email.Split(';');
                                        SyncConfig.IsSendEmail = true;
                                        SyncConfig.EmailPwd = config.GetConfigVal("error:EmailPwd");
                                }
                                SyncConfig.DisplayName = config.GetConfigVal("error:DisplayName");
                                SyncConfig.EmailAccount = config.GetConfigVal("error:EmailAccount");
                        }
                }

                /// <summary>
                /// 待获取锁
                /// </summary>
                public const int WaitLock = 0;
                /// <summary>
                /// 获得锁
                /// </summary>
                public const int ObtainLock = 1;

                /// <summary>
                /// 释放锁            
                /// </summary>
                public const int ReleaseLock = 2;

                /// <summary>
                /// 是否发送Email
                /// </summary>
                public static bool IsSendEmail { get; set; } = false;
                /// <summary>
                /// 发件人显示名
                /// </summary>
                public static string DisplayName { get; private set; }
                /// <summary>
                /// 事务回滚重试数
                /// </summary>
                public static short TranRollbackRetryNum { get; internal set; } = 3;



                /// <summary>
                /// Email发送限定等级
                /// </summary>
                public static LogGrade InfoLimitGrade = LogGrade.ERROR;

                /// <summary>
                /// 错误发送限定等级
                /// </summary>
                public static LogGrade ErrorLimitGrade = LogGrade.ERROR;

                /// <summary>
                /// Email账户
                /// </summary>
                public static string EmailAccount = null;

                /// <summary>
                /// 收件人
                /// </summary>
                public static string[] Reciver = null;

                /// <summary>
                /// Email账户密码
                /// </summary>
                public static string EmailPwd = null;
        }
}
