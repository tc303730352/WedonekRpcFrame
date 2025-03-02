using AutoTask.Gateway.Interface;
using AutoTask.Gateway.Model;
using RpcTaskModel.TaskItem;
using RpcTaskModel.TaskItem.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;

namespace AutoTask.Gateway.Service
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class TaskItemService : ITaskItemService
    {
        private readonly IServerTypeService _ServerType;
        private readonly IServerConfigService _Server;

        public TaskItemService (IServerTypeService serverType, IServerConfigService server)
        {
            this._ServerType = serverType;
            this._Server = server;
        }
        public TaskSelectItem[] GetSelectItems (long taskId)
        {
            return new GetTaskSelectItem
            {
                TaskId = taskId
            }.Send();
        }
        public long Add (long taskId, TaskItemSetParam datum)
        {
            return new AddTaskItem
            {
                TaskId = taskId,
                Dataum = datum
            }.Send();
        }
        public AutoTaskItem GetDetailed (long id)
        {
            TaskItemDatum item = new GetTaskItem
            {
                Id = id
            }.Send();
            AutoTaskItem obj = item.ConvertMap<TaskItemDatum, AutoTaskItem>();
            if (obj.SendType == RpcTaskModel.TaskSendType.Http)
            {
                obj.HttpParam = item.SendParam.HttpConfig;
            }
            else if (obj.SendType == RpcTaskModel.TaskSendType.指令)
            {
                obj.RemoteSet = item.SendParam.RpcConfig.RemoteSet;
                obj.RpcParam = item.SendParam.RpcConfig.ConvertMap<RpcParamConfig, TaskRpcParam>();
                if (item.SendParam.RpcConfig.ServerId != 0)
                {
                    obj.RpcParam.ToAddress = this._Server.GetName(obj.RpcParam.ServerId);
                }
                else
                {
                    string typeName = this._ServerType.GetName(obj.RpcParam.SystemType);
                    obj.RpcParam.ToAddress = string.Format("{0}({1})", typeName, obj.RpcParam.SystemType);
                }
            }
            else
            {
                obj.RemoteSet = item.SendParam.BroadcastConfig.RemoteSet;
                obj.BroadcastParam = item.SendParam.BroadcastConfig.ConvertMap<RpcBroadcastConfig, TaskRpcBroadcast>();
                if (!item.SendParam.BroadcastConfig.ServerId.IsNull())
                {
                    Dictionary<long, string> names = this._Server.GetNames(obj.BroadcastParam.ServerId);
                    if (names.Count > 0)
                    {
                        obj.BroadcastParam.ToAddress = names.Values.ToArray().Join(',');
                    }
                }
                else
                {
                    obj.BroadcastParam.ToAddress = obj.BroadcastParam.TypeVal.Join(',');
                }
            }
            return obj;
        }
        public void Delete (long id)
        {
            new DeleteTaskItem
            {
                ItemId = id
            }.Send();
        }
        public TaskItem[] Gets (long taskId)
        {
            return new GetsTaskItemByTaskId
            {
                TaskId = taskId
            }.Send();
        }
        public bool Set (long id, TaskItemSetParam datum)
        {
            return new SetTaskItem
            {
                ItemId = id,
                Datum = datum
            }.Send();
        }
        public TaskItemDatum Get (long id)
        {
            return new GetTaskItem
            {
                Id = id
            }.Send();
        }
    }
}
