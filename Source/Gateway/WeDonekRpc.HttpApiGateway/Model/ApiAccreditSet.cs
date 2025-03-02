namespace WeDonekRpc.HttpApiGateway.Model
{
    public class ApiAccreditSet
    {
        public ApiAccreditSet ( bool isAccredit, string[] prower )
        {
            this.IsAccredit = isAccredit;
            this.Prower = prower;
        }

        public bool IsAccredit
        {
            get;
        }
        public string[] Prower
        {
            get;
        }
    }
}
