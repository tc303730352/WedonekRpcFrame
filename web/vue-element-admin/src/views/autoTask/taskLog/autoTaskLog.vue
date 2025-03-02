<template>
  <el-card>
    <span slot="header"> 任务日志 </span>
    <el-form :inline="true" :model="queryParam">
      <el-form-item label="任务项">
        <el-select v-model="queryParam.ItemId" clearable placeholder="任务项">
          <el-option v-for="item in taskItems" :key="item.Id" :label="item.ItemTitle" :value="item.Id" />
        </el-select>
      </el-form-item>
      <el-form-item label="执行状态">
        <el-select v-model="queryParam.IsFail" clearable placeholder="执行状态">
          <el-option label="全部" :value="null" />
          <el-option label="成功" :value="false" />
          <el-option label="失败" :value="true" />
        </el-select>
      </el-form-item>
      <el-form-item label="执行时间">
        <el-date-picker
          v-model="queryParam.Time"
          type="daterange"
          range-separator="至"
          start-placeholder="开始日期"
          end-placeholder="结束日期"
        />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="search">查询</el-button>
        <el-button type="default" @click="reset">重置</el-button>
      </el-form-item>
    </el-form>
    <el-table
      :data="logList"
      style="width: 100%"
      @sort-change="sortChange"
    >
      <el-table-column type="index" fixed="left" :index="indexMethod" />
      <el-table-column
        prop="ItemTitle"
        label="任务項"
        min-width="200"
      />
      <el-table-column
        prop="IsFail"
        label="是否失败"
        sortable="custom"
        min-width="120"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.IsFail" type="danger">失败</el-tag>
          <el-tag v-else type="success">成功</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        prop="Error"
        label="错误信息"
        min-width="200"
      />
      <el-table-column
        prop="BeginTime"
        label="执行开始时间"
        sortable="custom"
        align="center"
        min-width="150"
      >
        <template slot-scope="scope">
          <span> {{ moment(scope.row.BeginTime).format('YYYY-MM-DD HH:mm:ss') }}</span>
        </template>
      </el-table-column>
      <el-table-column
        prop="EndTime"
        label="执行结束时间"
        sortable="custom"
        align="center"
        min-width="150"
      >
        <template slot-scope="scope">
          <span> {{ moment(scope.row.EndTime).format('YYYY-MM-DD HH:mm:ss') }}</span>
        </template>
      </el-table-column>
      <el-table-column
        prop="Action"
        fixed="right"
        label="操作"
        width="100"
      >
        <template slot-scope="scope">
          <el-button
            size="small"
            @click="showLog(scope.row)"
          >详细</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-row style="text-align: right">
      <el-pagination
        :current-page="pagination.page"
        :page-size="pagination.size"
        layout="total, prev, pager, next, jumper"
        :total="pagination.total"
        @current-change="pagingChange"
      />
    </el-row>
    <TaskLogDetailed :id="logId" :visible="visible" @cancel="closeLog" />
  </el-card>
</template>
<script>
import moment from 'moment'
import { GetSelectItems } from '@/api/task/autoTaskItem'
import { Query } from '@/api/task/autoTaskLog'
import TaskLogDetailed from './autoTaskLogDetailed'
export default {
  components: {
    TaskLogDetailed
  },
  props: {
    taskId: {
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
      taskItems: [],
      logId: null,
      visible: false,
      pagination: {
        size: 20,
        page: 1,
        total: 0,
        sort: null,
        order: null
      },
      logList: [],
      queryParam: {
        TaskId: null,
        ItemId: null,
        IsFail: null,
        Time: null
      }
    }
  },
  watch: {
    isload: {
      handler(val) {
        if (val && this.taskId) {
          this.loadTaskItem()
          this.reset()
        }
      },
      immediate: true
    },
    taskId: {
      handler(val) {
        if (val && this.isload) {
          this.loadTaskItem()
          this.reset()
        }
      },
      immediate: true
    }
  },
  mounted() {
  },
  methods: {
    moment,
    async loadTaskItem() {
      this.taskItems = await GetSelectItems(this.taskId)
    },
    showLog(row) {
      this.logId = row.Id
      this.visible = true
    },
    closeLog() {
      this.visible = false
    },
    reset() {
      this.pagination.page = 1
      this.queryParam.TaskId = this.taskId
      this.queryParam.ItemId = null
      this.queryParam.IsFail = null
      this.queryParam.Time = null
      this.loadTable()
    },
    pagingChange(index) {
      this.pagination.page = index
      this.loadTable()
    },
    search() {
      this.loadTable()
    },
    sortChange(e) {
      this.pagination.order = e.order
      this.pagination.sort = e.prop
      this.loadTable()
    },
    indexMethod(index) {
      return index + 1 + (this.pagination.page - 1) * this.pagination.size
    },
    formatQueryParam() {
      for (const i in this.queryParam) {
        if (this.queryParam[i] === '') {
          this.queryParam[i] = null
        }
      }
    },
    async loadTable() {
      this.formatQueryParam()
      const query = {
        TaskId: this.taskId,
        ItemId: this.queryParam.ItemId,
        IsFail: this.queryParam.IsFail
      }
      if (this.queryParam.Time && this.queryParam.Time.length > 0) {
        query.Begin = this.queryParam.Time[0]
        query.End = this.queryParam.Time[1]
      }
      const res = await Query(query, this.pagination)
      this.logList = res.List
      this.pagination.total = res.Count
    }
  }
}
</script>
