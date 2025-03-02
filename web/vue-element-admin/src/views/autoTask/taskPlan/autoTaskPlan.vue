<template>
  <el-card>
    <div slot="header">
      <span>任务计划</span>
      <el-button
        style="float: right"
        type="success"
        @click="addPlan"
      >新增计划</el-button>
    </div>
    <el-table :data="taskPlans" style="width: 100%">
      <el-table-column
        type="index"
        fixed="left"
        :index="indexMethod"
        width="80"
      />
      <el-table-column prop="PlanTitle" label="计划标题" min-width="200" />
      <el-table-column prop="PlanType" label="计划类型" min-width="120">
        <template slot-scope="scope">
          <span v-if="scope.row.PlanType == 0">循环任务</span>
          <span v-else-if="scope.row.PlanType == 1">只执行一次</span>
        </template>
      </el-table-column>
      <el-table-column prop="PlanShow" label="计划说明" min-width="300" />
      <el-table-column prop="ExecRate" label="执行周期" min-width="80">
        <template slot-scope="scope">
          <span v-if="scope.row.ExecRate == 0">每天</span>
          <span v-else-if="scope.row.ExecRate == 1">每周</span>
          <span v-else-if="scope.row.ExecRate == 2">每月</span>
        </template>
      </el-table-column>
      <el-table-column prop="IsEnable" label="是否启用" min-width="80">
        <template slot-scope="scope">
          <el-switch
            v-model="scope.row.IsEnable"
            @change="setIsEnable(scope.row)"
          />
        </template>
      </el-table-column>
      <el-table-column prop="Action" fixed="right" label="操作" width="200">
        <template slot-scope="scope">
          <el-button-group v-if="!scope.row.IsEnable">
            <el-button
              size="small"
              type="primary"
              @click="editPlan(scope.row)"
            >编辑</el-button>
            <el-button
              size="small"
              type="danger"
              @click="deletePlan(scope.row)"
            >删除</el-button>
          </el-button-group>
        </template>
      </el-table-column>
    </el-table>
    <taskPlanAdd :task-id="taskId" :visible="visible" @cancel="closeAdd" />
    <taskPlanEdit :id="planId" :visible="visibleEdit" @cancel="closeEdit" />
  </el-card>
</template>
<script>
import moment from 'moment'
import * as taskPlanApi from '@/api/task/autoTaskPlan'
import { AutoTaskSpaceWeek, AutoTaskDayRate } from '@/config/publicDic'
import taskPlanAdd from './taskPlanAdd'
import taskPlanEdit from './taskPlanEdit'
export default {
  components: {
    taskPlanAdd,
    taskPlanEdit
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
      planId: null,
      AutoTaskSpaceWeek,
      AutoTaskDayRate,
      taskPlans: [],
      visible: false,
      visibleEdit: false
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
  mounted() {},
  methods: {
    moment,
    addPlan() {
      this.visible = true
    },
    editPlan(row) {
      this.planId = row.Id
      this.visibleEdit = true
    },
    indexMethod(index) {
      return index + 1
    },
    closeEdit(isRefresh) {
      this.visibleEdit = false
      if (isRefresh) {
        this.loadTable()
      }
    },
    closeAdd(isRefresh) {
      this.visible = false
      if (isRefresh) {
        this.loadTable()
      }
    },
    deletePlan(row) {
      const that = this
      this.$confirm('确定删除?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      })
        .then(() => {
          that.delete(row)
        })
        .catch(() => {})
    },
    async delete(row) {
      await taskPlanApi.Delete(row.Id)
      this.$message({
        message: '删除成功！',
        type: 'success'
      })
      this.loadTable()
    },
    async setIsEnable(row) {
      await taskPlanApi.SetIsEnable(row.Id, row.IsEnable)
      this.$message({
        message: '设置成功!',
        type: 'success'
      })
    },
    async loadTable() {
      this.taskPlans = await taskPlanApi.Gets(this.taskId)
    }
  }
}
</script>
