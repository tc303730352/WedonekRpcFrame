namespace RpcModel.Model
{
        [System.Serializable]
        public class RpcControlServer
        {
                public int Id { get; set; }
                public string ServerIp
                {
                        get;
                        set;
                }
                public short Port
                {
                        get;
                        set;
                }

        }
}
