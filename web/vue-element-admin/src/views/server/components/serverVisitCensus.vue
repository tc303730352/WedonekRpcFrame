<template>
  <div>
    <el-form :inline="true">
      <el-form-item label="指令名">
        <el-input v-model="dictate" placeholder="指令名" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="search">查询</el-button>
        <el-button type="default" @click="reset">重置</el-button>
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
        prop="Show"
        label="说明"
        min-width="150"
      />
      <el-table-column
        prop="SuccessNum"
        label="成功量"
        min-width="80"
        :sortable="true"
      />
      <el-table-column
        prop="FailNum"
        label="失败量"
        min-width="80"
        :sortable="true"
      />
      <el-table-column
        prop="VisitNum"
        label="访问量"
        min-width="120"
        :sortable="true"
      />
      <el-table-column
        prop="TodayFail"
        label="今日失败量"
        min-width="120"
        :sortable="true"
      />
      <el-table-column
        prop="TodaySuccess"
        label="今日成功量"
        min-width="120"
        :sortable="true"
      />
      <el-table-column
        prop="TodayVisit"
        label="今日访问量"
        min-width="120"
        :sortable="true"
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
  </div>
</template>
<script>
import moment from 'moment'
import * as visitCensusApi from '@/api/server/visitCensus'
import { ServerState, UsableState } from '@/config/publicDic'
export default {
  props: {
    serverId: {
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
      UsableState,
      ServerState,
      isInit: false,
      dictate: null,
      oldServerId: 0,
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
  destroyed() {
    if (this.timer) {
      window.clearTimeout(this.timer)
    }
  },
  methods: {
    moment,
    checkChange(value) {
      this.isAutoRefresh = value
    },
    pagingChange(index) {
      this.pagination.page = index
      this.load()
    },
    search() {
      this.pagination.page = 1
      this.load()
    },
    reset() {
      this.dictate = null
      this.load()
    },
    async load() {
      const res = await visitCensusApi.Query({
        ServiceId: this.serverId,
        Dictate: this.dictate
      }, this.pagination)
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

