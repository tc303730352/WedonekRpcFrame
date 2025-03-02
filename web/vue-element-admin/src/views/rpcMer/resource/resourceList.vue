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
          <span slot="header"> 资源模块 </span>
          <el-form :inline="true" :model="queryParam">
            <el-form-item label="关键字">
              <el-input
                v-model="queryParam.QueryKey"
                placeholder="备注名/资源名"
              />
            </el-form-item>
            <el-form-item label="资源类型">
              <el-select v-model="queryParam.ResourceType" clearable placeholder="资源类型">
                <el-option label="API接口" value="2" />
                <el-option label="RPC接口" value="4" />
              </el-select>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="search">查询</el-button>
              <el-button type="default" @click="reset">重置</el-button>
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
                prop="ModularName"
                label="模块名称"
                sortable="custom"
                min-width="120"
              />
              <el-table-column
                prop="ResourceType"
                label="资源类型"
                sortable="custom"
                min-width="80"
              >
                <template slot-scope="scope">
                  <span v-if="scope.row.ResourceType ==2">API接口</span>
                  <span v-else-if="scope.row.ResourceType ==4">RPC接口</span>
                  <span v-else-if="scope.row.ResourceType ==8">RPC节点</span>
                </template>
              </el-table-column>
              <el-table-column
                prop="Remark"
                label="备注名"
                min-width="150"
              >
                <template slot-scope="scope">
                  {{ scope.row.Remark }}
                  <el-button style="margin-left:10px" plain size="mini" icon="el-icon-edit" @click="setRemark(scope.row)" />
                </template>
              </el-table-column>
              <el-table-column
                prop="SystemType"
                label="来源节点类别"
                sortable="custom"
                min-width="120"
              >
                <template slot-scope="scope">
                  {{ scope.row.SystemTypeName }}
                </template>
              </el-table-column>
              <el-table-column prop="AddTime" label="添加时间" sortable="custom" min-width="150">
                <template slot-scope="scope">
                  <span>{{
                    moment(scope.row.AddTime).format("YYYY-MM-DD HH:mm")
                  }}</span>
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
                      @click="showItem(suScope.row)"
                    >资源列表</el-button>
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
    <el-dialog
      title="设置资源模块备注名"
      :visible="visible"
      width="300px"
      :before-close="()=>visible=false"
    >
      <el-input v-model="remark" max-length="50" placeholder="备注名" />
      <div slot="footer" style="text-align:center">
        <el-button type="primary" @click="saveRemark">保存</el-button>
        <el-button type="default" @click="()=>visible=false">取消</el-button>
      </div>
    </el-dialog>
    <resourceItem :visible="visibleItem" :modular-id="modularId" @cancel="()=>visibleItem = false" />
  </div>
</template>
<script>
import moment from 'moment'
import * as serverBindApi from '@/api/rpcMer/serverBind'
import * as ResourceApi from '@/api/module/resource'
import resourceItem from '../../module/components/resourceItem'
export default {
  components: {
    resourceItem
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
      visibleItem: false,
      remark: null,
      modularId: null,
      choose: null,
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
        QueryKey: null,
        RpcMerId: null,
        SystemType: null,
        ResourceType: null
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
    showItem(row) {
      this.modularId = row.Id
      this.visibleItem = true
    },
    setRemark(row) {
      this.choose = row
      this.remark = row.Remark
      this.visible = true
    },
    async saveRemark() {
      if (this.choose.Remark === this.remark) {
        this.visible = false
        return
      }
      await ResourceApi.SetRemark(this.choose.Id, this.remark)
      this.$message({
        message: '保存成功！',
        type: 'success'
      })
      this.visible = false
      this.loadTable()
    },
    reset() {
      this.pagination.page = 1
      this.queryParam.RpcMerId = this.rpcMerId
      this.queryParam.SystemType = null
      this.queryParam.ResourceType = null
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
    },
    async loadTable() {
      this.formatQueryParam()
      const res = await ResourceApi.Query(this.queryParam, this.pagination)
      this.resourceList = res.List
      this.pagination.total = res.Count
    },
    handleNodeClick(data) {
      this.queryParam.SystemType = data.TypeVal
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
