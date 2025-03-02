<template>
  <el-card>
    <span slot="header"> 发布方案 </span>
    <el-form :inline="true" :model="queryParam">
      <el-form-item label="方案名">
        <el-input v-model="queryParam.VerTitle" placeholder="方案名" />
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
    <el-table :data="schemeList" style="width: 100%" @sort-change="sortChange">
      <el-table-column type="index" fixed="left" :index="indexMethod" />
      <el-table-column prop="SchemeName" label="方案名" min-width="150" />
      <el-table-column prop="SchemeShow" label="方案说明" min-width="200" />
      <el-table-column prop="Status" label="状态" sortable="custom" width="120">
        <template slot-scope="scope">
          <el-switch :value="scope.row.Status == 'aa'" @change="(val) => setIsEnable(val, scope.row)" />
        </template>
      </el-table-column>
      <el-table-column prop="AddTime" label="添加时间" sortable="custom" min-width="150">
        <template slot-scope="scope">
          <span>{{
            moment(scope.row.AddTime).format("YYYY-MM-DD HH:mm")
          }}</span>
        </template>
      </el-table-column>
      <el-table-column prop="Action" fixed="right" label="操作" width="300">
        <template slot-scope="scope">
          <el-option-group>
            <el-button type="default" size="mini" @click="show(scope.row)">详细</el-button>
            <el-button v-if="!scope.row.IsEnable" size="mini" type="primary" @click="edit(scope.row)">编辑</el-button>
            <el-button
              v-if="!scope.row.IsEnable"
              size="mini"
              type="danger"
              @click="deleteScheme(scope.row)"
            >删除</el-button>
          </el-option-group>
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
    <addScheme :visible="visibleAdd" :rpc-mer-id="rpcMerId" @cancel="closeAdd" />
    <editScheme :id="id" :visible="visibleEdit" :rpc-mer-id="rpcMerId" @cancel="closeEdit" />
    <schemeView :id="id" :visible="visibleView" :rpc-mer-id="rpcMerId" @cancel="visibleView=false" />
  </el-card>
</template>
<script>
import moment from 'moment'
import * as schemeApi from '@/api/module/publicScheme'
import addScheme from './addScheme'
import editScheme from './editScheme'
import schemeView from './schemeView.vue'
export default {
  components: {
    addScheme,
    editScheme,
    schemeView
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
      schemeList: [],
      oldRpcMerId: null,
      visibleAdd: false,
      visibleEdit: false,
      visibleView: false,
      id: null,
      pagination: {
        size: 20,
        page: 1,
        total: 0,
        sort: null,
        order: null
      },
      queryParam: {
        RpcMerId: null,
        VerTitle: null,
        IsEnable: null
      }
    }
  },
  watch: {
    isLoad: {
      handler(val) {
        if (val) {
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
    closeAdd(isRefresh) {
      this.visibleAdd = false
      if (isRefresh) {
        this.loadTable()
      }
    },
    add() {
      this.visibleAdd = true
    },
    show(row) {
      this.id = row.Id
      this.visibleView = true
    },
    closeEdit(isRefresh) {
      this.visibleEdit = false
      if (isRefresh) {
        this.loadTable()
      }
    },
    edit(row) {
      this.id = row.Id
      this.visibleEdit = true
    },
    setIsEnable(isEnable, row) {
      schemeApi.SetIsEnable(row.Id, isEnable).then(() => {
        row.Status = isEnable ? 'aa' : 'nn'
      })
    },
    deleteScheme(row) {
      const that = this
      this.$confirm('确定删除?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        that.delete(row.Id)
      }).catch(() => {
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
      this.queryParam.VerTitle = null
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
    }
  }
}
</script>
<style lang="scss" scoped>
.el-button-group {
  .el-button {
    margin-left: 5px;
  }
}
</style>
