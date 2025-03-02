<template>
  <el-card class="box-card">
    <span slot="header">{{ title }}</span>
    <el-tabs v-model="activeName" style="margin-top: 20px;" type="border-card">
      <el-tab-pane label="基础信息" name="basic">
        <autoTask :id="taskId" :isload="activeName=='basic'" />
      </el-tab-pane>
      <el-tab-pane label="任务项" name="taskItem">
        <autoTaskItem :task-id="taskId" :isload="activeName=='taskItem'" />
      </el-tab-pane>
      <el-tab-pane label="任务计划" name="taskPlan">
        <autoTaskPlan :task-id="taskId" :isload="activeName=='taskPlan'" />
      </el-tab-pane>
      <el-tab-pane label="任务日志" name="taskLog">
        <autoTaskLog :task-id="taskId" :isload="activeName=='taskLog'" />
      </el-tab-pane>
    </el-tabs>
  </el-card>
</template>
<script>
import autoTask from './components/autoTaskDatum'
import autoTaskItem from './taskItem/autoTaskItem'
import autoTaskPlan from './taskPlan/autoTaskPlan'
import autoTaskLog from './taskLog/autoTaskLog'
export default {
  components: {
    autoTask,
    autoTaskItem,
    autoTaskPlan,
    autoTaskLog
  },
  data() {
    return {
      taskId: null,
      title: '自动任务详细',
      activeName: 'basic'
    }
  },
  mounted() {
    this.taskId = parseInt(this.$route.params.Id)
  },
  methods: {
  }
}
</script>
