<template>
  <div>
    <el-row style="margin-top: 20px" :gutter="24" type="flex" justify="center">
      <el-col :span="3">
        <el-card>
          <span slot="header"> 日志类型 </span>
          <el-tree
            node-key="ItemCode"
            :props="defaultProps"
            :data="logGroup"
            :default-expanded-keys="expandedKeys"
            @node-click="handleNodeClick"
          />
        </el-card>
      </el-col>
      <el-col :span="21">
        <el-card>
          <span slot="header"> 系统日志 </span>
          <el-form :inline="true" :model="queryParam">
            <el-form-item label="关键字">
              <el-input
                v-model="queryParam.QueryKey"
                placeholder="链路ID/日志标题/日志组"
              />
            </el-form-item>
            <el-form-item label="日志类型">
              <el-select v-model="queryParam.LogType" placeholder="日志类型">
                <el-option label="信息日志" value="0" />
                <el-option label="错误日志" value="1" />
              </el-select>
            </el-form-item>
            <el-form-item label="系统类别">
              <el-select v-model="queryParam.SystemType" placeholder="请选择">
                <el-option-group
                  v-for="group in groupType"
                  :key="group.TypeVal"
                  :label="group.GroupName"
                >
                  <el-option
                    v-for="item in group.ServerType"
                    :key="item.Id"
                    :label="item.SystemName"
                    :value="item.TypeVal"
                  />
                </el-option-group>
              </el-select>
            </el-form-item>
            <el-form-item label="日志等级">
              <el-select v-model="queryParam.LogGrade" placeholder="日志等级">
                <el-option
                  v-for="item in LogGrade"
                  :key="item.value"
                  :label="item.text"
                  :value="item.value"
                />
              </el-select>
            </el-form-item>
            <el-form-item label="记录时段">
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
          <el-card>
            <el-table
              :data="sysLog"
              style="width: 100%"
              @sort-change="sortChange"
            >
              <el-table-column type="index" fixed="left" :index="indexMethod" />
              <el-table-column
                prop="LogTitle"
                label="日志标题"
                min-width="150"
              />
              <el-table-column
                prop="LogShow"
                label="日志说明"
                min-width="250"
              />
              <el-table-column
                prop="SystemTypeId"
                label="系统类别"
                sortable="custom"
                width="150"
              >
                <template slot-scope="scope">
                  {{ scope.row.SystemTypeName }}
                </template>
              </el-table-column>
              <el-table-column
                sortable="custom"
                prop="ServerId"
                label="服务节点名"
                min-width="150"
              >
                <template slot-scope="scope">
                  <el-link @click="showDetailed(scope.row)">{{ scope.row.ServerName }}</el-link>
                </template>
              </el-table-column>
              <el-table-column sortable="custom" prop="LogGroup" label="日志组" min-width="150" />
              <el-table-column prop="LogType" sortable="custom" label="日志类型" width="100">
                <template slot-scope="scope">
                  <span>{{ LogType[scope.row.LogType].text }}</span>
                </template>
              </el-table-column>
              <el-table-column prop="LogGrade" sortable="custom" label="日志等级" min-width="100">
                <template slot-scope="scope">
                  <span>{{ LogGrade[scope.row.LogGrade].text }}</span>
                </template>
              </el-table-column>
              <el-table-column
                prop="ErrorCode"
                sortable="custom"
                label="错误码"
                min-width="150"
              />
              <el-table-column prop="AddTime" sortable="custom" label="添加时间" min-width="120">
                <template slot-scope="scope">
                  <span>{{
                    moment(scope.row.AddTime).format("YYYY-MM-DD HH:mm:ss")
                  }}</span>
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
                      plain
                      @click="showLog(scope.row)"
                    >详细</el-button>
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
        </el-card>
      </el-col>
    </el-row>
    <LogDetailed
      :log-id="logId"
      :visible="visible"
      @cancel="() => (visible = false)"
    />
  </div>
</template>
<script>
import moment from 'moment'
import { GetGroupAndType } from '@/api/groupType/serverGroup'
import * as LogApi from '@/api/module/sysLog'
import { GetTrees } from '@/api/dictItem'
import { LogGrade, LogType } from '@/config/publicDic'
import LogDetailed from '../../module/components/logDetailed'
export default {
  components: {
    LogDetailed
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
      LogGrade,
      LogType,
      oldRpcMerId: null,
      logGroup: [],
      groupType: [],
      logId: null,
      visible: false,
      defaultProps: {
        children: 'Children',
        label: 'ItemText'
      },
      expandedKeys: [],
      pagination: {
        size: 20,
        page: 1,
        totoal: 0,
        sort: null,
        order: null
      },
      sysLog: [],
      queryParam: {
        QueryKey: null,
        RpcMerId: null,
        SystemType: null,
        LogType: null,
        LogGrade: null,
        Time: null,
        LogGroup: null
      }
    }
  },
  watch: {
    isLoad: {
      handler(val) {
        if (val && this.rpcMerId && this.rpcMerId !== this.oldRpcMerId) {
          this.loadTree()
          this.loadGroupType()
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
    async loadGroupType() {
      if (this.groupType.length === 0) {
        this.groupType = await GetGroupAndType()
      }
    },
    showDetailed(row) {
      this.$router.push({
        name: 'serverDetailed',
        params: {
          routeTitle: row.ServerName + '-节点详情',
          Id: row.ServerId
        }
      })
    },
    showLog(row) {
      this.logId = row.Id
      this.visible = true
    },
    reset() {
      this.pagination.page = 1
      this.queryParam.RpcMerId = this.rpcMerId
      this.queryParam.SystemType = null
      this.queryParam.LogType = null
      this.queryParam.LogGrade = null
      this.queryParam.LogGroup = null
      this.queryParam.time = null
      this.queryParam.QueryKey = null
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
      const query = {
        RpcMerId: this.queryParam.RpcMerId,
        SystemType: this.queryParam.SystemType,
        LogType: this.queryParam.LogType,
        LogGrade: this.queryParam.LogGrade,
        LogGroup: this.queryParam.LogGroup,
        QueryKey: this.queryParam.QueryKey
      }
      if (this.queryParam.time && this.queryParam.time.length === 2) {
        query.Begin = this.queryParam.time[0]
        query.End = this.queryParam.time[1]
      }
      return query
    },
    async loadTable() {
      const query = this.formatQueryParam()
      const res = await LogApi.Query(query, this.pagination)
      this.sysLog = res.List
      this.pagination.total = res.Count
    },
    handleNodeClick(data) {
      this.queryParam.LogGroup = data.ItemCode
      this.loadTable()
    },
    async loadTree() {
      if (this.logGroup.length === 0) {
        const res = await GetTrees('002')
        this.format(res)
        this.logGroup = res
      }
    },
    format(rows) {
      rows.forEach((c) => {
        this.expandedKeys.push(c.ItemCode)
        c.disabled = true
        c.Children.forEach((a) => {
          a.disabled = false
        })
      })
    }
  }
}
</script>
