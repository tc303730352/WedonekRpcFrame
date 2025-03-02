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
          <span slot="header"> 负载均衡方案 </span>
          <el-form :inline="true" :model="queryParam">
            <el-form-item label="方案名">
              <el-input
                v-model="queryParam.Scheme"
                placeholder="方案名"
              />
            </el-form-item>
            <el-form-item label="负载均衡方式">
              <el-select v-model="queryParam.TransmitType" clearable placeholder="负载均衡方式">
                <el-option label="ZoneIndex" :value="1" />
                <el-option label="HashCode" :value="2" />
                <el-option label="Number" :value="3" />
                <el-option label="FixedType" :value="4" />
              </el-select>
            </el-form-item>
            <el-form-item label="是否启用">
              <el-select v-model="queryParam.IsEnable" clearable placeholder="是否启用">
                <el-option label="是" :value="true" />
                <el-option label="否" :value="false" />
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
              :data="schemeList"
              style="width: 100%"
              @sort-change="sortChange"
            >
              <el-table-column type="index" fixed="left" :index="indexMethod" />
              <el-table-column
                prop="Scheme"
                label="方案名"
                min-width="150"
              />
              <el-table-column
                prop="SystemTypeId"
                label="应用节点类别"
                sortable="custom"
                min-width="150"
              >
                <template slot-scope="scope">
                  {{ scope.row.SystemType }}
                </template>
              </el-table-column>
              <el-table-column
                prop="VerNum"
                label="版本号"
                sortable="custom"
                min-width="150"
              />
              <el-table-column
                prop="TransmitType"
                label="负载方式"
                sortable="custom"
                min-width="80"
              >
                <template slot-scope="scope">
                  <span v-if="scope.row.TransmitType ==1">ZoneIndex</span>
                  <span v-else-if="scope.row.TransmitType ==2">HashCode</span>
                  <span v-else-if="scope.row.TransmitType ==3">Number</span>
                  <span v-else-if="scope.row.TransmitType ==4">FixedType</span>
                </template>
              </el-table-column>
              <el-table-column
                prop="IsEnable"
                sortable="custom"
                label="是否启用"
                width="110"
              >
                <template slot-scope="scope">
                  <el-switch v-model="scope.row.IsEnable" @change="setIsEnable(scope.row)" />
                </template>
              </el-table-column>
              <el-table-column
                prop="Show"
                label="备注"
                min-width="150"
              />
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
                width="300"
              >
                <template slot-scope="scope">
                  <el-button-group>
                    <el-button
                      size="small"
                      @click="showScheme(scope.row)"
                    >详细</el-button>
                    <template v-if="scope.row.IsEnable == false">
                      <el-button
                        size="small"
                        type="primary"
                        @click="editScheme(scope.row)"
                      >编辑</el-button>
                      <el-button
                        size="small"
                        type="danger"
                        @click="deleteScheme(scope.row)"
                      >删除</el-button>
                    </template>
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
  </div>
</template>
<script>
import moment from 'moment'
import * as serverBindApi from '@/api/rpcMer/serverBind'
import * as schemeApi from '@/api/transmit/scheme'
export default {
  components: {
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
      visibleTransmit: false,
      scheme: {
        Id: null,
        SystemTypeId: null,
        RpcMerId: null,
        Scheme: null,
        TransmitType: null
      },
      oldRpcMerId: null,
      groupType: [],
      schemeId: null,
      visibleSet: false,
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
      schemeList: [],
      queryParam: {
        RpcMerId: null,
        SystemTypeId: null,
        Scheme: null,
        TransmitType: null,
        IsEnable: null
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
    add() {
      this.$router.push({ name: 'addScheme', params: { rpcMerId: this.rpcMerId }})
    },
    transmitSet(row) {
      this.scheme = {
        Id: row.Id,
        SystemTypeId: row.SystemTypeId,
        RpcMerId: this.rpcMerId,
        Scheme: row.Scheme,
        TransmitType: row.TransmitType
      }
      this.visibleTransmit = true
    },
    showScheme(row) {
      this.$router.push({ name: 'schemeDetailed', params: { id: row.Id }})
    },
    editScheme(row) {
      this.$router.push({ name: 'schemeEdit', params: { id: row.Id }})
    },
    deleteScheme(row) {
      const that = this
      this.$confirm('确定删除?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        that.delete(row.Id)
      })
    },
    setIsEnable(row) {
      schemeApi.SetIsEnable(row.Id, row.IsEnable).then(() => {

      }).catch(() => {
        row.IsEnable = row.IsEnable === false
      })
    },
    async delete(id) {
      await schemeApi.Delete(id)
      this.$message({
        message: '删除成功！',
        type: 'success'
      })
      this.loadTable()
    },
    reset() {
      this.pagination.page = 1
      this.queryParam.RpcMerId = this.rpcMerId
      this.queryParam.SystemTypeId = null
      this.queryParam.TransmitType = null
      this.queryParam.Scheme = null
      this.queryParam.IsAuto = null
      this.queryParam.IsEnable = null
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
      const res = await schemeApi.Query(this.queryParam, this.pagination)
      this.schemeList = res.List
      this.pagination.total = res.Count
    },
    handleNodeClick(data) {
      this.queryParam.SystemTypeId = data.Id
      this.loadTable()
    },
    async loadTree() {
      if (this.groupType.length === 0 || this.rpcMerId !== this.oldRpcMerId) {
        this.oldRpcMerId = this.rpcMerId
        const res = await serverBindApi.GetGroupAndType(this.rpcMerId, null, null, true)
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
<style lang="scss" scoped>
.el-button-group{
 .el-button {
  margin-left: 5px;
 }
}
</style>
