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
          <span slot="header"> 屏蔽列表 </span>
          <el-form :inline="true" :model="queryParam">
            <el-form-item label="资源路径/指令名">
              <el-input
                v-model="queryParam.Path"
                placeholder="资源路径/指令名"
              />
            </el-form-item>
            <el-form-item label="是否已过期">
              <el-select v-model="queryParam.IsOverTime" clearable placeholder="是否已过期">
                <el-option label="过期" value="true" />
                <el-option label="未过期" value="false" />
              </el-select>
            </el-form-item>
            <el-form-item label="屏蔽类型">
              <el-select v-model="queryParam.ShieldType" clearable placeholder="屏蔽类型">
                <el-option :value="0" label="Api接口" />
                <el-option :value="1" label="RPC方法指令" />
              </el-select>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="search">查询</el-button>
              <el-button type="default" @click="reset">重置</el-button>
              <el-button type="success" @click="add">新增</el-button>
            </el-form-item>
          </el-form>
          <el-card>
            <el-table
              :data="resourceList"
              style="width: 100%"
              @sort-change="sortChange"
            >
              <el-table-column type="index" fixed="left" :index="indexMethod" />
              <el-table-column
                prop="ResourcePath"
                label="资源路径"
                sortable="custom"
                min-width="200"
              />
              <el-table-column
                prop="ShieldType"
                label="屏蔽类型"
                sortable="custom"
                min-width="150"
              >
                <template slot-scope="scope">
                  <el-tag v-if="scope.row.ShieldType == 1" type="success">API接口</el-tag>
                  <el-tag v-else type="default">RPC方法指令</el-tag>
                </template>
              </el-table-column>
              <el-table-column
                prop="ShieIdShow"
                label="屏蔽说明"
                min-width="150"
              />
              <el-table-column
                prop="SystemType"
                label="系统类别名"
                sortable="custom"
                min-width="150"
              >
                <template slot-scope="scope">
                  {{ scope.row.SystemTypeName }}
                </template>
              </el-table-column>
              <el-table-column
                prop="ServerId"
                label="服务节点名"
                sortable="custom"
                min-width="150"
              >
                <template slot-scope="scope">
                  <el-link v-if="scope.row.ServerId !=0" @click="showDetailed(scope.row)">{{ scope.row.ServerName }}</el-link>
                </template>
              </el-table-column>
              <el-table-column
                prop="ApiVer"
                label="接口版本"
                sortable="custom"
                width="110"
              />
              <el-table-column
                prop="SortNum"
                label="优先级"
                sortable="custom"
                width="100"
              />
              <el-table-column prop="BeOverdueTime" label="过期时间" sortable="custom" min-width="150">
                <template slot-scope="scope">
                  <template v-if="scope.row.BeOverdueTime">
                    <el-tag :type="moment(scope.row.BeOverdueTime) > now ? 'danger' : 'info'">{{ moment(scope.row.BeOverdueTime).format("YYYY-MM-DD HH:mm") }}</el-tag>
                  </template>
                  <el-tag v-else type="danger">永久有效</el-tag>
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
                      type="primary"
                      @click="cancelShieId(scope.row)"
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
        </el-card>
      </el-col>
    </el-row>
    <addShield :visible="visible" :rpc-mer-id="rpcMerId" @cancel="closeAdd" />
  </div>
</template>
<script>
import moment from 'moment'
import * as serverBindApi from '@/api/rpcMer/serverBind'
import * as shieIdApi from '@/api/module/resourceShieId'
import addShield from './addShield'
export default {
  components: {
    addShield
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
      oldRpcMerId: null,
      groupType: [],
      visible: false,
      now: moment(),
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
      resourceList: [],
      queryParam: {
        Path: null,
        RpcMerId: null,
        SystemType: null,
        IsOverTime: null,
        ShieldType: null
      }
    }
  },
  watch: {
    isLoad: {
      handler(val) {
        if (val) {
          this.loadTree()
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
    closeAdd() {
      this.visible = false
      this.reset()
    },
    add() {
      this.visible = true
    },
    async cancelShieId(row) {
      await shieIdApi.CancelShieId(row.Id)
      this.$message({
        message: '取消成功！',
        type: 'success'
      })
      this.loadTable()
    },
    reset() {
      this.pagination.page = 1
      this.queryParam.RpcMerId = this.rpcMerId
      this.queryParam.SystemType = null
      this.queryParam.IsOverTime = null
      this.queryParam.Path = null
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
      const res = await shieIdApi.Query(this.queryParam, this.pagination)
      this.now = moment()
      this.resourceList = res.List
      this.pagination.total = res.Count
    },
    handleNodeClick(data) {
      this.queryParam.SystemType = data.TypeVal
      this.loadTable()
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
    async loadTree() {
      if (this.groupType.length === 0 || this.rpcMerId !== this.oldRpcMerId) {
        const res = await serverBindApi.GetGroupAndType(this.rpcMerId, null)
        this.groupType = this.format(res)
      }
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
              label: a.SystemName + '(' + a.ServerNum + ')',
              fullName: c.GroupName + '-' + a.SystemName,
              ServerNum: a.ServerNum
            }
          })
        }
      })
    }
  }
}
</script>
