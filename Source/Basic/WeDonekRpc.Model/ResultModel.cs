namespace WeDonekRpc.Model
{
    public class ResultModel
    {
        public ResultModel ()
        {

        }

        public ResultModel (string error)
        {
            this.IsError = true;
            this.Error = error;
        }
        public bool IsError
        {
            get;
            set;
        }
        public string Error
        {
            get;
            set;
        }
    }
}
