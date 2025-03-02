<template>
  <div>
    <el-card>
      <span slot="header"> 广播错误日志 </span>
      <el-form :inline="true" :model="queryParam">
        <el-form-item label="关键字">
          <el-input v-model="queryParam.MsgKey" placeholder="消息Key" />
        </el-form-item>
        <el-form-item label="发生的时段">
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
        </el-form-item>
      </el-form>
      <el-card>
        <el-table :data="logs" style="width: 100%" @sort-change="sortChange">
          <el-table-column type="index" fixed="left" :index="indexMethod" width="80" />
          <el-table-column prop="MsgKey" label="调用方法" width="150" />
          <el-table-column prop="BroadcastType" label="广播类型" width="150">
            <template slot-scope="scope">
              {{ BroadcastType[scope.row.BroadcastType].text }}
            </template>
          </el-table-column>
          <el-table-column prop="RouteKey" label="队列路由Key" width="150" />
          <el-table-column
            prop="SourceServer"
            label="来源地址"
            min-width="200"
          >
            <template slot-scope="scope">
              <el-link @click="showDetailed(scope.row.SourceId, scope.row.SourceServer)">{{ scope.row.SourceServer }}</el-link>
            </template>
          </el-table-column>
          <el-table-column
            prop="ServerName"
            label="目的地节点"
            min-width="200"
          >
            <template slot-scope="scope">
              <el-link v-if="scope.row.ServerId !=0" @click="showDetailed(scope.row.ServerId, scope.row.ServerName)">{{ scope.row.ServerName }}</el-link>
            </template>
          </el-table-column>
          <el-table-column
            prop="SystemTypeName"
            label="目的地节点类型"
            min-width="120"
          />
          <el-table-column
            prop="RpcMer"
            label="目的地集群"
            min-width="120"
          />
          <el-table-column prop="Error" label="错误信息" width="150" />
          <el-table-column prop="AddTime" label="添加时间" width="150">
            <template slot-scope="scope">
              {{ moment(scope.row.AddTime).format('YYYY-MM-DD HH:mm') }}
            </template>
          </el-table-column>
          <el-table-column prop="Action" label="操作" width="150">
            <template slot-scope="scope">
              <el-button @click="showContent(scope.row)">详细</el-button>
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
    <el-dialog
      title="广播消息"
      :visible="visible"
      width="45%"
      :before-close="handleClose"
    >
      <el-card>
        <span slot="header">消息内容</span>
        <el-input :value="log.MsgBody" style="min-height: 100%;" type="textarea" />
      </el-card>
      <el-card>
        <span slot="header">消息来源</span>
        <el-form label-width="120px">
          <el-form-item label="来源节点">
            <el-input :value="log.MsgSource.ServerName" />
          </el-form-item>
          <el-form-item label="节点版本号">
            <el-input :value="log.MsgSource.VerNum" />
          </el-form-item>
          <el-form-item label="来源服务类型">
            <el-input :value="log.MsgSource.SystemTypeName" />
          </el-form-item>
          <el-form-item label="来源集群">
            <el-input :value="log.MsgSource.RpcMer" />
          </el-form-item>
          <el-form-item v-if="log.MsgSource.ContGroup" label="来源容器">
            <el-input :value="log.MsgSource.ContGroup" />
          </el-form-item>
          <el-form-item label="来源机房">
            <el-input :value="log.MsgSource.Region" />
          </el-form-item>
        </el-form>
      </el-card>
    </el-dialog>
  </div>
</template>
<script>
import moment from 'moment'
import * as broadcastLogApi from '@/api/module/broadcastLog'
import { BroadcastType } from '@/config/publicDic'
export default {
  components: {
  },
  data() {
    return {
      BroadcastType,
      visible: false,
      log: {
        MsgSource: {}
      },
      pagination: {
        size: 20,
        page: 1,
        totoal: 0,
        sort: null,
        order: null
      },
      logs: [],
      queryParam: {
        MsgKey: null,
        Time: null
      }
    }
  },
  mounted() {
    this.reset()
  },
  methods: {
    moment,
    handleClose() {
      this.visible = false
    },
    showDetailed(id, name) {
      this.$router.push({
        name: 'serverDetailed',
        params: {
          routeTitle: name + '-节点详情',
          Id: id
        }
      })
    },
    reset() {
      this.pagination.page = 1
      this.queryParam.MsgKey = null
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
    sortChange(e) {
      this.pagination.order = e.order
      this.pagination.sort = e.prop
      this.loadTable()
    },
    indexMethod(index) {
      return index + 1 + (this.pagination.page - 1) * this.pagination.size
    },
    async showContent(row) {
      const data = await broadcastLogApi.Get(row.Id)
      if (data.MsgBody && data.MsgBody !== '') {
        data.MsgBody = JSON.stringify(JSON.parse(data.MsgBody), null, 2)
      }
      this.log = data
      this.visible = true
    },
    async loadTable() {
      const query = {
        MsgKey: this.queryParam.MsgKey
      }
      if (this.queryParam.Time != null && this.queryParam.Time.length === 2) {
        query.Begin = this.queryParam.Time[0]
        query.End = this.queryParam.Time[1]
      }
      const res = await broadcastLogApi.Query(query, this.pagination, 'zh')
      this.logs = res.List
      this.pagination.total = res.Count
    }
  }
}
</script>
