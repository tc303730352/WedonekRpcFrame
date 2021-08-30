using System.Text;

using RpcClient.Interface;
using RpcClient.Track.Config;
using RpcClient.Track.Model;

using RpcHelper;

namespace RpcClient.Track
{
        public class ZipkinTack : ITack
        {
                private readonly ZipkinConfig _Config = null;
                private readonly IDelayDataSave<TrackBody> _TackList = null;
                private readonly string _ServerName = null;
                private static readonly HttpRequestSet _ReqestSet = new HttpRequestSet(HttpReqType.Json);
                public ZipkinTack(ZipkinConfig config, string name)
                {
                        this._Config = config;
                        this._ServerName = Tools.DropEscapeChar(name);
                        this._TackList = new DelayDataSave<TrackBody>(new SaveDelayData<TrackBody>(this._Save), 15, 10);
                }

                private void _Save(ref TrackBody[] tracks)
                {
                        byte[] datas = new ZipkinTackSerialize(this._ServerName).Serialize(tracks);
                        if (!HttpTools.SubmitByte(this._Config.PostUri, datas, _ReqestSet))
                        {
                                new WarnLog("Trace.submit.error", "链路跟踪数据提交失败!", "Trace_Zipkin")
                                {
                                        {"Uri",this._Config.PostUri },
                                        {"Trace",Encoding.UTF8.GetString(datas) }
                                }.Save();
                        }
                }

                public void AddTrace(TrackBody track)
                {
                        this._TackList.AddData(track);
                }

                public void Dispose()
                {
                        this._TackList.Dispose();
                }
        }
}
