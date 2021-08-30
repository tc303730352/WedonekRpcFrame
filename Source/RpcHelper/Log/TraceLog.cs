using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpcHelper.Log
{
        public class TraceLog : LogInfo
        {
                private const string _DefGroup = "Def";
                public TraceLog(string title, string content) : base(title, content, _DefGroup, LogGrade.Trace)
                {
                }
                public TraceLog(string title) : base(title, _DefGroup, LogGrade.Trace)
                {
                }
                public TraceLog(string title, string content, string group) : base(title, content, group, LogGrade.Trace)
                {
                }
        }
}
