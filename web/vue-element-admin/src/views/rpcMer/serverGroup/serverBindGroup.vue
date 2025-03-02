<template>
  <div>
    <el-row style="margin-top: 20px;" :gutter="24" type="flex" justify="center">
      <el-col :span="4">
        <el-card>
          <span slot="header">
            服务器类目
          </span>
          <el-tree node-key="TypeVal" :data="groupType" :default-expanded-keys="expandedKeys" @node-click="handleNodeClick" />
        </el-card>
      </el-col>
      <el-col :span="20">
        <el-card>
          <span slot="header"> 关联的服务节点 </span>
          <el-form :inline="true" :model="queryParam">
            <el-form-item label="关键字">
              <el-input v-model="queryParam.QueryKey" placeholder="名称/IP/MAC/版本号" />
            </el-form-item>
            <el-form-item label="服务状态">
              <el-select v-model="queryParam.ServiceState" multiple placeholder="服务状态">
                <el-option v-for="item in ServerState" :key="item.value" :label="item.text" :value="item.value" />
              </el-select>
            </el-form-item>
            <el-form-item label="在线状态">
              <el-select v-model="queryParam.IsOnline" clearable placeholder="在线状态">
                <el-option label="在线" value="true" />
                <el-option label="离线" value="false" />
              </el-select>
            </el-form-item>
            <el-form-item label="承载方式">
              <el-select v-model="queryParam.IsContainer" clearable placeholder="承载方式">
                <el-option label="物理机" value="false" />
                <el-option label="容器" value="true" />
              </el-select>
            </el-form-item>
            <el-form-item label="所在机房">
              <el-select v-model="queryParam.RegionId" clearable placeholder="所在机房">
                <el-option v-for="item in region" :key="item.Id" :label="item.RegionName" :value="item.Id" />
              </el-select>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="search">查询</el-button>
              <el-button type="default" @click="reset">重置</el-button>
              <el-button type="success" @click="addBind">绑定服务节点</el-button>
            </el-form-item>
          </el-form>
          <el-card>
            <el-table :data="serverList" style="width: 100%" @sort-change="sortChange">
              <el-table-column type="index" fixed="left" :index="indexMethod" width="80" />
              <el-table-column prop="ServerName" label="服务名" fixed="left" sortable="custom">
                <template slot-scope="scope">
                  <el-link @click="showDetailed(scope.row)">{{ scope.row.ServerName }}</el-link>
                </template>
              </el-table-column>
              <el-table-column prop="ServerIp" label="服务IP">
                <template slot-scope="scope">
                  <span>{{ scope.row.ServerIp + ':' +scope.row.ServerPort }}</span>
                </template>
              </el-table-column>
              <el-table-column prop="ServerMac" label="MAC" sortable="custom" />
              <el-table-column prop="SystemType" label="服务类型" sortable="custom" min-width="120">
                <template slot-scope="scope">
                  <span>{{ scope.row.ServerGroup + '-' +scope.row.SystemType }}</span>
                </template>
              </el-table-column>
              <el-table-column prop="Region" label="所在机房" sortable="custom" width="100" />
              <el-table-column
                prop="IsOnline"
                label="是否在线"
                sortable="custom"
                width="100"
              >
                <template slot-scope="scope">
                  <el-tag v-if="scope.row.IsOnline" type="success">在线</el-tag>
                  <el-tag v-else type="danger">离线</el-tag>
                </template>
              </el-table-column>
              <el-table-column
                prop="IsContainer"
                label="承载方式"
                sortable="custom"
                width="100"
              >
                <template slot-scope="scope">
                  <span v-if="scope.row.IsContainer">容器</span>
                  <span v-else>物理机</span>
                </template>
              </el-table-column>
              <el-table-column
                prop="IsHold"
                label="是否属于"
                sortable="custom"
                width="100"
              >
                <template slot-scope="scope">
                  <el-tag v-if="scope.row.IsHold" type="success">是</el-tag>
                  <el-tag v-else type="default">否</el-tag>
                </template>
              </el-table-column>
              <el-table-column
                prop="ServiceState"
                label="状态"
                sortable="custom"
                width="80"
              >
                <template slot-scope="scope">
                  <span :style="{ color: ServerState[scope.row.ServiceState].color }">{{ ServerState[scope.row.ServiceState].text }}</span>
                </template>
              </el-table-column>
              <el-table-column prop="Weight" label="权重" width="80" sortable="custom" />
              <el-table-column
                prop="Action"
                min-width="210"
                label="操作"
              >
                <template slot-scope="scope">
                  <el-button-group v-if="scope.row.IsHold">
                    <el-button type="default" size="small" plain @click="showLimit(scope.row)">客户端限流</el-button>
                    <el-button type="default" size="small" plain @click="showReduceInRank(scope.row)">服务降级</el-button>
                  </el-button-group>
                  <el-button-group v-else>
                    <el-button type="danger" size="small" plain @click="deleteBind(scope.row)">删除</el-button>
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
    <ClientLimit :server-id="serverId" :visible="visibleLimit" @cancel="closeLimit" />
    <ReduceInRank :rpc-mer-id="rpcMerId" :server-id="serverId" :visible="visibleReduceInRank" @cancel="closeReduceInRank" />
    <BindServer :rpc-mer-id="rpcMerId" :visible="visible" @cancel="closeBind" />
  </div>
