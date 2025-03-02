<template>
  <el-card>
    <div slot="header">
      <span>任务项列表</span>
      <el-button type="success" style="float: right;" @click="addTaskItem">添加任务项</el-button>
    </div>
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
        <template v-if="scope.row.IsEnable && scope.row.LastExecTime" slot-scope="scope">
          <el-tag v-if="scope.row.IsSuccess" type="success">成功</el-tag>
          <el-tag v-else type="danger">失败</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        prop="ErrorCode"
        label="错误码"
        min-width="100"
      />
      <el-table-column
        prop="LastExecTime"
        label="下次执行时间"
        min-width="100"
      >
        <template slot-scope="scope">
          {{ scope.row.LastExecTime ? moment(scope.row.LastExecTime).format("YYYY-MM-DD HH:mm:ss") : null }}
        </template>
      </el-table-column>
      <el-table-column
        prop="Action"
        fixed="right"
        label="操作"
        width="200"
      >
        <template slot-scope="scope">
          <el-button-group>
            <el-button
              size="small"
              @click="showItem(scope.row)"
            >详细</el-button>
            <el-button
              size="small"
              type="primary"
              @click="editItem(scope.row)"
            >编辑</el-button>
            <el-button
              size="small"
              type="danger"
              @click="deleteItem(scope.row)"
            >删除</el-button>
          </el-button-group>
        </template>
      </el-table-column>
    </el-table>
    <taskItemDetailed :id="id" :visible="visible" @cancel="close" />
    <addTaskItem :rpc-mer-id="rpcMerId" :task-id="taskId" :visible="visibleAdd" @cancel="closeAdd" />
    <editTaskItem :id="id" :rpc-mer-id="rpcMerId" :visible="visibleEdit" @cancel="closeEdit" />
  </el-card>
</template>
<script>
import moment from 'moment'
import * as taskItemApi from '@/api/task/autoTaskItem'
import { AutoTaskStep, AutoTaskLogRange } from '@/config/publicDic'
import taskItemDetailed from './taskItemDetailed'
import addTaskItem from './addTaskItem'
import editTaskItem from './editTaskItem'
export default {
  components: {
    taskItemDetailed,
    addTaskItem,
    editTaskItem
  },
  props: {
    taskId: {
      type: String,
      default: null
    },
    rpcMerId: {
      type: String,
      default: null
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
      visible: false,
      visibleAdd: false,
      visibleEdit: false
    }
  },
  watch: {
    isload: {
      handler(val) {
        if (val) {
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
    closeEdit(isRefresh) {
      this.visibleEdit = false
      if (isRefresh) {
        this.loadTable()
      }
    },
    addTaskItem() {
      this.visibleAdd = true
    },
    closeAdd(isRefresh) {
      this.visibleAdd = false
      if (isRefresh) {
        this.loadTable()
      }
    },
    showItem(row) {
      this.id = row.Id
      this.visible = true
    },
    close() {
      this.visible = false
    },
    cancelAdd(isRefresh) {
      this.visible = false
      if (isRefresh) {
        this.loadTable()
      }
    },
    deleteItem(row) {
      const that = this
      this.$confirm('确定删除?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        that.delete(row)
      }).catch(() => {
      })
    },
    editItem(row) {
      this.id = row.Id
      this.visibleEdit = true
    },
    async delete(row) {
      await taskItemApi.Delete(row.Id)
      this.$message({
        message: '删除成功！',
        type: 'success'
      })
      this.loadTable()
    },
    add() {
      this.visible = true
    },
    async loadTable() {
      this.taskItems = await taskItemApi.Gets(this.taskId)
    }
  }
}
</script>
