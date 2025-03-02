using WeDonekRpc.Helper.Error;

namespace WeDonekRpc.HttpApiDoc.Error
{
    internal class ProErrorMsg
    {
        public ProErrorMsg()
        {

        }
        public ProErrorMsg(string errorCode)
        {
            if (LocalErrorManage.GetErrorMsg(errorCode, out ErrorMsg error))
            {
                this.ErrorId = error.ErrorId;
                this.ErrorCode = error.ErrorCode;
            }
            else
            {
                this.ErrorId = -1;
                this.ErrorCode = errorCode;
            }
        }
        public long ErrorId
        {
            get;
            set;
        }
        public string ErrorCode
        {
            get;
        }
    }
}
