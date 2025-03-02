namespace WeDonekRpc.HttpApiDoc.Model
{

    internal class ApiReturnBody
    {
        public ApiReturnType ReturnType
        {
            get;
            set;
        } = ApiReturnType.无;

        public string Show { get; set; }
        public string ClassId { get; set; }

        public ApiDataFormat[] Pros
        {
            get;
            set;
        }
    }
}
