<template>
  <div>
    <el-form :inline="true">
      <el-form-item label="指令名">
        <el-input v-model="queryParam.Dictate" placeholder="指令名" />
      </el-form-item>
      <el-form-item label="限流类型">
        <el-select v-model="queryParam.LimitType" placeholder="限流类型">
          <el-option v-for="item in ServerLimitType" :key="item.value" :label="item.text" :value="item.value" />
        </el-select>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="search">查询</el-button>
        <el-button type="default" @click="reset">重置</el-button>
        <el-button type="success" @click="add">新增限流</el-button>
      </el-form-item>
    </el-form>
    <el-table
      :data="datasource"
      style="width: 100%"
    >
      <el-table-column
        type="index"
        fixed="left"
        width="50"
        :index="indexMethod"
      />
      <el-table-column
        prop="Dictate"
        label="指令"
        min-width="150"
      />
      <el-table-column
        prop="LimitType"
        label="限制类型"
        min-width="150"
      >
        <template slot-scope="scope">
          <span :style="{ color: ServerLimitType[scope.row.LimitType].color }">{{ ServerLimitType[scope.row.LimitType].text }}</span>
        </template>
      </el-table-column>
      <el-table-column
        prop="LimitNum"
        label="最大流量"
        min-width="80"
      />
      <el-table-column
        prop="LimitTime"
        label="窗口大小(秒)"
        min-width="80"
      />
      <el-table-column
        prop="BucketSize"
        label="桶大小"
        min-width="120"
      />
      <el-table-column
        prop="BucketOutNum"
        label="桶溢出速度"
        min-width="120"
      />
      <el-table-column
        prop="TokenNum"
        label="令牌最大数"
        min-width="120"
      />
      <el-table-column
        prop="TokenInNum"
        label="每秒添加令牌数"
        min-width="120"
      />
      <el-table-column
        prop="Action"
        fixed="right"
        label="操作"
        width="260"
      >
        <template slot-scope="scope">
          <el-button-group>
            <el-button size="small" plain @click="editLimit(scope.row)">编辑</el-button>
            <el-button size="small" type="danger" plain @click="deleteLimit(scope.row)">删除</el-button>
          </el-button-group>
        </template>
      </el-table-column>
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
    <dictateLimitAdd :visible="showAdd" :server-id="serverId" @cancel="closeAdd" />
    <dictateLimitEdit :id="id" :visible="showEdit" @cancel="closeEdit" />
  </div>
</template>
<script>
import moment from 'moment'
import * as dictateLimitApi from '@/api/server/dictateLimit'
import { ServerLimitType } from '@/config/publicDic'
import dictateLimitAdd from './dictateLimitAdd'
import dictateLimitEdit from './dictateLimitEdit'
export default {
  components: {
    dictateLimitAdd,
    dictateLimitEdit
  },
  props: {
    serverId: {
      type: String,
      default: 0
    },
    isLoad: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      showAdd: false,
      showEdit: false,
      id: null,
      ServerLimitType,
      isInit: false,
      oldServerId: 0,
      queryParam: {
        Dictate: null,
        LimitType: null
      },
      pagination: {
        size: 20,
        page: 1,
        total: 0,
        sort: null,
        order: null
      },
      datasource: []
    }
  },
  watch: {
    isLoad: {
      handler(val) {
        if (val && this.serverId && this.serverId !== this.oldServerId) {
          this.load()
          this.beginRefresh()
        }
      },
      immediate: true
    },
    serverId: {
      handler(val) {
        if (val && val !== this.oldServerId && this.isLoad) {
          this.load()
        }
      },
      immediate: true
    }
  },
  methods: {
    moment,
    closeAdd() {
      this.showAdd = false
      this.reset()
    },
    closeEdit() {
      this.showEdit = false
      this.load()
    },
    editLimit(row) {
      this.id = row.Id
      this.showEdit = true
    },
    deleteLimit(row) {
      const that = this
      this.$confirm('确定删除(' + row.Dictate + ')指令的限流配置?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        that.delete(row)
      }).catch(() => {
      })
    },
    async delete(row) {
      await dictateLimitApi.Delete(row.Id)
      this.$message({
        message: '删除成功！',
        type: 'success'
      })
      this.load()
    },
    add() {
      this.showAdd = true
    },
    checkChange(value) {
      this.isAutoRefresh = value
    },
    pagingChange(index) {
      this.pagination.page = index
      this.load()
    },
    search() {
      this.load()
    },
    reset() {
      this.queryParam.Dictate = null
      this.queryParam.LimitType = null
      this.load()
    },
    async load() {
      this.queryParam.ServerId = this.serverId
      const res = await dictateLimitApi.Query(this.queryParam, this.pagination)
      this.datasource = res.List
      this.pagination.total = res.Count
    },
    indexMethod(index) {
      return (index + 1) + ((this.pagination.page - 1) * this.pagination.size)
    },
    beginRefresh() {
      const that = this
      if (this.timer) {
        window.clearTimeout(this.timer)
      }
      this.timer = window.setTimeout(function() {
        if (!that.isLoad) {
          return
        } else if (that.isAutoRefresh) {
          that.load()
        }
        that.beginRefresh()
      }, parseInt(this.interval))
    }
  }
}
</script>

