<template>
  <div>
    <el-row style="margin-top: 20px" :gutter="24" type="flex" justify="center">
      <el-col :span="3">
        <el-card>
          <span slot="header"> 系统类别 </span>
          <el-tree node-key="key" :data="groupType" :default-expanded-keys="expandedKeys" @node-click="handleNodeClick" />
        </el-card>
      </el-col>
      <el-col :span="21">
        <el-card>
          <span slot="header"> 事务日志 </span>
          <el-form :inline="true" :model="queryParam">
            <el-form-item label="事务名">
              <el-input
                v-model="queryParam.TranName"
                placeholder="事务名"
              />
            </el-form-item>
            <el-form-item label="发起机房">
              <el-select v-model="queryParam.RegionId" clearable placeholder="发起机房" @change="chooseRegion">
                <el-option v-for="item in region" :key="item.Id" :label="item.RegionName" :value="item.Id" />
              </el-select>
            </el-form-item>
            <el-form-item label="发起节点">
              <el-select v-model="queryParam.ServerId" clearable placeholder="发起节点">
                <el-option v-for="item in servers" :key="item.ServerId" :label="item.ServerName" :value="item.ServerId" />
              </el-select>
            </el-form-item>
            <el-form-item label="事务状态">
              <el-select v-model="queryParam.TranStatus" multiple clearable placeholder="事务状态">
                <el-option v-for="item in TranStatus" :key="item.value" :label="item.text" :value="item.value" />
              </el-select>
            </el-form-item>
            <el-form-item label="发起日期">
              <el-date-picker v-model="queryParam.Begin" clearable placeholder="发起日期" />
            </el-form-item>
            <el-form-item label="截止日期">
              <el-date-picker v-model="queryParam.End" clearable placeholder="截止日期" />
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="search">查询</el-button>
              <el-button type="default" @click="reset">重置</el-button>
            </el-form-item>
          </el-form>
          <el-card>
            <el-table
              :data="tranList"
              style="width: 100%"
              row-key="Id"
              @expand-change="expendRow"
              @sort-change="sortChange"
            >
              <el-table-column type="index" fixed="left" :index="indexMethod" />
              <el-table-column type="expand">
                <template slot-scope="scope">
                  <el-table
                    v-loading="scope.row.IsLoad"
                    :data="scope.row.TranList"
                    row-key="Id"
                    style="width: 100%"
                    default-expand-all
                    :tree-props="{children: 'Children'}"
                  >
                    <el-table-column type="index" :index="tranSubIndexMethod" />
                    <el-table-column
                      prop="ServerName"
                      label="执行的节点"
                      min-width="150"
                    >
                      <template slot-scope="tranScope">
                        <el-link @click="showDetailed(tranScope.row)">{{ tranScope.row.ServerName }}</el-link>
                      </template>
                    </el-table-column>
                    <el-table-column
                      prop="RpcMerName"
                      label="执行的集群"
                      min-width="120"
                    />
                    <el-table-column
                      prop="SystemTypeName"
                      label="执行的节点类型"
                      min-width="120"
                    />
                    <el-table-column
                      prop="Region"
                      label="所在机房"
                      min-width="120"
                    />
                    <el-table-column
                      prop="TranName"
                      label="事务名"
                      min-width="150"
                    />
                    <el-table-column
                      prop="TranStatus"
                      label="事务状态"
                      min-width="80"
                    >
                      <template slot-scope="tranScope">
                        <span>{{ TranStatus[tranScope.row.TranStatus].text }}</span>
                      </template>
                    </el-table-column>
                    <el-table-column
                      prop="TranMode"
                      label="事务模式"
                      min-width="80"
                    >
                      <template slot-scope="tranScope">
                        <span>{{ TranMode[tranScope.row.TranMode].text }}</span>
                      </template>
                    </el-table-column>
                    <el-table-column
                      prop="Error"
                      label="错误码"
                      min-width="150"
                    />
                    <el-table-column
                      prop="IsLock"
                      label="是否被锁定"
                      min-width="80"
                    >
                      <template slot-scope="tranScope">
                        <el-tag v-if="tranScope.row.IsLock" type="danger">已锁定</el-tag>
                        <el-tag v-else>未锁定</el-tag>
                      </template>
                    </el-table-column>
                    <el-table-column
                      prop="lastTime"
                      label="最后操作时间"
                      min-width="150"
                    >
                      <template slot-scope="tranScope">
                        <span v-if="tranScope.row.IsLock">{{ moment(tranScope.row.LockTime).format('YYYY-MM-DD HH:mm:ss.ms') }}</span>
                        <span v-else-if="tranScope.row.TranStatus == 3 || tranScope.row.TranStatus == 2">{{ moment(tranScope.row.FailTime).format('YYYY-MM-DD HH:mm:ss.ms') }}</span>
                        <span v-else-if="tranScope.row.CommitStatus == 1 || tranScope.row.CommitStatus == 3">{{ moment(tranScope.row.EndTime).format('YYYY-MM-DD HH:mm:ss.ms') }}</span>
                        <span v-else>{{ moment(tranScope.row.AddTime).format('YYYY-MM-DD HH:mm:ss.ms') }}</span>
                      </template>
                    </el-table-column>
                    <el-table-column
                      prop="Action"
                      fixed="right"
                      label="操作"
                      width="100"
                    >
                      <template slot-scope="tranScope">
                        <el-button-group>
                          <el-button
                            size="small"
                            plain
                            @click="showLog(tranScope.row)"
                          >详细</el-button>
                        </el-button-group>
                      </template>
                    </el-table-column>
                  </el-table>
                </template>
              </el-table-column>
              <el-table-column
                prop="ServerId"
                label="发起节点"
                min-width="150"
                sortable="custom"
              >
                <template slot-scope="scope">
                  <el-link @click="showDetailed(scope.row)">{{ scope.row.ServerName }}</el-link>
                </template>
              </el-table-column>
              <el-table-column
                prop="SystemType"
                label="发起节点类型"
                min-width="120"
                sortable="custom"
              >
                <template slot-scope="scope">
                  {{ scope.row.SystemTypeName }}
                </template>
              </el-table-column>
              <el-table-column
                prop="RegionId"
                label="发起机房"
                sortable="custom"
                min-width="120"
              >
                <template slot-scope="scope">
                  {{ scope.row.Region }}
                </template>
              </el-table-column>
              <el-table-column
                prop="TranName"
                label="事务名"
                min-width="150"
              />
              <el-table-column
                prop="TranStatus"
                label="事务状态"
                sortable="custom"
                min-width="80"
              >
                <template slot-scope="scope">
                  <span>{{ TranStatus[scope.row.TranStatus].text }}</span>
                </template>
              </el-table-column>
              <el-table-column
                prop="TranMode"
                label="事务模式"
                sortable="custom"
                min-width="80"
              >
                <template slot-scope="scope">
                  <span>{{ TranMode[scope.row.TranMode].text }}</span>
                </template>
              </el-table-column>
              <el-table-column
                prop="Error"
                label="错误码"
                sortable="custom"
                min-width="150"
              />
              <el-table-column
                prop="IsLock"
                label="是否被锁定"
                sortable="custom"
                min-width="90"
              >
                <template slot-scope="scope">
                  <el-tag v-if="scope.row.IsLock" type="danger">已锁定</el-tag>
                  <el-tag v-else>未锁定</el-tag>
                </template>
              </el-table-column>
              <el-table-column
                prop="OverTime"
                label="设定超时时间"
                sortable="custom"
                min-width="140"
              >
                <template slot-scope="scope">
                  {{ moment(scope.row.OverTime).format('YYYY-MM-DD HH:mm:ss.ms') }}
                </template>
              </el-table-column>
              <el-table-column
                prop="lastTime"
                label="最后操作时间"
                min-width="150"
              >
                <template slot-scope="scope">
                  <span v-if="scope.row.IsLock">{{ moment(scope.row.LockTime).format('YYYY-MM-DD HH:mm:ss.ms') }}</span>
                  <span v-else-if="scope.row.TranStatus == 3 || scope.row.TranStatus == 2">{{ moment(scope.row.FailTime).format('YYYY-MM-DD HH:mm:ss.ms') }}</span>
                  <span v-else-if="scope.row.CommitStatus == 1 || scope.row.CommitStatus == 3">{{ moment(scope.row.EndTime).format('YYYY-MM-DD HH:mm:ss.ms') }}</span>
                  <span v-else-if="scope.row.TranStatus == 1">{{ moment(scope.row.SubmitTime).format('YYYY-MM-DD HH:mm:ss.ms') }}</span>
                  <span v-else>{{ moment(scope.row.AddTime).format('YYYY-MM-DD HH:mm:ss.ms') }}</span>
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
    <tranasctionDetailed :id="id" :visible="visible" @cancel="visible=false" />
  </div>
