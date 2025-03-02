<template>
  <div>
    <el-card>
      <span slot="header"> 服务集群 </span>
      <el-form :inline="true" :model="queryParam">
        <el-form-item label="关键字">
          <el-input v-model="queryParam.QueryKey" placeholder="关键字" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="search">查询</el-button>
          <el-button type="default" @click="reset">重置</el-button>
          <el-button type="success" @click="add">新增集群</el-button>
        </el-form-item>
      </el-form>
      <el-card>
        <el-table :data="rpcMer" style="width: 100%" @sort-change="sortChange">
          <el-table-column type="index" fixed="left" :index="indexMethod" width="80" />
          <el-table-column prop="SystemName" label="集群名" fixed="left" sortable="custom" />
          <el-table-column prop="AppId" label="应用AppId" sortable="custom" />
          <el-table-column prop="AppSecret" label="应用密钥" sortable="custom" />
          <el-table-column prop="ServerNum" label="服务节点数" width="120" />
          <el-table-column prop="Action" fixed="right" label="操作" width="260">
            <template slot-scope="scope">
              <el-button-group>
                <el-button size="small" plain @click="showDetailed(scope.row)">详细</el-button>
                <template v-if="scope.row.ServerNum == 0">
                  <el-button size="small" plain @click="editRpcMer(scope.row)">编辑</el-button>
                  <el-button size="small" type="danger" plain @click="deleteRpcMer(scope.row)">删除</el-button>
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
    <addRpcMer :visible="visibleAdd" @cancel="closeAdd" />
    <editRpcMer :visible="visibleEdit" :rpc-mer-id="rpcMerId" @cancel="closeEdit" />
  </div>
</template>
<script>
import moment from 'moment'
import addRpcMer from './components/rpcMerAdd'
import editRpcMer from './components/rpcMerEdit'
import * as rpcMerApi from '@/api/rpcMer/rpcMer'
export default {
  components: {
    addRpcMer,
    editRpcMer
  },
  data() {
    return {
      visibleAdd: false,
      visibleEdit: false,
      rpcMerId: null,
      pagination: {
        size: 20,
        page: 1,
        totoal: 0,
        sort: null,
        order: null
      },
      rpcMer: [],
      queryParam: {
        QueryKey: null
      }
    }
  },
  mounted() {
    this.reset()
  },
  methods: {
    moment,
    closeAdd() {
      this.visibleAdd = false
      this.reset()
    },
    showDetailed(row) {
      this.$router.push({
        name: 'rpcMerDetailed',
        params: {
          routeTitle: row.SystemName + '-集群详情',
          Id: row.Id
        }
      })
    },
    closeEdit() {
      this.visibleEdit = false
      this.reset()
    },
    editRpcMer(row) {
      this.rpcMerId = row.Id
      this.visibleEdit = true
    },
    add() {
      this.visibleAdd = true
    },
    reset() {
      this.pagination.page = 1
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
    deleteRpcMer(row) {
      const that = this
      this.$confirm('确定删除(' + row.SystemName + ')服务集群?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        that.delete(row)
      }).catch(() => {
      })
    },
    async delete(row) {
      await rpcMerApi.Delete(row.Id)
      this.reset()
    },
    indexMethod(index) {
      return index + 1 + (this.pagination.page - 1) * this.pagination.size
    },
    async loadTable() {
      const res = await rpcMerApi.Query(this.queryParam.QueryKey, this.pagination)
      this.rpcMer = res.List
      this.pagination.total = res.Count
    }
  }
}
</script>
