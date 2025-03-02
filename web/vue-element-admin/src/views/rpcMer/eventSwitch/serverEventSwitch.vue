<template>
  <div>
    <el-form :inline="true">
      <el-form-item label="事件节点">
        <el-select v-model="queryParam.ServerId" placeholder="事件节点">
          <el-option v-for="(item) in server" :key="item.ServerId" :value="item.ServerId" :label="item.ServerName" />
        </el-select>
      </el-form-item>
      <el-form-item label="事件源">
        <el-select v-model="queryParam.SysEventId" placeholder="事件源">
          <el-option v-for="item in sysEventList" :key="item.Id" :label="item.EventName" :value="item.Id" />
        </el-select>
      </el-form-item>
      <el-form-item label="事件级别">
        <el-select v-model="queryParam.EventLevel" placeholder="事件级别">
          <el-option v-for="item in SysEventLevel" :key="item.value" :label="item.text" :value="item.value" />
        </el-select>
      </el-form-item>
      <el-form-item label="事件类型">
        <el-select v-model="queryParam.EventType" placeholder="事件类型">
          <el-option v-for="item in SysEventType" :key="item.value" :label="item.text" :value="item.value" />
        </el-select>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="search">查询</el-button>
        <el-button type="default" @click="reset">重置</el-button>
        <el-button type="success" @click="add">新增事件</el-button>
      </el-form-item>
    </el-form>
    <el-table
      :data="datasource"
      style="width: 100%"
    >
      <el-table-column
        type="index"
        fixed="left"
        :index="indexMethod"
      />
      <el-table-column
        prop="ServerName"
        label="事件节点"
        width="200"
      />
      <el-table-column
        prop="SysEventName"
        label="事件名"
        width="200"
      />
      <el-table-column
        prop="Module"
        label="模块名"
        width="200"
      />
      <el-table-column
        prop="EventLevel"
        label="级别"
        width="150"
      >
        <template slot-scope="scope">
          {{ SysEventLevel[scope.row.EventLevel].text }}
        </template>
      </el-table-column>
      <el-table-column
        prop="EventType"
        label="事件类别"
        width="150"
      >
        <template slot-scope="scope">
          {{ SysEventType[scope.row.EventType].text }}
        </template>
      </el-table-column>
      <el-table-column
        prop="IsEnable"
        label="是否启用"
        width="120"
      >
        <template slot-scope="scope">
          <el-switch v-model="scope.row.IsEnable" @change="enableChange(scope.row)" />
        </template>
      </el-table-column>
      <el-table-column
        prop="MsgTemplate"
        label="通知消息摸版"
        min-width="200"
      />
      <el-table-column
        prop="Action"
        fixed="right"
        label="操作"
        min-width="150"
      >
        <template slot-scope="scope">
          <el-option-group>
            <el-button size="small" plain @click="showDetailed(scope.row)">详细</el-button>
            <el-button v-if="!scope.row.IsEnable" type="primary" size="small" plain @click="showEditEvent(scope.row)">编辑</el-button>
            <el-button v-if="!scope.row.IsEnable" type="danger" size="small" plain @click="deleteEvent(scope.row)">删除</el-button>
          </el-option-group>
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
    <addEventSwtich :visible="showAdd" :rpc-mer-id="rpcMerId" @cancel="closeAdd" />
    <editEventSwtich :id="id" :rpc-mer-id="rpcMerId" :visible="showEdit" @cancel="closeEdit" />
    <eventSwtichInfo :id="id" :visible="visible" @cancel="()=>visible=false" />
  </div>
</template>
<script>
import moment from 'moment'
import { GetItems } from '@/api/rpcMer/serverBind'
import * as sysEventApi from '@/api/server/sysEvent'
import * as eventSwitchApi from '@/api/server/eventSwitch'
import { SysEventType, SysEventLevel } from '@/config/publicDic'
import addEventSwtich from './addEventSwtich'
import editEventSwtich from './editEventSwtich'
import eventSwtichInfo from './eventSwtichInfo'
export default {
  components: {
    addEventSwtich,
    eventSwtichInfo,
    editEventSwtich
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
      SysEventLevel,
      SysEventType,
      showAdd: false,
      showEdit: false,
      visible: false,
      id: null,
      sysEventList: [],
      server: [],
      isInit: false,
      oldRpcMerId: null,
      queryParam: {
        ServerId: null,
        RpcMerId: null,
        SysEventId: null,
        EventLevel: null,
        EventType: null
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
        if (val && this.rpcMerId && this.rpcMerId !== this.oldRpcMerId) {
          this.queryParam.RpcMerId = this.rpcMerId
          this.loadServer()
          this.load()
        }
      },
      immediate: true
    },
    rpcMerId: {
      handler(val) {
        if (val && val !== this.oldRpcMerId && this.isLoad) {
          this.queryParam.RpcMerId = this.rpcMerId
          this.loadServer()
          this.load()
        }
      },
      immediate: true
    }
  },
  mounted() {
    this.loadItem()
  },
  methods: {
    moment,
    async loadServer() {
      this.server = await GetItems({
        RpcMerId: this.rpcMerId,
        IsHold: true,
        ServiceState: [0, 1]
      })
    },
    showDetailed(row) {
      this.id = row.Id
      this.visible = true
    },
    async enableChange(row) {
      await eventSwitchApi.SetIsEnable(row.Id, row.IsEnable)
    },
    showEditEvent(row) {
      this.id = row.Id
      this.showEdit = true
    },
    deleteEvent(row) {
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
      await eventSwitchApi.Delete(row.Id)
      this.load()
      this.$message({
        message: '删除成功！',
        type: 'success'
      })
    },
    async loadItem() {
      this.sysEventList = await sysEventApi.GetItems()
    },
    add() {
      this.showAdd = true
    },
    closeAdd(isRefresh) {
      this.showAdd = false
      if (isRefresh) {
        this.load()
      }
    },
    closeEdit(isRefresh) {
      this.showEdit = false
      if (isRefresh) {
        this.load()
      }
    },
    pagingChange(index) {
      this.pagination.page = index
      this.load()
    },
    search() {
      this.load()
    },
    reset() {
      this.queryParam.ServerId = null
      this.queryParam.SysEventId = null
      this.queryParam.EventLevel = null
      this.queryParam.EventType = null
      this.load()
    },
    async load() {
      const res = await eventSwitchApi.Query(this.queryParam, this.pagination)
      this.datasource = res.List
      this.pagination.total = res.Count
    },
    indexMethod(index) {
      return (index + 1) + ((this.pagination.page - 1) * this.pagination.size)
    }
  }
}
</script>

