<template>
  <el-card>
    <span slot="header"> 自动任务管理 </span>
    <el-form :inline="true" :model="queryParam">
      <el-form-item label="任务名">
        <el-input
          v-model="queryParam.QueryKey"
          placeholder="任务名"
        />
      </el-form-item>
      <el-form-item label="应用的机房">
        <el-select v-model="queryParam.RegionId" clearable placeholder="应用的机房">
          <el-option v-for="item in region" :key="item.Id" :label="item.RegionName" :value="item.Id" />
        </el-select>
      </el-form-item>
      <el-form-item label="任务状态">
        <el-select v-model="queryParam.TaskStatus" multiple clearable placeholder="任务状态">
          <el-option label="起草" :value="0" />
          <el-option label="启用" :value="1" />
          <el-option label="停用" :value="2" />
          <el-option label="错误" :value="3" />
        </el-select>
      </el-form-item>
      <el-form-item label="最后执行时间">
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
        <el-button type="success" @click="add">新增</el-button>
      </el-form-item>
    </el-form>
    <el-card>
      <el-table
        :data="taskList"
        style="width: 100%"
        @sort-change="sortChange"
      >
        <el-table-column type="index" fixed="left" :index="indexMethod" />
        <el-table-column
          prop="TaskName"
          label="任务名"
          min-width="200"
        />
        <el-table-column
          prop="RegionId"
          label="应用机房"
          sortable="custom"
          min-width="120"
        >
          <template slot-scope="scope">
            {{ scope.row.RegionName }}
          </template>
        </el-table-column>
        <el-table-column
          prop="TaskStatus"
          label="任务状态"
          sortable="custom"
          min-width="140"
        >
          <template slot-scope="scope">
            <template v-if="scope.row.TaskStatus == 3">
              <el-tag type="danger">错误</el-tag>
            </template>
            <el-switch
              v-else
              v-model="scope.row.IsEnable"
              active-text="启用"
              :inactive-text="scope.row.TaskStatus == 0 ? '起草': '停用'"
              @change="taskChange(scope.row)"
            />
          </template>
        </el-table-column>
        <el-table-column
          prop="TaskShow"
          label="任务说明"
          min-width="200"
        />
        <el-table-column
          prop="TaskPriority"
          label="任务优先级"
          sortable="custom"
          align="center"
          min-width="120"
        />
        <el-table-column
          prop="IsExec"
          label="执行状态"
          sortable="custom"
          min-width="100"
        >
          <template slot-scope="scope">
            <el-tag v-if="scope.row.IsExec" type="success">执行中</el-tag>
            <el-tag v-else>待执行</el-tag>
          </template>
        </el-table-column>
        <el-table-column
          prop="LastExecTime"
          label="最后执行时间"
          sortable="custom"
          min-width="150"
        >
          <template slot-scope="scope">
            {{ scope.row.LastExecTime ? moment(scope.row.LastExecTime).format('YYYY-MM-DD HH:mm:ss') : null }}
          </template>
        </el-table-column>
        <el-table-column
          prop="NextExecTime"
          label="下次执行时间"
          sortable="custom"
          min-width="150"
        >
          <template slot-scope="scope">
            {{ scope.row.NextExecTime ? moment(scope.row.NextExecTime).format('YYYY-MM-DD HH:mm:ss') : null }}
          </template>
        </el-table-column>
        <el-table-column
          prop="LastExecEndTime"
          label="最后操作时间"
          sortable="custom"
          min-width="150"
        >
          <template slot-scope="scope">
            <span v-if="scope.row.TaskStatus==2"> {{ scope.row.StopTime ? moment(scope.row.StopTime).format('YYYY-MM-DD HH:mm:ss') : null }}</span>
            <span v-else> {{ scope.row.LastExecEndTime ? moment(scope.row.LastExecEndTime).format('YYYY-MM-DD HH:mm:ss') : null }}</span>
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
                v-if="scope.row.TaskStatus != 0"
                size="small"
                @click="showTask(scope.row)"
              >任务详细</el-button>
              <el-button
                v-if="scope.row.TaskStatus == 0 || scope.row.TaskStatus == 2"
                size="small"
                type="primary"
                @click="editTask(scope.row)"
              >编辑</el-button>
              <el-button
                v-if="scope.row.TaskStatus == 0"
                size="small"
                type="danger"
                @click="deleteTask(scope.row)"
              >删除</el-button>
            </el-button-group>
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
    </el-card>
    <addAutoTask :visible="visible" :rpc-mer-id="rpcMerId" @cancel="cancelAdd" />
  </el-card>
</template>
<script>
import moment from 'moment'
import { GetList } from '@/api/basic/region'
import * as autoTaskApi from '@/api/task/autoTask'
import addAutoTask from '@/views/autoTask/components/addAutoTask'
export default {
  components: {
    addAutoTask
  },
  props: {
    rpcMerId: {
      type: String,
      default: null
    },
    isLoad: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      region: [],
      visible: false,
      pagination: {
        size: 20,
        page: 1,
        total: 0,
        sort: null,
        order: null
      },
      taskList: [],
      queryParam: {
        QueryKey: null,
        RpcMerId: null,
        RegionId: null
      }
    }
  },
  watch: {
    isLoad: {
      handler(val) {
        if (val) {
          this.loadRegion()
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
    cancelAdd(isRefresh) {
      this.visible = false
      if (isRefresh) {
        this.loadTable()
      }
    },
    showTask(row) {
      this.$router.push({
        name: 'taskDetailed',
        params: {
          routeTitle: row.TaskName + '-详细',
          Id: row.Id
        }
      })
    },
    deleteTask(row) {
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
    editTask(row) {
      this.$router.push({
        name: 'editTask',
        params: {
          routeTitle: row.TaskName + '-编辑',
          Id: row.Id,
          rpcMerId: this.rpcMerId
        }
      })
    },
    async delete(row) {
      await autoTaskApi.Delete(row.Id)
      this.$message({
        message: '删除成功！',
        type: 'success'
      })
      this.loadTable()
    },
    taskChange(row) {
      if (row.IsEnable) {
        autoTaskApi.Enable(row.Id).then(res => {
          row.IsEnable = res
          row.TaskStatus = 1
        }).catch(() => {
          row.IsEnable = false
        })
      } else {
        const that = this
        this.$confirm('是否强制停止任务？', '确认信息', {
          distinguishCancelAndClose: true,
          confirmButtonText: '是',
          cancelButtonText: '否'
        }).then(() => {
          that.disable(row, true)
        }).catch(() => {
          that.disable(row, false)
        })
      }
    },
    async disable(row, isEnd) {
      autoTaskApi.Disable(row.Id, isEnd).then(res => {
        row.IsEnable = !res
        row.TaskStatus = 2
        this.$message({
          message: '停用成功！',
          type: 'success'
        })
      }).catch(() => {
        row.IsEnable = true
      })
    },
    add() {
      this.visible = true
    },
    async loadRegion() {
      if (this.region.length === 0) {
        this.region = await GetList()
      }
    },
    reset() {
      this.pagination.page = 1
      this.queryParam.RpcMerId = this.rpcMerId
      this.queryParam.QueryKey = null
      this.queryParam.RegionId = null
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
      const res = await autoTaskApi.Query(this.queryParam, this.pagination)
      res.List.forEach(c => {
        c.IsEnable = c.TaskStatus === 1
      })
      this.taskList = res.List
      this.pagination.total = res.Count
    }
  }
}
</script>
