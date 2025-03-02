namespace WeDonekRpc.Model.Server
{
    public struct AccessTokenRes
    {
        public AccessTokenRes ( string accessToken, long merId, int exin = 7200 )
        {
            this.RpcMerId = merId;
            this.access_token = accessToken;
            this.expires_in = exin;
        }
        public long RpcMerId
        {
            get;
            set;
        }
        public string access_token
        {
            get;
            set;
        }

        public int expires_in
        {
            get;
            set;
        }
    }
}
