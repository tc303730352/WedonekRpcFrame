<template>
  <div>
    <el-row style="margin-top: 20px" :gutter="24" type="flex" justify="center">
      <el-col :span="3">
        <el-card>
          <span slot="header"> 系统类别 </span>
          <el-tree
            node-key="key"
            :data="groupType"
            :default-expanded-keys="expandedKeys"
            @node-click="handleNodeClick"
          />
        </el-card>
      </el-col>
      <el-col :span="21">
        <el-card>
          <div slot="header" class="clearfix">
            <span style="line-height: 40px">重试任务</span>
          </div>
          <el-form :inline="true" :model="queryParam">
            <el-form-item label="任务标识">
              <el-input v-model="queryParam.QueryKey" placeholder="任务标识" />
            </el-form-item>
            <el-form-item label="应用的机房">
              <el-select
                v-model="queryParam.RegionId"
                clearable
                placeholder="应用的机房"
              >
                <el-option
                  v-for="item in region"
                  :key="item.Id"
                  :label="item.RegionName"
                  :value="item.Id"
                />
              </el-select>
            </el-form-item>
            <el-form-item label="执行时段">
              <el-date-picker
                v-model="queryParam.RunDate"
                type="date"
                placeholder="执行日期"
              />
              <el-time-picker
                v-model="queryParam.Time"
                is-range
                range-separator="至"
                :clearable="true"
                :picker-options="{
                  format: 'HH:mm',
                }"
                start-placeholder="开始时间"
                end-placeholder="结束时间"
                placeholder="选择时间范围"
              />
            </el-form-item>
            <el-form-item label="重试状态">
              <el-select
                v-model="queryParam.Status"
                multiple
                clearable
                placeholder="重试状态"
              >
                <el-option
                  v-for="item in taskStatus"
                  :key="item.value"
                  :label="item.text"
                  :value="item.value"
                />
              </el-select>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="loadTable">查询</el-button>
              <el-button type="default" @click="reset">重置</el-button>
            </el-form-item>
          </el-form>
          <el-table :data="tasks" style="width: 100%" @sort-change="sortChange">
            <el-table-column type="index" fixed="left" :index="indexMethod" />
            <el-table-column
              prop="IdentityId"
              label="任务标识"
              min-width="200"
            />
            <el-table-column prop="Show" label="说明" min-width="150" />
            <el-table-column prop="RegionName" label="机房名" min-width="120" />
            <el-table-column
              prop="ServerName"
              label="发起节点"
              min-width="200"
            />
            <el-table-column
              prop="SystemTypeName"
              label="发起节点类型"
              min-width="120"
            />
            <el-table-column prop="Status" label="状态" min-width="100">
              <template slot-scope="scope">
                <el-tag :type="taskStatus[scope.row.Status].type">{{
                  taskStatus[scope.row.Status].text
                }}</el-tag>
              </template>
            </el-table-column>
            <el-table-column
              prop="RegService"
              label="负责重试的节点"
              min-width="150"
            />
            <el-table-column
              prop="NextRetryTime"
              label="下次重试时间"
              min-width="180"
            >
              <template slot-scope="scope">
                {{
                  moment(scope.row.NextRetryTime).format("YYYY-MM-DD HH:mm:ss")
                }}
              </template>
            </el-table-column>
            <el-table-column
              prop="RetryNum"
              label="已重试次数"
              min-width="120"
            />
            <el-table-column
              prop="ErrorCode"
              label="最后次错误码"
              min-width="120"
            />
            <el-table-column
              prop="ComplateTime"
              label="完成/失败时间"
              min-width="150"
            >
              <template slot-scope="scope">
                {{
                  scope.row.ComplateTime != null
                    ? moment(scope.row.ComplateTime).format(
                      "YYYY-MM-DD HH:mm:ss"
                    )
                    : null
                }}
              </template>
            </el-table-column>
            <el-table-column prop="AddTime" label="添加时间" min-width="150">
              <template slot-scope="scope">
                {{ moment(scope.row.AddTime).format("YYYY-MM-DD HH:mm:ss") }}
              </template>
            </el-table-column>
            <el-table-column
              prop="Action"
              fixed="right"
              label="操作"
              width="150"
            >
              <template slot-scope="scope">
                <el-button-group>
                  <el-button
                    size="small"
                    type="primary"
                    @click="show(scope.row)"
                  >详细</el-button>
                  <el-button
                    v-if="scope.row.Status == taskStatus[0].value"
                    size="small"
                    type="danger"
                    @click="cancel(scope.row)"
                  >取消</el-button>
                  <el-button
                    v-if="
                      scope.row.Status == taskStatus[0].value ||
                        scope.row.Status == taskStatus[2].value
                    "
                    size="small"
                    type="danger"
                    @click="resetTask(scope.row)"
                  >重试</el-button>
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
      </el-col>
    </el-row>
    <retryTaskDetailed :id="id" :visible="visible" @cancel="closeDetailed" />
  </div>