</template>
<script>
import moment from 'moment'
import * as serverBindApi from '@/api/rpcMer/serverBind'
import { GetList } from '@/api/basic/region'
import { ServerState } from '@/config/publicDic'
import BindServer from './BindServer'
import ClientLimit from '../clientLimit/clientLimit'
import ReduceInRank from '../reduceInRank/reduceInRankSet'
export default {
  components: {
    BindServer,
    ClientLimit,
    ReduceInRank
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
      ServerState,
      visible: false,
      visibleReduceInRank: false,
      pagination: {
        size: 20,
        page: 1,
        totoal: 0,
        sort: null,
        order: null
      },
      visibleLimit: false,
      serverId: null,
      region: [],
      groupType: [],
      serverList: [],
      expandedKeys: [],
      oldRpcMerId: null,
      queryParam: {
        QueryKey: null,
        GroupId: null,
        SystemTypeId: null,
        ServiceState: null,
        RegionId: null,
        IsOnline: null

      }
    }
  },
  watch: {
    isLoad: {
      handler(val) {
        if (val && this.rpcMerId) {
          this.loadTree()
          this.loadRegion()
          this.loadTable()
        }
      },
      immediate: true
    },
    rpcMerId: {
      handler(val) {
        if (val && this.isLoad) {
          this.loadTree()
          this.loadRegion()
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
    showDetailed(row) {
      this.$router.push({
        name: 'serverDetailed',
        params: {
          routeTitle: row.ServerName + '-节点详情',
          Id: row.ServerId
        }
      })
    },
    showReduceInRank(row) {
      this.serverId = row.ServerId
      this.visibleReduceInRank = true
    },
    closeReduceInRank() {
      this.visibleReduceInRank = false
    },
    closeLimit() {
      this.visibleLimit = false
    },
    showLimit(row) {
      this.serverId = row.ServerId
      this.visibleLimit = true
    },
    addBind() {
      this.visible = true
    },
    closeBind() {
      this.visible = false
      this.reset()
    },
    async loadRegion() {
      if (this.region.length === 0) {
        this.region = await GetList()
      }
    },
    indexMethod(index) {
      return (index + 1) + ((this.pagination.page - 1) * this.pagination.size)
    },
    async loadTable() {
      const res = await serverBindApi.Query(this.rpcMerId, this.queryParam, this.pagination)
      this.serverList = res.List
      this.pagination.total = res.Count
    },
    handleNodeClick(data) {
      if (data.type === 'group') {
        this.queryParam.GroupId = data.Id
        this.queryParam.SystemTypeId = null
      } else {
        this.queryParam.GroupId = null
        this.queryParam.SystemTypeId = data.Id
      }
      this.loadTable()
    },
    deleteBind(row) {
      const that = this
      this.$confirm('确定解除与(' + row.ServerName + ')服务节点的关联?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        that.delete(row)
      })
    },
    async delete(row) {
      await serverBindApi.Delete(row.BindId)
      this.$message({
        message: '解绑成功!',
        type: 'success'
      })
      this.loadTable()
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
          TypeVal: 'group_' + c.Id,
          Id: c.Id,
          label: c.GroupName,
          children: c.ServerType.map(a => {
            return {
              type: 'serverType',
              TypeVal: 'type_' + a.Id,
              Id: a.Id,
              label: a.SystemName
            }
          })
        }
      })
    },
    reset() {
      this.pagination.page = 1
      this.queryParam.QueryKey = null
      this.queryParam.GroupId = null
      this.queryParam.SystemTypeId = null
      this.queryParam.ServiceState = null
      this.queryParam.RegionId = null
      this.queryParam.IsOnline = null
      this.queryParam.IsContainer = null
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
    }
  }
}
</script>
<style scoped>
.el-button-group {
  .el-button {
    margin-left: 10px;
  }
}
</style>
