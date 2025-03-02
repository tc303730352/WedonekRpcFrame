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
          <span slot="header"> {{ title }} </span>
          <el-form :inline="true" :model="queryParam">
            <el-form-item label="应用的容器">
              <el-select v-model="queryParam.ContainerGroup" clearable placeholder="应用的容器">
                <el-option v-for="item in container" :key="item.Id" :label="item.Name" :value="item.Id" />
              </el-select>
            </el-form-item>
            <el-form-item label="应用的机房">
              <el-select v-model="queryParam.RegionId" clearable placeholder="来源机房">
                <el-option v-for="item in region" :key="item.Id" :label="item.RegionName" :value="item.Id" />
              </el-select>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="search">查询</el-button>
              <el-button type="default" @click="reset">重置</el-button>
              <el-button type="success" @click="add">新增</el-button>
            </el-form-item>
          </el-form>
          <el-table
            :data="configList"
            style="width: 100%"
            @sort-change="sortChange"
          >
            <el-table-column type="index" fixed="left" :index="indexMethod" />
            <el-table-column
              prop="Name"
              label="配置名"
              min-width="200"
              sortable="custom"
            />
            <el-table-column
              prop="Show"
              label="配置说明"
              min-width="200"
            />
            <el-table-column
              prop="ServerId"
              sortable="custom"
              label="应用的服务节点"
              min-width="200"
            >
              <template slot-scope="scope">
                <el-link @click="showDetailed(scope.row)">{{ scope.row.ServerName }}</el-link>
              </template>
            </el-table-column>
            <el-table-column
              prop="RegionId"
              label="应用的机房"
              sortable="custom"
              min-width="150"
            >
              <template slot-scope="scope">
                {{ scope.row.Region }}
              </template>
            </el-table-column>
            <el-table-column
              prop="SystemType"
              label="应用的服务类型"
              min-width="200"
              sortable="custom"
            >
              <template slot-scope="scope">
                {{ scope.row.SystemTypeName }}
              </template>
            </el-table-column>
            <el-table-column
              prop="ContainerGroup"
              label="应用的容器组"
              min-width="150"
              sortable="custom"
            >
              <template slot-scope="scope">
                {{ scope.row.ContainerGroupName }}
              </template>
            </el-table-column>
            <el-table-column
              prop="VerNumStr"
              label="应用的版本号"
              min-width="150"
              sortable="custom"
            />
            <el-table-column
              prop="IsEnable"
              label="是否启用"
              sortable="custom"
              width="110"
            >
              <template slot-scope="scope">
                <el-switch v-model="scope.row.IsEnable" @change="setIsEnable(scope.row)" />
              </template>
            </el-table-column>
            <el-table-column
              prop="Prower"
              label="权重"
              sortable="custom"
              width="100"
            />
            <el-table-column prop="ToUpdateTime" label="最后更新时间" sortable="custom" min-width="150">
              <template slot-scope="scope">
                {{ moment(scope.row.ToUpdateTime).format("YYYY-MM-DD HH:mm:ss") }}
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
                    @click="editConfig(scope.row)"
                  >编辑</el-button>
                  <el-button
                    size="small"
                    type="danger"
                    @click="deleteConfig(scope.row)"
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
      </el-col>
    </el-row>
    <sysConfigAdd :config-name="configName" :config-type="configType" :is-hold="isHold" :title="'新增'+ title" :service-type="serviceType" :form-view="addView" :rpc-mer-id="rpcMerId" :visible="visible" @cancel="closeAdd" />
    <sysConfigEdit :id="id" :config-name="configName" :title="'编辑'+ title" :form-view="editView" :visible="visibleEdit" @cancel="closeEdit" />
  </div>
</template>
<script>
import moment from 'moment'
import * as serverBindApi from '@/api/rpcMer/serverBind'
import * as configApi from '@/api/module/sysConfig'
import { GetList } from '@/api/basic/region'
import sysConfigAdd from './sysConfigAdd'
import sysConfigEdit from './sysConfigEdit.vue'
export default {
  components: {
    sysConfigAdd,
    sysConfigEdit
  },
  props: {
    rpcMerId: {
      type: String,
      default: null
    },
    configName: {
      type: String,
      default: null
    },
    serviceType: {
      type: Number,
      default: 2
    },
    title: {
      type: String,
      default: null
    },
    addView: {
      type: String,
      default: null
    },
    editView: {
      type: String,
      default: null
    },
    configType: {
      type: Number,
      default: 0
    },
    isLoad: {
      type: Boolean,
      default: false
    },
    isHold: {
      type: Boolean,
      default: true
    }
  },
  data() {
    return {
      groupType: [],
      container: [],
      id: null,
      visible: false,
      visibleEdit: false,
      defaultProps: {
        children: 'children',
        label: 'label'
      },
      expandedKeys: [],
      oldRpcMerId: null,
      region: [],
      pagination: {
        size: 20,
        page: 1,
        total: 0,
        sort: null,
        order: null
      },
      configList: [],
      queryParam: {
        RpcMerId: null,
        RegionId: null,
        ContainerGroup: null,
        SystemType: null,
        ConfigName: null
      }
    }
  },
  watch: {
    isLoad: {
      handler(val) {
        if (val && this.rpcMerId) {
          this.loadRegion()
          this.load()
          this.loadTree()
          this.reset()
        }
      },
      immediate: true
    },
    rpcMerId: {
      handler(val) {
        if (val && this.isLoad) {
          this.loadRegion()
          this.load()
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
    showDetailed(row) {
      this.$router.push({
        name: 'serverDetailed',
        params: {
          routeTitle: row.ServerName + '-节点详情',
          Id: row.ServerId
        }
      })
    },
    closeAdd(isRefresh) {
      this.visible = false
      if (isRefresh) {
        this.reset()
      }
    },
    editConfig(row) {
      this.id = row.Id
      this.visibleEdit = true
    },
    closeEdit(isRefresh) {
      this.visibleEdit = false
      if (isRefresh) {
        this.reset()
      }
    },
    add() {
      this.visible = true
    },
    async loadRegion() {
      if (this.region.length === 0) {
        this.region = await GetList()
      }
    },
    async load() {
      if (this.container.length === 0 || this.oldRpcMerId !== this.rpcMerId) {
        this.container = serverBindApi.GetContainerGroup(this.rpcMerId, null, null, this.isHold)
      }
    },
    deleteConfig(row) {
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
    async setIsEnable(row) {
      await configApi.SetIsEnable(row.Id, row.IsEnable)
      this.$message({
        message: '设置成功！',
        type: 'success'
      })
    },
    async delete(row) {
      await configApi.Delete(row.Id)
      this.$message({
        message: '删除成功！',
        type: 'success'
      })
      this.loadTable()
    },
    reset() {
      this.pagination.page = 1
      this.queryParam.ConfigType = this.configType
      this.queryParam.RpcMerId = this.rpcMerId
      this.queryParam.ConfigName = this.configName
      this.queryParam.RegionId = null
      this.queryParam.ContainerGroup = null
      this.queryParam.SystemType = null
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
      const res = await configApi.Query(this.queryParam, this.pagination)
      this.configList = res.List
      this.pagination.total = res.Count
    },
    handleNodeClick(data) {
      this.queryParam.SystemType = data.TypeVal
      this.loadTable()
    },
    async loadTree() {
      if (this.groupType.length === 0 || this.oldRpcMerId !== this.rpcMerId) {
        this.oldRpcMerId = this.rpcMerId
        const res = await serverBindApi.GetGroupAndType(this.rpcMerId, null, this.serviceType, this.isHold)
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
