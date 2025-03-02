<template>
  <div>
    <el-card>
      <span slot="header"> 错误码 </span>
      <el-form :inline="true" :model="queryParam">
        <el-form-item label="关键字">
          <el-input v-model="queryParam.QueryKey" placeholder="错误码/错误Code" />
        </el-form-item>
        <el-form-item label="是否完善">
          <el-select v-model="queryParam.IsPerfect" clearable placeholder="是否完善了信息">
            <el-option label="是" value="true" />
            <el-option label="否" value="false" />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="search">查询</el-button>
          <el-button type="default" @click="reset">重置</el-button>
          <el-button type="success" @click="addError">新增错误</el-button>
        </el-form-item>
      </el-form>
      <el-card>
        <el-table :data="errorList" style="width: 100%" @sort-change="sortChange">
          <el-table-column type="index" fixed="left" :index="indexMethod" width="80" />
          <el-table-column prop="Id" label="错误ID" fixed="left" width="150" />
          <el-table-column prop="ErrorCode" label="错误码" width="300" sortable="custom" />
          <el-table-column prop="Zh" label="中文" width="400">
            <template slot-scope="scope">
              <el-input v-model="scope.row.Zh" placeholder="中文错误提示!" maxlength="100" @change="(value)=>saveText(scope.row,value,'zh')" />
            </template>
          </el-table-column>
          <el-table-column prop="En" label="英文" width="400">
            <template slot-scope="scope">
              <el-input v-model="scope.row.En" placeholder="英文错误提示!" maxlength="100" @change="(value)=>saveText(scope.row,value,'en')" />
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
    <editError :visible="visible" @cancel="closeAdd" />
  </div>
</template>
<script>
import moment from 'moment'
import * as errorApi from '@/api/module/error'
import editError from './components/addError'
export default {
  components: {
    editError
  },
  data() {
    return {
      visible: false,
      pagination: {
        size: 20,
        page: 1,
        totoal: 0,
        sort: null,
        order: null
      },
      errorList: [],
      queryParam: {
        QueryKey: null,
        IsPerfect: null
      }
    }
  },
  mounted() {
    this.reset()
  },
  methods: {
    moment,
    closeAdd(isAdd) {
      this.visible = false
      if (isAdd) {
        this.loadTable()
      }
    },
    addError() {
      this.visible = true
    },
    reset() {
      this.pagination.page = 1
      this.queryParam.QueryKey = null
      this.queryParam.IsPerfect = null
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
    async saveText(row, msg, lang) {
      await errorApi.SetMsg({
        ErrorId: row.Id,
        Lang: lang,
        Msg: msg
      })
    },
    indexMethod(index) {
      return index + 1 + (this.pagination.page - 1) * this.pagination.size
    },
    async loadTable() {
      const res = await errorApi.Query(this.queryParam, this.pagination)
      this.errorList = res.List
      this.pagination.total = res.Count
    }
  }
}
</script>
