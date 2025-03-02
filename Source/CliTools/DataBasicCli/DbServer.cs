using System.Reflection;
using System.Text;
using DataBasicCli.Model;
using DataBasicCli.RpcExtendService;
using DataBasicCli.RpcService;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;
using SqlSugar;
namespace DataBasicCli
{
    public class DbServer
    {
        public static void InitRpcService ()
        {
            IConfigSection section = LocalConfig.Local.GetSection("db");
            DbModel db = section.GetValue<DbModel>("RpcService");
            if (db != null)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"template\RpcService.json");
                if (!File.Exists(path))
                {
                    Console.WriteLine("未找到数据摸版文件，文件路径：" + path);
                    return;
                }
                string json = File.ReadAllText(path);
                RpcServiceTemplate template = json.Json<RpcServiceTemplate>();
                using (ISqlSugarClient client = _CreateClient(db))
                {
                    client.Insertable(template.Region).AddQueue();
                    client.Insertable(template.Control).AddQueue();
                    client.Insertable(template.Dict).AddQueue();
                    client.Insertable(template.DictItem).AddQueue();
                    client.Insertable(template.RpcMer).AddQueue();
                    client.Insertable(template.ServerGroup).AddQueue();
                    client.Insertable(template.ServerType).AddQueue();
                    client.Insertable(template.SysConfig).AddQueue();
                    client.Insertable(template.Servers).AddQueue();
                    client.Insertable(template.RemoteServerGroup).AddQueue();
                    client.Insertable(template.TransmitScheme).AddQueue();
                    if (!template.Errors.IsNull())
                    {
                        client.Insertable(template.Errors).AddQueue();
                    }
                    if (!template.ErrorLang.IsNull())
                    {
                        client.Insertable(template.ErrorLang).AddQueue();
                    }
                    _ = client.SaveQueues();
                    Console.WriteLine("导入数据成功！");
                }
            }
            else
            {
                Console.WriteLine("未找到RpcService的链接字符串！");
            }
        }
        public static void InitRpcExtendService ()
        {
            IConfigSection section = LocalConfig.Local.GetSection("db");
            DbModel db = section.GetValue<DbModel>("RpcExtendService");
            if (db != null)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"template\RpcExtendService.json");
                if (!File.Exists(path))
                {
                    Console.WriteLine("未找到数据摸版文件，文件路径：" + path);
                    return;
                }
                string json = File.ReadAllText(path);
                RpcExtendServiceTemplate template = json.Json<RpcExtendServiceTemplate>();
                using (ISqlSugarClient client = _CreateClient(db))
                {
                    client.Insertable(template.Task).AddQueue();
                    client.Insertable(template.TaskItem).AddQueue();
                    client.Insertable(template.TaskPlan).AddQueue();
                    client.Insertable(template.EventConfig).AddQueue();
                    client.Insertable(template.EventSwitch).AddQueue();
                    int row = client.SaveQueues();
                    Console.WriteLine("导入数据成功！");
                }
            }
            else
            {
                Console.WriteLine("未找到RpcService的链接字符串！");
            }
        }
        private static SqlSugarClient _CreateClient (DbModel db)
        {
            ConnectionConfig con = new ConnectionConfig
            {
                ConnectionString = db.ConnectionString,
                ConfigId = "default",
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
                DbType = db.DbType,
                LanguageType = LanguageType.Chinese
            };
            return new SqlSugarClient(con);
        }
        /// <summary>
        /// 创建RpcExtendService摸版
        /// </summary>
        public static void CreateRpcExtendServiceTemplate ()
        {
            IConfigSection section = LocalConfig.Local.GetSection("db");
            DbModel db = section.GetValue<DbModel>("RpcExtendService");
            if (db != null)
            {
                ConnectionConfig con = new ConnectionConfig
                {
                    ConnectionString = db.ConnectionString,
                    ConfigId = "default",
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute,
                    DbType = db.DbType,
                    LanguageType = LanguageType.Chinese
                };
                using (SqlSugarClient client = new SqlSugarClient(con))
                {
                    RpcExtendServiceTemplate template = new RpcExtendServiceTemplate
                    {
                        TaskItem = client.Queryable<AutoTaskItemModel>().ToArray(),
                        Task = client.Queryable<AutoTaskModel>().ToArray(),
                        TaskPlan = client.Queryable<AutoTaskPlanModel>().ToArray(),
                        EventConfig = client.Queryable<SystemEventConfigModel>().ToArray(),
                        EventSwitch = client.Queryable<ServerEventSwitchModel>().ToArray()
                    };
                    string json = template.ToJson();
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"template\RpcExtendService.json");
                    Tools.WriteText(path, json, Encoding.UTF8);
                    Console.WriteLine("RpcExtendService摸版生成成功！");
                }
            }
        }
        /// <summary>
        /// 创建RpcService摸版
        /// </summary>
        public static void CreateRpcServiceTemplate ()
        {
            IConfigSection section = LocalConfig.Local.GetSection("db");
            DbModel db = section.GetValue<DbModel>("RpcService");
            if (db != null)
            {
                ConnectionConfig con = new ConnectionConfig
                {
                    ConnectionString = db.ConnectionString,
                    ConfigId = "default",
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute,
                    DbType = db.DbType,
                    LanguageType = LanguageType.Chinese
                };
                using (SqlSugarClient client = new SqlSugarClient(con))
                {
                    RpcServiceTemplate template = new RpcServiceTemplate
                    {
                        RpcMer = client.Queryable<RpcMerModel>().ToArray(),
                        Region = client.Queryable<ServerRegionModel>().ToArray(),
                        Control = client.Queryable<RpcControlModel>().ToArray(),
                        Dict = client.Queryable<DictCollect>().ToArray(),
                        DictItem = client.Queryable<DictItemModel>().ToArray(),
                        Servers = client.Queryable<RemoteServerConfigModel>().ToArray(),
                        RemoteServerGroup = client.Queryable<RemoteServerGroupModel>().ToArray(),
                        ServerType = client.Queryable<RemoteServerTypeModel>().ToArray(),
                        ServerGroup = client.Queryable<ServerGroupModel>().ToArray(),
                        SysConfig = client.Queryable<SysConfigModel>().ToArray(),
                        TransmitScheme = client.Queryable<ServerTransmitSchemeModel>().ToArray(),
                        Errors = client.Queryable<ErrorCollectModel>().ToArray(),
                        ErrorLang = client.Queryable<ErrorLangMsgModel>().ToArray()
                    };
                    string json = template.ToJson();
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"template\RpcService.json");
                    Tools.WriteText(path, json, Encoding.UTF8);
                    Console.WriteLine("RpcService摸版生成成功！");
                }
            }
        }
        private static void _InitDb (DbModel db, string name)
        {
            DbType dbType = db.DbType;
            ConnectionConfig con = new ConnectionConfig
            {
                ConnectionString = db.ConnectionString,
                ConfigId = "default",
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
                DbType = db.DbType,
                LanguageType = LanguageType.Chinese,
                MoreSettings = new ConnMoreSettings
                {
                    SqlServerCodeFirstNvarchar = false
                },
                ConfigureExternalServices = new ConfigureExternalServices
                {
                    EntityService = (c, p) =>
                    {
                        if (dbType != DbType.SqlServer && p.DataType == "smalldatetime")
                        {
                            p.DataType = "datetime";
                        }
                        else if (dbType == DbType.SqlServer && p.DataType == null && p.UnderType == PublicDataDic.StrType)
                        {
                            p.DataType = "nvarchar";
                        }
                        if (c.PropertyType.Name == PublicDataDic.Nullable && p.IsNullable == false)
                        {
                            p.IsNullable = true;
                        }
                    }
                }
            };

            Assembly type = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "DataBasicCli");
            string key = "DataBasicCli." + name;
            Type[] types = type.GetTypes().Where(c => c.Namespace == key).ToArray();
            if (types.Length > 0)
            {
                using (SqlSugarClient client = new SqlSugarClient(con))
                {
                    List<DbTableInfo> tables = client.DbMaintenance.GetTableInfoList();
                    types.ForEach(c =>
                    {
                        SugarTable table = c.GetCustomAttribute<SugarTable>();
                        if (table != null)
                        {
                            if (!tables.Exists(a => a.Name == table.TableName))
                            {
                                client.CodeFirst.InitTables(c);
                                Console.WriteLine("表: " + table.TableName + "，创建成功!");
                            }
                            else
                            {
                                Console.WriteLine("表: " + table.TableName + "，已存在!");
                            }
                        }
                    });
                }
            }
        }
        public static void InitDb ()
        {
            IConfigSection section = LocalConfig.Local.GetSection("db");
            DbModel db = section.GetValue<DbModel>("RpcService");
            if (db != null)
            {
                Console.WriteLine("RpcService，开始创建表!");
                _InitDb(db, "RpcService");
            }
            else
            {
                Console.WriteLine("RpcService，链接字符串缺失!");
            }
            db = section.GetValue<DbModel>("RpcExtendService");
            if (db != null)
            {
                Console.WriteLine("RpcExtendService，开始创建表!");
                _InitDb(db, "RpcExtendService");
            }
            else
            {
                Console.WriteLine("RpcExtendService，链接字符串缺失!");
            }
        }
    }
}