</template>
<script>
import moment from 'moment'
import * as serverBindApi from '@/api/rpcMer/serverBind'
import * as tranApi from '@/api/module/transaction'
import { TranStatus, TranMode } from '@/config/publicDic'
import { GetList } from '@/api/basic/region'
import tranasctionDetailed from './tranasctionDetailed.vue'
export default {
  components: {
    tranasctionDetailed
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
      TranStatus,
      TranMode,
      region: [],
      visible: false,
      id: null,
      groupType: [],
      servers: [],
      oldRpcMerId: null,
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
      tranList: [],
      systemTypeId: null,
      queryParam: {
        RpcMerId: null,
        TranName: null,
        ServerId: null,
        SystemType: null,
        RegionId: null,
        TranStatus: null,
        Begin: null,
        End: null
      }
    }
  },
  watch: {
    isLoad: {
      handler(val) {
        if (val) {
          this.loadTree()
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
    showLog(row) {
      this.id = row.Id
      this.visible = true
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
    reset() {
      this.systemTypeId = null
      this.pagination.page = 1
      this.queryParam.RpcMerId = this.rpcMerId
      this.queryParam.TranName = null
      this.queryParam.ServerId = null
      this.queryParam.SystemType = null
      this.queryParam.RegionId = null
      this.queryParam.TranStatus = null
      this.queryParam.Begin = null
      this.queryParam.End = null
      this.loadServers()
      this.loadTable()
    },
    expendRow(row, expands) {
      if (expands.length > 0 && expands.find(c => c.Id === row.Id) !== null) {
        this.loadTranTree(row)
      }
    },
    loadTranTree(row) {
      if (!row.IsLoad && row.TranList.length === 0) {
        row.IsLoad = true
        tranApi.GetTree(row.Id).then((list) => {
          row.TranList = list
          row.IsLoad = false
        })
      }
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
    tranSubIndexMethod(index) {
      return index + 1
    },
    formatQueryParam() {
      for (const i in this.queryParam) {
        if (this.queryParam[i] === '') {
          this.queryParam[i] = null
        }
      }
    },
    async loadRegion() {
      if (this.region.length === 0) {
        this.region = await GetList()
      }
    },
    async loadTable() {
      this.formatQueryParam()
      const res = await tranApi.Query(this.queryParam, this.pagination)
      res.List.forEach(c => {
        c.IsLoad = false
        c.TranList = []
      })
      this.tranList = res.List
      this.pagination.total = res.Count
    },
    handleNodeClick(data) {
      this.queryParam.SystemType = data.TypeVal
      this.systemTypeId = data.Id
      this.loadServers()
    },
    chooseRegion() {
      if (this.queryParam.RegionId === '') {
        this.queryParam.RegionId = null
      }
      this.loadServers()
    },
    async loadTree() {
      if (this.groupType.length === 0 || this.rpcMerId !== this.oldRpcMerId) {
        const res = await serverBindApi.GetGroupAndType(this.rpcMerId, null)
        this.groupType = this.format(res)
      }
    },
    async loadServers() {
      this.servers = await serverBindApi.GetItems({
        RpcMerId: this.rpcMerId,
        RegionId: this.queryParam.RegionId,
        SystemTypeId: this.systemTypeId
      })
    },
    format(rows) {
      return rows.map(c => {
        this.expandedKeys.push('group_' + c.Id)
        return {
          type: 'group',
          key: 'group_' + c.Id,
          Id: c.Id,
          label: c.GroupName,
          disabled: true,
          children: c.ServerType.map(a => {
            return {
              type: 'serverType',
              key: 'type_' + a.Id,
              Id: a.Id,
              TypeVal: a.TypeVal,
              label: a.SystemName + '(' + a.ServerNum + ')'
            }
          })
        }
      })
    }
  }
}
</script>
