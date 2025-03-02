<template>
  <el-card>
    <span slot="header">任务项</span>
    <el-table
      :data="taskItems"
      style="width: 100%"
    >
      <el-table-column
        prop="ItemSort"
        label="序号"
        min-width="80"
      />
      <el-table-column
        prop="ItemTitle"
        label="任务项标题"
        min-width="200"
      />
      <el-table-column
        prop="SendType"
        label="发送类型"
        min-width="120"
      >
        <template slot-scope="scope">
          <span v-if="scope.row.SendType == 0"> 指令</span>
          <span v-else-if="scope.row.SendType == 1"> HTTP</span>
          <span v-else-if="scope.row.SendType == 2"> 广播</span>
        </template>
      </el-table-column>
      <el-table-column
        prop="TimeOut"
        label="超时时间(秒)"
        min-width="100"
      />
      <el-table-column
        prop="RetryNum"
        label="重试次数"
        min-width="100"
      />
      <el-table-column
        prop="FailStep"
        label="失败步骤"
        min-width="200"
      >
        <template slot-scope="scope">
          <span v-if="scope.row.FailStep != 2"> {{ AutoTaskStep[scope.row.FailStep].text }}</span>
          <span v-else>执行步骤: {{ scope.row.FailNextStep }}</span>
        </template>
      </el-table-column>
      <el-table-column
        prop="SuccessStep"
        label="成功步骤"
        min-width="200"
      >
        <template slot-scope="scope">
          <span v-if="scope.row.SuccessStep != 2"> {{ AutoTaskStep[scope.row.SuccessStep].text }}</span>
          <span v-else>执行步骤: {{ scope.row.NextStep }}</span>
        </template>
      </el-table-column>
      <el-table-column
        prop="LogRange"
        label="日志记录范围"
        min-width="200"
      >
        <template slot-scope="scope">
          {{ AutoTaskLogRange[scope.row.LogRange].text }}
        </template>
      </el-table-column>
      <el-table-column
        prop="IsSuccess"
        label="是否成功"
        min-width="100"
      >
        <template v-if="scope.row.LastExecTime" slot-scope="scope">
          <el-tag v-if="scope.row.IsSuccess" type="success">成功</el-tag>
          <el-tag v-else type="danger">失败</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        prop="Error"
        label="错误码"
        min-width="100"
      />
      <el-table-column
        prop="LastExecTime"
        label="下次执行时间"
        min-width="120"
      >
        <template slot-scope="scope">
          {{ scope.row.LastExecTime ? moment(scope.row.LastExecTime).format("YYYY-MM-DD HH:mm:ss") : null }}
        </template>
      </el-table-column>
      <el-table-column
        prop="Action"
        fixed="right"
        label="操作"
        width="100"
      >
        <template slot-scope="scope">
          <el-button-group>
            <el-button
              size="small"
              @click="showItem(scope.row)"
            >详细</el-button>
          </el-button-group>
        </template>
      </el-table-column>
    </el-table>
    <taskItemDetailed :id="id" :visible="visible" @cancel="close" />
  </el-card>
</template>
<script>
import moment from 'moment'
import * as taskItemApi from '@/api/task/autoTaskItem'
import { AutoTaskStep, AutoTaskLogRange } from '@/config/publicDic'
import taskItemDetailed from './taskItemDetailed'
export default {
  components: {
    taskItemDetailed
  },
  props: {
    taskId: {
      type: String,
      default: 0
    },
    isload: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      AutoTaskStep,
      AutoTaskLogRange,
      taskItems: [],
      id: null,
      visible: false
    }
  },
  watch: {
    isload: {
      handler(val) {
        if (val && this.taskId) {
          this.loadTable()
        }
      },
      immediate: true
    },
    taskId: {
      handler(val) {
        if (val && this.isload) {
          this.loadTable()
        }
      },
      immediate: true
    }
  },
  mounted() {
  },
  methods: {
    moment,
    showItem(row) {
      this.id = row.Id
      this.visible = true
    },
    close() {
      this.visible = false
    },
    async loadTable() {
      this.taskItems = await taskItemApi.Gets(this.taskId)
    }
  }
}
</script>
