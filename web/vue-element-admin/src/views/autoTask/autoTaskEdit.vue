<template>
  <div>
    <el-card class="box-card">
      <span slot="header">{{ title }}</span>
    </el-card>
    <el-tabs v-model="activeName" style="margin-top: 20px;" type="border-card">
      <el-tab-pane label="基础信息" name="basic">
        <edit-auto-task :id="taskId" :isload="activeName=='basic'" />
      </el-tab-pane>
      <el-tab-pane label="任务项" name="taskItem">
        <autoTaskItemEdit :task-id="taskId" :rpc-mer-id="rpcMerId" :isload="activeName=='taskItem'" />
      </el-tab-pane>
      <el-tab-pane label="任务计划" name="taskPlan">
        <autoTaskPlan :task-id="taskId" :isload="activeName=='taskPlan'" />
      </el-tab-pane>
    </el-tabs>
  </div>
</template>
<script>
import editAutoTask from './components/editAutoTask'
import autoTaskItemEdit from './taskItem/autoTaskItemEdit'
import autoTaskPlan from './taskPlan/autoTaskPlan'
export default {
  components: {
    editAutoTask,
    autoTaskItemEdit,
    autoTaskPlan
  },
  data() {
    return {
      taskId: null,
      rpcMerId: null,
      title: '编辑自动任务',
      activeName: 'basic'
    }
  },
  mounted() {
    this.taskId = this.$route.params.Id
    this.rpcMerId = this.$route.params.rpcMerId
  },
  methods: {
  }
}
</script>
