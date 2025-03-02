<template>
  <div>
    <el-row style="margin-top: 20px" :gutter="24" type="flex" justify="center">
      <el-col :span="3">
        <el-card>
          <span slot="header"> 系统类别 </span>
          <el-tree node-key="TypeVal" :data="groupType" :default-expanded-keys="expandedKeys" @node-click="handleNodeClick" />
        </el-card>
      </el-col>
      <el-col :span="21">
        <el-card>
          <span slot="header"> 链路日志 </span>
          <el-form :inline="true" :model="queryParam">
            <el-form-item label="关键字">
              <el-input
                v-model="queryParam.QueryKey"
                placeholder="链路ID/服务名"
              />
            </el-form-item>
            <el-form-item label="来源机房">
              <el-select v-model="queryParam.RegionId" clearable placeholder="来源机房">
                <el-option v-for="item in region" :key="item.Id" :label="item.RegionName" :value="item.Id" />
              </el-select>
            </el-form-item>
            <el-form-item label="来源集群">
              <el-select
                v-model="queryParam.RpcMerId"
                clearable
                placeholder="来源集群"
              >
                <el-option
                  v-for="item in rpcMer"
                  :key="item.Id"
                  :label="item.SystemName"
                  :value="item.Id"
                />
              </el-select>
            </el-form-item>
            <el-form-item label="记录时段">
              <el-date-picker
                v-model="queryParam.Time"
                width="180px"
                type="datetimerange"
                range-separator="至"
                start-placeholder="开始时间"
                end-placeholder="结束时间"
              />
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="search">查询</el-button>
              <el-button type="default" @click="reset">重置</el-button>
            </el-form-item>
          </el-form>
          <el-card>
            <el-table
              :data="traceLog"
              style="width: 100%"
              @expand-change="expendRow"
              @sort-change="sortChange"
            >
              <el-table-column type="index" fixed="left" :index="indexMethod" />
              <el-table-column type="expand">
                <template slot-scope="scope">
                  <el-table
                    v-loading="scope.row.IsLoad"
                    :data="scope.row.TraceLog"
                    row-key="Id"
                    style="width: 100%"
                    default-expand-all
                    :tree-props="{children: 'Children'}"
                  >
                    <el-table-column type="index" :index="traceIndexMethod" />
                    <el-table-column
                      prop="Dictate"
                      label="事件指令"
                      min-width="150"
                    />
                    <el-table-column
                      prop="Show"
                      label="说明"
                      min-width="120"
                    />
                    <el-table-column
                      prop="RemoteServerName"
                      label="目的地"
                      min-width="120"
                    >
                      <template slot-scope="suScope">
                        <el-link @click="showRemote(suScope.row)">{{ suScope.row.RemoteServerName }}</el-link>
                      </template>
                    </el-table-column>
                    <el-table-column
                      prop="SystemTypeName"
                      label="发起节点类型"
                      min-width="120"
                    />
                    <el-table-column
                      prop="Region"
                      label="机房"
                      min-width="80"
                    />
                    <el-table-column
                      prop="MsgType"
                      label="消息类型"
                      min-width="80"
                    />
                    <el-table-column
                      prop="StageType"
                      label="发送方向"
                      min-width="100"
                    >
                      <template slot-scope="suScope">
                        <el-tag v-if="suScope.row.StageType ==0">发送</el-tag>
                        <el-tag v-if="suScope.row.StageType ==1" type="success">回复</el-tag>
                      </template>
                    </el-table-column>
                    <el-table-column
                      prop="Duration"
                      label="耗时"
                      min-width="100"
                    >
                      <template slot-scope="suScope">
                        <span>{{
                          suScope.row.Duration / 1000
                        }}</span>
                      </template>
                    </el-table-column>
                    <el-table-column
                      prop="BeginTime"
                      label="发生时间"
                      min-width="170"
                    >
                      <template slot-scope="suScope">
                        {{ moment(suScope.row.BeginTime).format("YYYY-MM-DD HH:mm:ss.ms") }}
                      </template>
                    </el-table-column>
                    <el-table-column
                      prop="Action"
                      fixed="right"
                      label="操作"
                      width="100"
                    >
                      <template slot-scope="suScope">
                        <el-button-group>
                          <el-button
                            size="small"
                            plain
                            @click="showLog(suScope.row)"
                          >详细</el-button>
                        </el-button-group>
                      </template>
                    </el-table-column>
                  </el-table>
                </template>
              </el-table-column>
              <el-table-column
                prop="TraceId"
                label="链路ID"
                min-width="190"
              />
              <el-table-column
                prop="ServerName"
                label="来源节点"
                min-width="120"
              >
                <template slot-scope="scope">
                  <el-link @click="showDetailed(scope.row)">{{ scope.row.ServerName }}</el-link>
                </template>
              </el-table-column>
              <el-table-column
                prop="Dictate"
                label="发起事件"
                min-width="140"
              />
              <el-table-column
                prop="Show"
                label="说明"
                min-width="150"
              />
              <el-table-column
                prop="SystemTypeName"
                label="节点类别"
                min-width="120"
              />
              <el-table-column
                prop="RpcMer"
                label="所属集群"
                min-width="120"
              />
              <el-table-column
                prop="Region"
                label="所属机房"
                min-width="80"
              />
              <el-table-column prop="Duration" label="耗时（毫秒）" sortable="custom" width="150">
                <template slot-scope="scope">
                  <span>{{
                    scope.row.Duration / 1000
                  }}</span>
                </template>
              </el-table-column>
              <el-table-column prop="BeginTime" label="请求时间" sortable="custom" min-width="150">
                <template slot-scope="scope">
                  <span>{{
                    moment(scope.row.BeginTime).format("YYYY-MM-DD HH:mm:ss.ms")
                  }}</span>
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
    <traceDetailed :id="logId" :visible="visible" @cancel="()=>visible=false" />
  </div>
