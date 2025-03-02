<template>
  <el-card>
    <span slot="header"> 事件日志 </span>
    <el-form :inline="true" :model="queryParam">
      <template v-if="queryParam.ServerId == null">
        <el-form-item label="事件节点">
          <el-select v-model="queryParam.ServerId" placeholder="事件节点">
            <el-option v-for="(item) in server" :key="item.ServerId" :value="item.ServerId" :label="item.ServerName" />
          </el-select>
        </el-form-item>
        <el-form-item label="来源机房">
          <el-select
            v-model="queryParam.RegionId"
            clearable
            placeholder="来源机房"
          >
            <el-option
              v-for="item in region"
              :key="item.Id"
              :label="item.RegionName"
              :value="item.Id"
            />
          </el-select>
        </el-form-item>
      </template>
      <el-form-item label="事件级别">
        <el-select v-model="queryParam.EventLevel" placeholder="事件级别">
          <el-option
            v-for="item in SysEventLevel"
            :key="item.value"
            :label="item.text"
            :value="item.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="事件类别">
        <el-select v-model="queryParam.EventType" placeholder="事件类型">
          <el-option
            v-for="item in SysEventType"
            :key="item.value"
            :label="item.text"
            :value="item.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="search">查询</el-button>
        <el-button type="default" @click="reset">重置</el-button>
      </el-form-item>
    </el-form>
    <el-table :data="logList" style="width: 100%" @sort-change="sortChange">
      <el-table-column type="index" fixed="left" :index="indexMethod" />
      <el-table-column prop="EventName" label="事件名" min-width="150" />
      <el-table-column prop="ServerName" label="来源节点" min-width="200">
        <template slot-scope="scope">
          <el-link @click="showDetailed(scope.row)">{{
            scope.row.ServerName
          }}</el-link>
        </template>
      </el-table-column>
      <el-table-column prop="Region" label="来源机房" width="150" />
      <el-table-column prop="EventLevel" label="事件级别" width="120">
        <template slot-scope="scope">
          <el-tag :type="SysEventLevel[scope.row.EventLevel].type">{{ SysEventLevel[scope.row.EventLevel].text }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="EventType" label="事件类型" width="120">
        <template slot-scope="scope">
          {{ SysEventType[scope.row.EventType].text }}
        </template>
      </el-table-column>
      <el-table-column prop="EventShow" label="事件说明" min-width="250" />
      <el-table-column
        prop="AddTime"
        label="事件时间"
        sortable="custom"
        min-width="150"
      >
        <template slot-scope="scope">
          {{ moment(scope.row.AddTime).format("YYYY-MM-DD HH:mm:ss") }}
        </template>
      </el-table-column>
      <el-table-column
        prop="Action"
        label="操作"
        fixed="right"
        min-width="100"
      >
        <template slot-scope="scope">
          <el-button size="small" plain @click="showEventLog(scope.row)">详细</el-button>
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
    <eventLogView :id="id" :visible="visible" @cancel="()=>visible=false" />
  </el-card>
</template>
<script>
import moment from 'moment'
import { SysEventType, SysEventLevel } from '@/config/publicDic'
import { GetList } from '@/api/basic/region'
import { GetItems } from '@/api/rpcMer/serverBind'
import { Query } from '@/api/module/eventLog'
import eventLogView from './eventLogView'
export default {
  components: {
    eventLogView
  },
  props: {
    rpcMerId: {
      type: String,
      default: null
    },
    systemTypeId: {
      type: String,
      default: null
    }
  },
  data() {
    return {
      SysEventType,
      SysEventLevel,
      visible: false,
      id: null,
      region: [],
      logList: [],
      server: [],
      pagination: {
        size: 20,
        page: 1,
        total: 0,
        sort: null,
        order: null
      },
      queryParam: {
        RegionId: null,
        RpcMerId: null,
        SystemTypeId: null,
        EventLevel: null,
        EventType: null,
        ServerId: null
      }
    }
  },
  watch: {
    rpcMerId: {
      handler(val) {
        if (val && val !== 0) {
          this.queryParam.RpcMerId = val
          this.loadServer()
          this.loadTable()
        }
      },
      immediate: true
    },
    systemTypeId: {
      handler(val) {
        this.queryParam.SystemTypeId = val
        if (this.rpcMerId != null && this.rpcMerId !== 0) {
          this.loadTable()
        }
      },
      immediate: false
    }
  },
  mounted() {
  },
  methods: {
    moment,
    showEventLog(row) {
      this.id = row.Id
      this.visible = true
    },
    showDetailed(row) {
      this.$router.push({
        name: 'serverDetailed',
        params: {
          routeTitle: row.ServerName + '-节点详情',
          Id: row.ServerId
        }
      })
    },
    async loadServer() {
      this.server = await GetItems({
        RpcMerId: this.rpcMerId,
        IsHold: true,
        ServiceState: [0, 1]
      })
    },
    async loadRegion() {
      if (this.region.length === 0) {
        this.region = await GetList()
      }
    },
    reset() {
      this.pagination.page = 1
      this.queryParam.RegionId = null
      this.queryParam.SystemTypeId = this.systemTypeId
      this.queryParam.EventLevel = null
      this.queryParam.EventType = null
      this.queryParam.RpcMerId = this.rpcMerId
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
      const res = await Query(this.queryParam, this.pagination)
      this.logList = res.List
      this.pagination.total = res.Count
    }
  }
}
</script>
