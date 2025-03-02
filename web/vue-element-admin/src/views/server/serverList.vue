<template>
  <div>
    <el-row style="margin-top: 20px;" :gutter="24" type="flex" justify="center">
      <el-col :sm="4" :xl="3">
        <el-card>
          <span slot="header">
            服务器类目
          </span>
          <el-tree node-key="TypeVal" :data="groupType" :default-expanded-keys="expandedKeys" @node-click="handleNodeClick" />
        </el-card>
      </el-col>
      <el-col :sm="20" :xl="21">
        <el-card>
          <span slot="header">
            服务器节点
          </span>
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
            <el-form-item v-if="activeName=='container'" label="容器节点">
              <el-select v-model="queryParam.ContainerGroup" clearable placeholder="所属容器节点">
                <el-option v-for="item in container" :key="item.Id" :label="item.Name" :value="item.Id" />
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
              <el-button v-if="activeName !=='container'" type="success" @click="add">新增节点</el-button>
            </el-form-item>
          </el-form>
          <el-tabs v-model="activeName" style="margin-top: 20px;" type="border-card" @tab-click="handleClick">
            <el-tab-pane label="物理机" name="physics">
              <serverTable ref="physicsList" :query-param="queryParam" />
            </el-tab-pane>
            <el-tab-pane label="容器" name="container">
              <serverTable ref="containerList" :query-param="queryParam" />
            </el-tab-pane>
          </el-tabs>
          <addServer :visible="visibleAdd" @cancel="closeAdd" />
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>
<script>
import moment from 'moment'
import serverTable from './components/serverTable'
import { GetGroupAndType } from '@/api/groupType/serverGroup'
import addServer from './components/serverAdd'
import { GetItems } from '@/api/server/containerGroup'
import { GetList } from '@/api/basic/region'
import { ServerState, ServiceType } from '@/config/publicDic'
export default {
  components: {
    serverTable,
    addServer
  },
  data() {
    return {
      ServiceType,
      ServerState,
      visibleAdd: false,
      activeName: 'physics',
      groupType: [],
      container: [],
      defaultProps: {
        children: 'children',
        label: 'label'
      },
      expandedKeys: [],
      region: [],
      queryParam: {
        QueryKey: null,
        ServiceState: [],
        SystemTypeId: null,
        GroupId: null,
        RegionId: null,
        IsOnline: null,
        IsContainer: null,
        ContainerGroup: null,
        RpcMerId: null
      }
    }
  },
  mounted() {
    this.loadTree()
    this.loadRegion()
    this.loadContainer()
    this.activeName = 'physics'
    this.reset()
  },
  methods: {
    moment,
    async loadContainer() {
      this.container = await GetItems()
    },
    handleClick(e) {
      this.activeName = e.name
      this.reset()
    },
    reset() {
      const param = this.$route.params
      this.queryParam.GroupId = null
      this.queryParam.SystemTypeId = null
      this.queryParam.RegionId = null
      this.queryParam.ServiceState = []
      this.queryParam.IsOnline = null
      this.queryParam.IsContainer = this.activeName === 'container'
      this.queryParam.ContainerGroup = null
      this.queryParam.QueryKey = null
      if (param != null) {
        for (const i in param) {
          if (i === 'systemTypeId') {
            this.queryParam.SystemTypeId = [param.systemTypeId]
          } else if (i === 'regionId') {
            this.queryParam.RegionId = [param.regionId]
          } else if (i === 'containerGroup') {
            this.activeName = 'container'
            this.queryParam.IsContainer = true
            this.queryParam.containerGroup = param.containerGroup
          }
        }
      }
      this.getTable().reset()
    },
    add() {
      this.visibleAdd = true
    },
    getTable() {
      if (this.activeName === 'physics') {
        return this.$refs.physicsList
      } else {
        return this.$refs.containerList
      }
    },
    closeAdd(isAdd) {
      this.visibleAdd = false
      if (isAdd) {
        this.reset()
      }
    },
    search() {
      this.loadTable()
    },
    loadTable() {
      this.getTable().loadTable()
    },
    async loadRegion() {
      this.region = await GetList()
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
