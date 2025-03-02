<template>
  <div>
    <el-row style="padding-right: 20px;text-align: right;">
      <el-checkbox :checked="isAutoRefresh" @change="checkChange">自动刷新</el-checkbox>
      <el-select v-model="interval" style="margin-left: 10px; width: 100px;" placeholder="刷新频率">
        <el-option label="1秒" value="1000" />
        <el-option label="2秒" value="2000" />
        <el-option label="5秒" value="5000" />
      </el-select>
    </el-row>
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
        prop="ServerName"
        label="链接服务名"
        min-width="250"
      />
      <el-table-column
        prop="IsContainer"
        label="承载方式"
        width="100"
      >
        <template slot-scope="scope">
          <span v-if="scope.row.IsContainer">容器</span>
          <span v-else>物理机</span>
        </template>
      </el-table-column>
      <el-table-column
        prop="ServiceState"
        label="服务状态"
        width="120"
      >
        <template slot-scope="scope">
          <span :style="{ color: ServerState[scope.row.ServiceState].color }">{{ ServerState[scope.row.ServiceState].text }}</span>
        </template>
      </el-table-column>
      <el-table-column
        prop="IsOnline"
        label="是否在线"
        width="100"
      >
        <template slot-scope="scope">
          <el-tag v-if="scope.row.IsOnline" type="success">在线</el-tag>
          <el-tag v-else type="danger">离线</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        prop="ConNum"
        label="链接数"
        min-width="80"
        :sortable="true"
      />
      <el-table-column
        prop="AvgTime"
        label="平均响应时间(毫秒)"
        min-width="120"
        :sortable="true"
      >
        <template slot-scope="scope">
          {{ scope.row.AvgTime / 1000 }}
        </template>
      </el-table-column>
      <el-table-column
        prop="SendNum"
        label="心跳包发送量"
        min-width="120"
        :sortable="true"
      />
      <el-table-column
        prop="ErrorNum"
        label="心跳包错误量"
        min-width="120"
        :sortable="true"
      />
      <el-table-column
        prop="UsableState"
        label="可用状态"
        min-width="120"
        :sortable="true"
      >
        <template slot-scope="scope">
          <span :style="{ color: UsableState[scope.row.UsableState].color }">{{ UsableState[scope.row.UsableState].text }}</span>
        </template>
      </el-table-column>
      <el-table-column
        prop="SyncTime"
        label="同步时间"
        min-width="150"
        :sortable="true"
      >
        <template slot-scope="scope">
          <span>{{ moment(scope.row.SyncTime).format('YYYY-MM-DD HH:mm') }}</span>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>
<script>
import moment from 'moment'
import * as signalStateApi from '@/api/server/signalState'
import { ServerState, UsableState } from '@/config/publicDic'
export default {
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
      UsableState,
      ServerState,
      isAutoRefresh: true,
      isInit: false,
      timer: null,
      interval: '2000',
      oldServerId: 0,
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
    async load() {
      this.datasource = await signalStateApi.Get(this.serverId)
    },
    indexMethod(index) {
      return index + 1
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

