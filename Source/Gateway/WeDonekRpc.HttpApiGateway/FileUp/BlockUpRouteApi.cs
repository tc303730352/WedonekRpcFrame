using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway.Collect;
using WeDonekRpc.HttpApiGateway.FileUp.Interface;
using WeDonekRpc.HttpApiGateway.FileUp.Model;
using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.FileUp
{
    internal class BlockUpRouteApi: IApiGateway
    {

        [ApiGateway.Attr.ApiStop]
        public BlockUpSate GetState ( [NullValidate("gateway.http.up.taskId.error")] string taskId )
        {
            if ( BlockUpCollect.GetTask(taskId, out IBlockUpTask task) )
            {
                return task.GetUpState();
            }
            throw new ErrorException("gateway.http.up.task.not.find");
        }
    }
}
