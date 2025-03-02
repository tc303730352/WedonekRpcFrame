<template>
  <el-dialog
    title="选择绑定的服务节点"
    :visible="visible"
    width="75%"
    :before-close="handleClose"
  >
    <el-row :gutter="24" type="flex" justify="center">
      <el-col :span="3">
        <el-card>
          <span slot="header">
            服务器类目
          </span>
          <el-tree node-key="TypeVal" :data="groupType" :props="defaultProps" :default-expanded-keys="expandedKeys" @node-click="handleNodeClick" />
        </el-card>
      </el-col>
      <el-col :span="21">
        <el-card>
          <span slot="header">
            服务器节点
          </span>
          <el-form :inline="true" :model="queryParam">
            <el-form-item label="关键字">
              <el-input v-model="queryParam.QueryKey" placeholder="名称/IP/MAC/版本号" />
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
              <el-select v-model="queryParam.RegionId" multiple clearable placeholder="所在机房">
                <el-option v-for="item in region" :key="item.Id" :label="item.RegionName" :value="item.Id" />
              </el-select>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="search">查询</el-button>
              <el-button type="default" @click="reset">重置</el-button>
            </el-form-item>
          </el-form>
          <div class="chiose-server">
            <span class="title"> 新增节点数: {{ chioseRow.length }}</span>
            <el-tag
              v-for="item in chioseRow"
              :key="item.Id"
              effect="plain"
            >
              {{ item.ServerName }}
            </el-tag>
          </div>
          <el-card>
            <el-table
              ref="servers"
              :data="serverList"
              row-key="Id"
              style="width: 100%"
              @selection-change="chioseChange"
              @sort-change="sortChange"
            >
              <el-table-column
                type="selection"
                :reserve-selection="true"
                :selectable="(row,index) => {
                  return (bindServerId[pagination.page] && bindServerId[pagination.page].bind.includes(row.Id)) == false
                }"
                width="55"
              />
              <el-table-column
                prop="ServerName"
                label="服务名"
                fixed="left"
                sortable="custom"
                width="250"
              />
              <el-table-column
                prop="ServerIp"
                label="内网IP"
                sortable="custom"
                width="150"
              >
                <template slot-scope="scope">
                  <span>{{ scope.row.ServerIp + ':' + scope.row.ServerPort }}</span>
                </template>
              </el-table-column>
              <el-table-column
                prop="SystemTypeName"
                label="服务类目"
                sortable="custom"
                width="300"
              >
                <template slot-scope="scope">
                  <span>{{ scope.row.GroupName + '-' + scope.row.SystemTypeName }}</span>
                </template>
              </el-table-column>
              <el-table-column
                prop="Region"
                label="所在机房"
                sortable="custom"
                width="100"
              />
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
                prop="ServiceState"
                label="状态"
                sortable="custom"
                width="80"
              >
                <template slot-scope="scope">
                  <span :style="{ color: ServerState[scope.row.ServiceState].color }">{{ ServerState[scope.row.ServiceState].text }}</span>
                </template>
              </el-table-column>
              <el-table-column
                prop="ServerMac"
                label="MAC"
                sortable="custom"
                width="130"
              />
            </el-table>
            <el-row style="text-align: right;">
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
    <el-row slot="footer" style="text-align: center;">
      <el-button type="primary" :loading="isLoad" @click="save()">保存</el-button>
      <el-button type="default" @click="handleClose">关闭</el-button>
    </el-row>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import { GetGroupAndType } from '@/api/groupType/serverGroup'
import * as serverApi from '@/api/server/server'
import * as serverBindApi from '@/api/rpcMer/serverBind'
import { GetList } from '@/api/basic/region'
import { ServerState } from '@/config/publicDic'
export default {
  props: {
    rpcMerId: {
      type: String,
      default: null
    },
    visible: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      ServerState,
      groupType: [],
      bindServerId: {},
      chioseRow: [],
      defaultProps: {
        children: 'children',
        label: 'label'
      },
      expandedKeys: [],
      isLoad: false,
      pagination: {
        size: 20,
        page: 1,
        totoal: 0,
        sort: null,
        order: null
      },
      region: [],
      serverList: [],
      queryParam: {
        QueryKey: null,
        ServiceState: [2, 1, 0],
        SystemTypeId: null,
        GroupId: null,
        RegionId: null,
        IsOnline: null,
        IsContainer: null,
        RpcMerId: null
      }
    }
  },
  watch: {
    visible: {
      handler(val) {
        if (val) {
          this.loadTree()
          this.loadRegion()
          this.bindServerId = {
            1: {
              bind: [],
              isLoad: false
            }
          }
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
    handleClose() {
      this.$emit('cancel', false)
    },
    chioseChange(vals) {
      this.chioseRow = vals
    },
    reset() {
      this.pagination.page = 1
      this.queryParam.GroupId = null
      this.queryParam.SystemTypeId = null
      this.queryParam.ServiceState = []
      this.queryParam.RegionId = []
      this.queryParam.IsOnline = null
      this.queryParam.IsContainer = null
      this.queryParam.QueryKey = null
      this.loadTable()
    },
    async checkBind() {
      if (this.serverList.length === 0) {
        this.isLoad = false
        return
      }
      const item = this.bindServerId[this.pagination.page]
      if (!item.isLoad) {
        item.isLoad = true
        const ids = await serverBindApi.CheckIsBind(this.rpcMerId, this.serverList.map(c => c.Id))
        if (ids.length > 0) {
          item.bind = ids
          const items = this.serverList.filter(c => ids.includes(c.Id))
          items.forEach(c => {
            this.$refs.servers.toggleRowSelection(c, true)
          })
        }
      }
      this.isLoad = false
    },
    pagingChange(index) {
      this.pagination.page = index
      if (!this.bindServerId[index]) {
        this.bindServerId[index] = {
          bind: [],
          isLoad: false
        }
      }
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
    async loadRegion() {
      if (this.region.length === 0) {
        this.region = await GetList()
      }
    },
    async loadTable() {
      this.isLoad = true
      const res = await serverApi.Query(this.queryParam, this.pagination)
      this.serverList = res.List
      this.pagination.total = res.Count
      this.checkBind()
    },
    handleNodeClick(data) {
      if (data.type === 'group') {
        this.queryParam.GroupId = data.Id
        this.queryParam.SystemTypeId = null
      } else {
        this.queryParam.GroupId = null
        this.queryParam.SystemTypeId = [data.Id]
      }
      this.loadTable()
    },
    async loadTree() {
      if (this.groupType.length === 0) {
        const res = await GetGroupAndType()
        this.groupType = this.format(res)
      }
    },
    async save() {
      if (this.chioseRow.length === 0) {
        this.$message({
          message: '请选择添加的节点！',
          type: 'warning'
        })
        return
      }
      this.isLoad = true
      await serverBindApi.Set(this.rpcMerId, this.chioseRow.map(c => c.Id))
      this.$message({
        message: '保存成功！',
        type: 'success'
      })
      this.isLoad = false
      this.$emit('cancel', true)
    },
    format(rows) {
      return rows.map(c => {
        this.expandedKeys.push(c.TypeVal)
        return {
          type: 'group',
          TypeVal: c.TypeVal,
          Id: c.Id,
          label: c.GroupName,
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

<style lang="scss" scoped>
.chiose-server {
  line-height: 50px;
  height: 50px;
  width: 100%;
  display: inline-block;
  .title {
    padding-right: 15px;
  }
  .el-tag {
    margin-right: 10px;
  }
}
</style>
