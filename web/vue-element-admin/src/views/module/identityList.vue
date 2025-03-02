<template>
  <div>
    <el-card>
      <span slot="header"> 租户详细 </span>
      <el-form :inline="true" :model="queryParam">
        <el-form-item label="租户名">
          <el-input v-model="queryParam.AppName" placeholder="租户名" />
        </el-form-item>
        <el-form-item label="有效期">
          <el-date-picker
            v-model="queryParam.Time"
            type="daterange"
            range-separator="至"
            start-placeholder="开始日期"
            end-placeholder="结束日期"
          />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="search">查询</el-button>
          <el-button type="default" @click="reset">重置</el-button>
          <el-button type="success" @click="add">新增</el-button>
        </el-form-item>
      </el-form>
      <el-card>
        <el-table :data="identitys" style="width: 100%" @sort-change="sortChange">
          <el-table-column type="index" fixed="left" :index="indexMethod" width="80" />
          <el-table-column prop="AppId" label="应用ID" width="300" />
          <el-table-column prop="AppName" label="租户名" width="300" />
          <el-table-column prop="AppShow" label="说明" min-width="150" />
          <el-table-column prop="IsEnable" label="是否启用" width="100">
            <template slot-scope="scope">
              <el-switch v-model="scope.row.IsEnable" @change="setIsEnable(scope.row)" />
            </template>
          </el-table-column>
          <el-table-column prop="EffectiveDate" label="有效期" width="150">
            <template slot-scope="scope">
              {{ moment(scope.row.EffectiveDate).format('YYYY-MM-DD') }}
            </template>
          </el-table-column>
          <el-table-column prop="CreateTime" label="创建时间" width="150">
            <template slot-scope="scope">
              {{ moment(scope.row.CreateTime).format('YYYY-MM-DD HH:mm') }}
            </template>
          </el-table-column>
          <el-table-column prop="Action" label="操作" width="300">
            <template slot-scope="scope">
              <el-option-group>
                <el-button type="default" size="mini" @click="show(scope.row)">详细</el-button>
                <el-button v-if="!scope.row.IsEnable" size="mini" type="primary" @click="edit(scope.row)">编辑</el-button>
                <el-button v-if="!scope.row.IsEnable" size="mini" type="danger" @click="deleteApp(scope.row)">删除</el-button>
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
      </el-card>
    </el-card>
    <addIdentity :visible="visible" @cancel="closeAdd" />
    <setIdentity :id="id" :visible="visibleSet" @cancel="closeSet" />
    <identityDetailed :id="id" :visible="visibleShow" @cancel="visibleShow=false" />
  </div>
</template>
<script>
import moment from 'moment'
import * as identityApi from '@/api/module/identity'
import addIdentity from './identity/addIdentity'
import setIdentity from './identity/setIdentity'
import identityDetailed from './identity/identityDetailed'
export default {
  components: {
    addIdentity,
    setIdentity,
    identityDetailed
  },
  data() {
    return {
      visible: false,
      visibleSet: false,
      visibleShow: false,
      pagination: {
        size: 20,
        page: 1,
        totoal: 0,
        sort: null,
        order: null
      },
      id: null,
      identitys: [],
      queryParam: {
        AppName: null,
        Time: null
      }
    }
  },
  mounted() {
    this.reset()
  },
  methods: {
    moment,
    show(row) {
      this.id = row.Id
      this.visibleShow = true
    },
    edit(row) {
      this.id = row.Id
      this.visibleSet = true
    },
    closeAdd(isRefresh) {
      this.visible = false
      if (isRefresh) {
        this.loadTable()
      }
    },
    closeSet(isRefresh) {
      this.visibleSet = false
      if (isRefresh) {
        this.loadTable()
      }
    },
    add() {
      this.visible = true
    },
    setIsEnable(row) {
      identityApi.SetIsEnable(row.Id, row.IsEnable).then(() => {

      }).catch(() => {
        row.IsEnable = !row.IsEnable
      })
    },
    reset() {
      this.pagination.page = 1
      this.queryParam.AppName = null
      this.queryParam.Time = null
      this.loadTable()
    },
    pagingChange(index) {
      this.pagination.page = index
      this.loadTable()
    },
    search() {
      this.loadTable()
    },
    deleteApp(row) {
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
    async delete(row) {
      await identityApi.Delete(row.Id)
      this.loadTable()
      this.$message({
        message: '删除成功！',
        type: 'success'
      })
    },
    sortChange(e) {
      this.pagination.order = e.order
      this.pagination.sort = e.prop
      this.loadTable()
    },
    indexMethod(index) {
      return index + 1 + (this.pagination.page - 1) * this.pagination.size
    },
    async loadTable() {
      const query = {
        AppName: this.queryParam.AppName
      }
      if (this.queryParam.Time != null && this.queryParam.Time.length === 2) {
        query.Begin = this.queryParam.Time[0]
        query.End = this.queryParam.Time[1]
      }
      const res = await identityApi.Query(query, this.pagination)
      this.identitys = res.List
      this.pagination.total = res.Count
    }
  }
}
</script>