</template>
<script>
import moment from 'moment'
import { GetGroupAndType } from '@/api/groupType/serverGroup'
import { GetItems } from '@/api/rpcMer/rpcMer'
import { GetList } from '@/api/basic/region'
import * as TraceApi from '@/api/module/trace'
import traceDetailed from './components/traceDetailed'
export default {
  components: {
    traceDetailed
  },
  data() {
    return {
      region: [],
      groupType: [],
      visible: false,
      logId: null,
      defaultProps: {
        children: 'children',
        label: 'label'
      },
      expandedKeys: [],
      pagination: {
        size: 20,
        page: 1,
        total: 0,
        sort: null,
        order: null
      },
      traceLog: [],
      traceList: [],
      rpcMer: [],
      queryParam: {
        QueryKey: null,
        RpcMerId: null,
        SystemType: null,
        ServerId: null,
        RegionId: null,
        Time: null
      }
    }
  },
  mounted() {
    this.loadRpcMer()
    this.loadTree()
    this.loadRegion()
    this.reset()
  },
  methods: {
    moment,
    showDetailed(row) {
      this.$router.push({
        name: 'serverDetailed',
        params: {
          routeTitle: row.ServerName + '-节点详情',
          Id: row.ServerId
        }
      })
    },
    showRemote(row) {
      this.$router.push({
        name: 'serverDetailed',
        params: {
          routeTitle: row.RemoteServerName + '-节点详情',
          Id: row.RemoteId
        }
      })
    },
    async loadRegion() {
      this.region = await GetList()
    },
    async loadRpcMer() {
      this.rpcMer = await GetItems()
    },
    showLog(row) {
      this.logId = row.Id
      this.visible = true
    },
    expendRow(row, expands) {
      if (expands.length > 0 && expands.find(c => c.Id === row.Id) !== null) {
        this.loadTraceLog(row)
      }
    },
    loadTraceLog(row) {
      if (row.IsLoad) {
        TraceApi.GetByTraceId(row.TraceId).then((list) => {
          row.TraceLog = list
          row.IsLoad = false
        })
      }
    },
    reset() {
      this.pagination.page = 1
      this.queryParam.RpcMerId = null
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
    traceIndexMethod(index) {
      return index + 1
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
        RegionId: this.queryParam.RegionId,
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
      const res = await TraceApi.Query(query, this.pagination)
      res.List.forEach(c => {
        c.IsLoad = true
        c.TraceLog = []
      })
      this.traceLog = res.List
      this.pagination.total = res.Count
    },
    handleNodeClick(data) {
      this.queryParam.SystemType = data.TypeVal
      this.loadTable()
    },
    async loadTree() {
      const res = await GetGroupAndType()
      this.groupType = this.format(res)
    },
    format(rows) {
      return rows.map(c => {
        this.expandedKeys.push(c.TypeVal)
        return {
          type: 'group',
          TypeVal: c.TypeVal,
          Id: c.Id,
          label: c.GroupName,
          disabled: true,
          children: c.ServerType.map(a => {
            return {
              type: 'serverType',
              TypeVal: a.TypeVal,
              Id: a.Id,
              label: a.SystemName
            }
          })
        }
      })
    }
  }
}
</script>
