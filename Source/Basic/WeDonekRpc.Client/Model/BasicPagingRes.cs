using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Model
{
        internal class BasicPagingRes : BasicRes
        {
                public BasicPagingRes()
                {

                }
                public BasicPagingRes(object res, object count)
                {
                        this.DataList = res;
                        this.Count = count;
                }
                public object DataList
                {
                        get;
                        set;
                }
                public object Count
                {
                        get;
                        set;
                }
        }
}
