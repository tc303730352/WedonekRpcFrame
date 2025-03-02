namespace WeDonekRpc.Helper
{
    public enum PhoneOperator
    {
        未知 = -1,
        移动 = 0,
        电信 = 1,
        联通 = 2,
        虚拟运营商 = 3
    }
    public enum AreaType
    {
        未知 = 0,
        国家 = 1,
        省份 = 2,
        城市 = 3,
        区县 = 4
    }
    public enum GpsType : int
    {
        高德谷歌GCJ02 = 0,
        百度坐标系BD09 = 1,
        GPS坐标 = 2
    }

    public enum TimeIntervalType
    {
        早晨 = 2,
        上午 = 4,
        中午 = 8,
        下午 = 16,
        傍晚 = 32,
        晚上 = 64,
        深夜 = 128,
        凌晨 = 256
    }
    public enum FilterHtmlType
    {
        无 = -1,
        除基础HTML外全部 = 1,
        HTML = 0,
        Script = 2,
        Style = 4,
        Link = 8,
        If = 16,
        注释 = 32,
        A标签 = 64,
        Event = 128
    }
    public enum LogType
    {
        信息日志 = 0,
        错误日志 = 1,
    }
    public enum LogGrade
    {
        Trace = 0,
        Information = 2,
        DEBUG = 1,
        WARN = 3,
        ERROR = 4,
        Critical = 5
    }

    public enum MoneyUnit
    {
        元 = 0,
        角 = 1,
        分 = 2,
        里 = 3,
        毫 = 4
    }
    public enum DevType
    {
        无 = 0,
        IPad = 2,
        苹果 = 4,
        PC = 8,
        安卓 = 16,
        WindowsPhone = 32,
        未知 = 1
    }
    public enum RequestType
    {
        未知 = 0,
        移动设备 = 1,
        PC = 2,
        微信 = 3
    }
    public enum SequentialGuidType
    {
        /// <summary>
        /// 适用MySql PostgreSQL 
        /// </summary>
        SequentialAsString,
        /// <summary>
        /// 适用Oracle 
        /// </summary>
        SequentialAsBinary,
        /// <summary>
        /// 适用sqlServer
        /// </summary>
        SequentialAtEnd
    }
}
