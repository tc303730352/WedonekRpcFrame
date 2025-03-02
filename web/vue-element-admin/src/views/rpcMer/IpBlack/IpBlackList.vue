<template>
  <div>
    <el-row style="margin-top: 20px;" :gutter="24" type="flex" justify="center">
      <el-col :span="4">
        <el-card>
          <span slot="header">
            服务器类目
          </span>
          <el-tree node-key="key" :data="groupType" :default-expanded-keys="expandedKeys" @node-click="handleNodeClick" />
        </el-card>
      </el-col>
      <el-col :span="20">
        <el-card>
          <span slot="header"> IP黑名单 </span>
          <el-form :inline="true" :model="queryParam">
            <el-form-item label="IP地址">
              <el-input
                v-model="queryParam.Ip"
                placeholder="IP地址"
              />
            </el-form-item>
            <el-form-item label="IP类型">
              <el-select v-model="queryParam.IpType" clearable placeholder="IP类型">
                <el-option label="Ip4" :value="0" />
                <el-option label="Ip6" :value="1" />
              </el-select>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="search">查询</el-button>
              <el-button type="default" @click="reset">重置</el-button>
              <el-button type="success" @click="add">添加</el-button>
            </el-form-item>
          </el-form>
          <el-card>
            <el-table
              :data="ipBlack"
              style="width: 100%"
              @sort-change="sortChange"
            >
              <el-table-column type="index" fixed="left" :index="indexMethod" />
              <el-table-column
                prop="IpType"
                label="IP类型"
                sortable="custom"
                min-width="120"
              >
                <template slot-scope="scope">
                  <span v-if="scope.row.IpType ==0">IP4</span>
                  <span v-else-if="scope.row.IpType ==1">IP6</span>
                </template>
              </el-table-column>
              <el-table-column
                prop="SystemType"
                label="应用节点类型"
                sortable="custom"
                min-width="150"
              >
                <template slot-scope="scope">
                  {{ scope.row.SystemTypeName }}
                </template>
              </el-table-column>
              <el-table-column
                prop="Ip"
                sortable="custom"
                label="IP地址"
                min-width="300"
              />
              <el-table-column
                prop="Remark"
                label="备注"
                min-width="150"
              >
                <template slot-scope="scope">
                  <el-input v-model="scope.row.Remark">
                    <el-button slot="append" type="primary" @click="saveRemark(scope.row)">保存</el-button>
                  </el-input>
                </template>
              </el-table-column>
              <el-table-column
                prop="Action"
                fixed="right"
                label="操作"
                width="200"
              >
                <template slot-scope="scope">
                  <el-button
                    size="small"
                    type="danger"
                    @click="dropIpBlack(scope.row)"
                  >删除</el-button>
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
    <addIpBlack :visible="visible" :rpc-mer-id="rpcMerId" @cancel="close" />
  </div>
</template>
<script>
import moment from 'moment'
import * as serverBindApi from '@/api/rpcMer/serverBind'
import * as ipBlackApi from '@/api/module/ipBlack'
import addIpBlack from './addIpBlack.vue'
export default {
  components: {
    addIpBlack
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
      visible: false,
      groupType: [],
      expandedKeys: [],
      ipBlack: [],
      oldRpcMerId: null,
      queryParam: {
        Ip: null,
        IpType: null,
        SystemType: null,
        RpcMerId: null
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
    close(isRefresh) {
      this.visible = false
      if (isRefresh) {
        this.loadTable()
      }
    },
    add() {
      this.visible = true
    },
    handleNodeClick(data) {
      if (data.type === 'serverType') {
        this.queryParam.SystemType = data.TypeVal
        this.loadTable()
      }
    },
    reset() {
      this.pagination.page = 1
      this.queryParam.Ip = null
      this.queryParam.IpType = null
      this.queryParam.SystemType = null
      this.queryParam.RpcMerId = this.rpcMerId
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
    dropIpBlack(row) {
      const that = this
      this.$confirm('确定删除?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        that.drop(row.Id)
      })
    },
    async saveRemark(row) {
      await ipBlackApi.SetRemark(row.Id, row.Remark)
      this.$message({
        message: '保存成功',
        type: 'success'
      })
    },
    async drop(id) {
      await ipBlackApi.Drop(id)
      this.$message({
        message: '删除成功',
        type: 'success'
      })
      this.loadTable()
    },
    async loadTable() {
      this.formatQueryParam()
      const res = await ipBlackApi.Query(this.queryParam, this.pagination)
      this.ipBlack = res.List
      this.pagination.total = res.Count
    },
    async loadTree() {
      if (this.groupType.length === 0 || this.rpcMerId !== this.oldRpcMerId) {
        const res = await serverBindApi.GetGroupAndType(this.rpcMerId, null, 2, true)
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
          children: c.ServerType.map(a => {
            return {
              type: 'serverType',
              key: 'type_' + a.Id,
              label: a.SystemName + '(' + a.ServerNum + ')',
              fullName: c.GroupName + '-' + a.SystemName,
              TypeVal: a.TypeVal
            }
          })
        }
      })
    }
  }
}
</script>