</template>
<script>
import moment from 'moment'
import * as autoRetryApi from '@/api/autoRetry/autoRetryTask'
import * as serverBindApi from '@/api/rpcMer/serverBind'
import retryTaskDetailed from './components/retryTaskDetailed'
import { GetList } from '@/api/basic/region'
export default {
  components: {
    retryTaskDetailed
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
      tasks: [],
      groupType: [],
      region: [],
      oldRpcMerId: null,
      expandedKeys: [],
      id: null,
      visible: false,
      taskStatus: {
        0: {
          value: 0,
          text: '待重试',
          type: 'info'
        },
        1: {
          value: 1,
          text: '已重试成功',
          type: 'success'
        },
        2: {
          value: 2,
          text: '已重试失败',
          type: 'danger'
        },
        3: {
          value: 3,
          text: '已取消',
          color: 'warning'
        }
      },
      queryParam: {
        Status: [],
        RegionId: null,
        QueryKey: null,
        RunDate: null,
        Time: []
      },
      pagination: {
        size: 20,
        page: 1,
        total: 0,
        sort: null,
        order: null
      }
    }
  },
  watch: {
    rpcMerId: {
      handler(val) {
        if (val) {
          this.queryParam.RpcMerId = val
          if (this.isLoad) {
            this.reset()
          }
        }
      },
      immediate: true
    },
    isLoad: {
      handler(val) {
        if (val && this.rpcMerId != null) {
          this.loadTree()
          this.loadRegion()
          this.reset()
        }
      },
      immediate: true
    }
  },
  mounted() {},
  methods: {
    moment,
    closeDetailed() {
      this.visible = false
    },
    show(row) {
      this.id = row.Id
      this.visible = true
    },
    async loadRegion() {
      if (this.region.length === 0) {
        this.region = await GetList()
      }
    },
    handleNodeClick(data) {
      this.queryParam.TypeVal = data.TypeVal
      this.loadTable()
    },
    cancel(row) {
      const that = this
      this.$confirm('确认取消该任务?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        that.configmCancel(row)
      })
    },
    async loadTree() {
      if (this.groupType.length === 0 || this.oldRpcMerId !== this.rpcMerId) {
        this.oldRpcMerId = this.rpcMerId
        const res = await serverBindApi.GetGroupAndType(
          this.rpcMerId,
          null,
          this.serviceType,
          this.isHold
        )
        this.groupType = this.format(res)
      }
    },
    reset() {
      this.pagination.page = 1
      this.queryParam.QueryKey = null
      this.queryParam.Status = null
      this.queryParam.RegionId = null
      this.queryParam.Time = null
      this.queryParam.RunDate = null
      this.loadTable()
    },
    resetTask(row) {
      const that = this
      this.$confirm('确认重试该任务?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        that.confirmCancel(row)
      })
    },
    async confirmReset(row) {
      await autoRetryApi.Reset(row.RegServiceId, row.IdentityId)
      this.$message({
        message: '重试成功，结果请查看详细！',
        type: 'success'
      })
    },
    async confirmCancel(row) {
      await autoRetryApi.Cancel(row.Id)
      this.$message({
        message: '取消成功！',
        type: 'success'
      })
      this.loadTable()
    },
    pagingChange(index) {
      this.pagination.page = index
      this.loadTable()
    },
    sortChange(e) {
      this.pagination.order = e.order
      this.pagination.sort = e.prop
      this.loadTable()
    },
    format(rows) {
      return rows.map((c) => {
        this.expandedKeys.push('group_' + c.Id)
        return {
          type: 'group',
          key: 'group_' + c.Id,
          Id: c.Id,
          label: c.GroupName,
          disabled: true,
          children: c.ServerType.map((a) => {
            return {
              type: 'serverType',
              key: 'type_' + a.Id,
              Id: a.Id,
              TypeVal: a.TypeVal,
              label: a.SystemName + '(' + a.ServerNum + ')',
              fullName: c.GroupName + '-' + a.SystemName,
              ServerNum: a.ServerNum
            }
          })
        }
      })
    },
    indexMethod(index) {
      return index + 1 + (this.pagination.page - 1) * this.pagination.size
    },
    async loadTable() {
      const query = {
        RpcMerId: this.queryParam.RpcMerId,
        Status: this.queryParam.Status,
        SystemType: this.queryParam.TypeVal,
        QueryKey: this.queryParam.QueryKey,
        RegionId: this.queryParam.RegionId
      }
      if (
        this.queryParam.RunDate != null &&
        this.queryParam.Time != null &&
        this.queryParam.Time.length !== 0
      ) {
        query.Begin =
          moment(this.queryParam.RunDate).format('YYYY-MM-DD') +
          ' ' +
          this.queryParam.Time[0]
        query.End =
          moment(this.queryParam.RunDate).format('YYYY-MM-DD') +
          ' ' +
          this.queryParam.Time[1]
      }
      const res = await autoRetryApi.Query(query, this.pagination)
      this.tasks = res.List
      this.pagination.total = res.Count
    }
  }
}
</script>
