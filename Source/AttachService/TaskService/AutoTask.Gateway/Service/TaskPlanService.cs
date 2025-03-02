using AutoTask.Gateway.Interface;
using RpcTaskModel.TaskPlan;
using RpcTaskModel.TaskPlan.Model;

namespace AutoTask.Gateway.Service
{
    internal class TaskPlanService : ITaskPlanService
    {
        public long Add (TaskPlanAdd add)
        {
            return new AddTaskPlan
            {
                TaskPlan = add
            }.Send();
        }
        public void Delete (long id)
        {
            new DeleteTaskPlan
            {
                Id = id
            }.Send();
        }
        public bool SetIsEnable (long id, bool isEnable)
        {
            return new SetTaskPlanIsEnable
            {
                Id = id,
                IsEnable = isEnable
            }.Send();
        }
        public TaskPlanDatum[] Gets (long taskId)
        {
            return new GetTaskPlans
            {
                TaskId = taskId
            }.Send();
        }
        public TaskPlanDatum Get (long id)
        {
            return new GetTaskPlan
            {
                Id = id
            }.Send();
        }
        public bool Set (long id, TaskPlanSet planSet)
        {
            return new SetTaskPlan
            {
                Id = id,
                TaskPlan = planSet
            }.Send();
        }
    }
}
