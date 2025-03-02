using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Track.Config;
using WeDonekRpc.Client.Track.Model;

using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Http;
using WeDonekRpc.Helper.Interface;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.Client.Track
{
    /// <summary>
    /// Zipkin链路记录器
    /// </summary>
    public class ZipkinTack : ITack
    {
        private readonly ZipkinConfig _Config = null;
        private readonly IDelayDataSave<TrackBody> _TackList = null;
        public ZipkinTack ( ZipkinConfig config )
        {
            this._Config = config;
            this._TackList = new DelayDataSave<TrackBody>(new SaveDelayData<TrackBody>(this._Save), 15, 10);
        }

        private void _Save ( ref TrackBody[] tracks )
        {
            string json = new ZipkinTackSerialize().Serialize(tracks);
            using ( HttpClient client = HttpClientFactory.Create() )
            {
                try
                {
                    client.SendJson(_Config.PostUri, json, HttpCompletionOption.ResponseHeadersRead);
                }
                catch(Exception ex )
                {
                    ErrorException error = ErrorException.FormatError(ex);
                    error.AppendArg(new Dictionary<string, string>{ 
                                        {"Uri",this._Config.PostUri.AbsoluteUri },
                                        {"Trace",json }
                                });
                    error.SaveLog("Trace_Zipkin");
                }
            }
        }

        public void AddTrace ( TrackBody track )
        {
            this._TackList.AddData(track);
        }

        public void Dispose ()
        {
            this._TackList.Dispose();
        }
    }
}
