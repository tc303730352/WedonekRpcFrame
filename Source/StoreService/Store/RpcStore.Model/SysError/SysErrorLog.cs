using WeDonekRpc.Helper;

namespace RpcStore.Model.SysError
{
    public class SysErrorLog
    {
        public long Id
        {
            get;
            set;
        }
        public long RpcMerId
        {
            get;
            set;
        }
        public string TraceId
        {
            get;
            set;
        }


        public string LogTitle
        {
            get;
            set;
        }

        public string LogShow
        {
            get;
            set;
        }

        public string SystemType
        {
            get;
            set;
        }

        public long ServerId
        {
            get;
            set;
        }

        public string LogGroup
        {
            get;
            set;
        }

        public LogType LogType
        {
            get;
            set;
        }

        public LogGrade LogGrade
        {
            get;
            set;
        }

        public string ErrorCode
        {
            get;
            set;
        }

        public DateTime AddTime
        {
            get;
            set;
        }
    }
}
