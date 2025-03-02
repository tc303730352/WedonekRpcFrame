<template>
  <el-dialog
    title="配置服务节点权重"
    :visible="visible"
    width="65%"
    :before-close="handleClose"
  >
    <el-form :inline="true" :model="queryParam">
      <el-form-item label="关键字">
        <el-input v-model="queryParam.QueryKey" placeholder="名称/IP/MAC/版本号" />
      </el-form-item>
      <el-form-item label="服务状态">
        <el-select v-model="queryParam.ServiceState" multiple placeholder="服务状态">
          <el-option v-for="item in ServerState" :key="item.value" :label="item.text" :value="item.value" />
        </el-select>
      </el-form-item>
      <el-form-item label="在线状态">
        <el-select v-model="queryParam.IsOnline" clearable placeholder="在线状态">
          <el-option label="在线" value="true" />
          <el-option label="离线" value="false" />
        </el-select>
      </el-form-item>
      <el-form-item label="承载方式">
        <el-select v-model="queryParam.IsContainer" clearable placeholder="承载方式">
          <el-option label="物理机" value="false" />
          <el-option label="容器" value="true" />
        </el-select>
      </el-form-item>
      <el-form-item label="所在机房">
        <el-select v-model="queryParam.RegionId" clearable placeholder="所在机房">
          <el-option v-for="item in region" :key="item.Id" :label="item.RegionName" :value="item.Id" />
        </el-select>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="search">查询</el-button>
        <el-button type="default" @click="reset">重置</el-button>
      </el-form-item>
    </el-form>
    <div class="chiose-server">
      <span class="title"> 已修改的节点数: {{ weightNum }}</span>
      <el-button type="danger" size="mini" @click="clear">清除</el-button>
    </div>
    <el-card>
      <el-table
        ref="servers"
        :data="serverList"
        row-key="Id"
        style="width: 100%"
        @sort-change="sortChange"
      >
        <el-table-column type="index" fixed="left" :index="indexMethod" width="80" />
        <el-table-column prop="ServerName" label="服务名" fixed="left" width="250" sortable="custom" />
        <el-table-column prop="ServerIp" label="服务IP" width="120">
          <template slot-scope="scope">
            <span>{{ scope.row.ServerIp + ':' +scope.row.ServerPort }}</span>
          </template>
        </el-table-column>
        <el-table-column prop="ServerMac" label="MAC" sortable="custom" width="150" />
        <el-table-column prop="SystemType" label="服务类型" width="200">
          <template slot-scope="scope">
            <span>{{ scope.row.ServerGroup + '-' +scope.row.SystemType }}</span>
          </template>
        </el-table-column>
        <el-table-column prop="Region" label="所在机房" sortable="custom" width="100" />
        <el-table-column
          prop="IsOnline"
          label="是否在线"
          sortable="custom"
          width="100"
        >
          <template slot-scope="scope">
            <el-tag v-if="scope.row.IsOnline" type="success">在线</el-tag>
            <el-tag v-else type="danger">离线</el-tag>
          </template>
        </el-table-column>
        <el-table-column
          prop="IsContainer"
          label="承载方式"
          sortable="custom"
          width="100"
        >
          <template slot-scope="scope">
            <span v-if="scope.row.IsContainer">容器</span>
            <span v-else>物理机</span>
          </template>
        </el-table-column>
        <el-table-column
          prop="ServiceState"
          label="状态"
          sortable="custom"
          width="80"
        >
          <template slot-scope="scope">
            <span :style="{ color: ServerState[scope.row.ServiceState].color }">{{ ServerState[scope.row.ServiceState].text }}</span>
          </template>
        </el-table-column>
        <el-table-column prop="Weight" label="当前权重" width="120" sortable="custom" />
        <el-table-column prop="NewWeight" label="新的权重">
          <template slot-scope="scope">
            <el-input-number v-model="scope.row.NewWeight" :min="1" @change="(value)=>setWeight(scope.row, value)" />
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
    </el-card>
    <el-row slot="footer" style="text-align: center;">
      <el-button type="primary" :loading="isLoad" @click="save()">保存</el-button>
      <el-button type="default" @click="handleClose">关闭</el-button>
    </el-row>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import * as serverBindApi from '@/api/rpcMer/serverBind'
import { GetList } from '@/api/basic/region'
import { ServerState } from '@/config/publicDic'
export default {
  props: {
    rpcMerId: {
      type: String,
      default: null
    },
    systemTypeId: {
      type: String,
      default: null
    },
    visible: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      ServerState,
      isLoad: false,
      pagination: {
        size: 20,
        page: 1,
        totoal: 0,
        sort: null,
        order: null
      },
      changeWeight: {},
      weightNum: 0,
      region: [],
      serverList: [],
      queryParam: {
        QueryKey: null,
        GroupId: null,
        SystemTypeId: null,
        ServiceState: null,
        RegionId: null,
        IsOnline: null,
        IsContainer: null
      }
    }
  },
  watch: {
    visible: {
      handler(val) {
        if (val) {
          this.loadRegion()
          this.clear()
        }
      },
      immediate: true
    }
  },
  mounted() {
  },
  methods: {
    moment,
    handleClose() {
      this.$emit('cancel', false)
    },
    async loadRegion() {
      if (this.region.length === 0) {
        this.region = await GetList()
      }
    },
    setWeight(row, value) {
      if (this.changeWeight[row.BindId] == null) {
        this.weightNum = this.weightNum + 1
      }
      this.changeWeight[row.BindId] = value
    },
    indexMethod(index) {
      return (index + 1) + ((this.pagination.page - 1) * this.pagination.size)
    },
    clear() {
      this.changeWeight = {}
      this.weightNum = 0
      this.reset()
    },
    reset() {
      this.pagination.page = 1
      this.queryParam.GroupId = null
      this.queryParam.ServiceState = []
      this.queryParam.RegionId = null
      this.queryParam.IsContainer = null
      this.queryParam.IsOnline = null
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
    formatRow(items) {
      if (items) {
        items.forEach(e => {
          const weight = this.changeWeight[e.BindId]
          if (weight) {
            e.NewWeight = weight
          } else {
            e.NewWeight = e.Weight
          }
        })
      }
    },
    async loadTable() {
      this.queryParam.SystemTypeId = this.systemTypeId
      this.isLoad = true
      const res = await serverBindApi.Query(this.rpcMerId, this.queryParam, this.pagination)
      this.formatRow(res.List)
      this.serverList = res.List
      this.pagination.total = res.Count
      this.isLoad = false
    },
    async save() {
      if (this.weightNum === 0) {
        this.$message({
          message: '无数据可保存！',
          type: 'warning'
        })
        return
      }
      this.isLoad = true
      await serverBindApi.SaveWeight({
        RpcMerId: this.rpcMerId,
        SystemType: this.systemTypeId,
        Weight: this.changeWeight
      })
      this.$message({
        message: '保存成功！',
        type: 'success'
      })
      this.isLoad = false
      this.$emit('cancel', true)
    }
  }
}
</script>

  <style lang="scss" scoped>
  .chiose-server {
    line-height: 50px;
    height: 50px;
    width: 100%;
    display: inline-block;
    .title {
      padding-right: 15px;
    }
    .el-tag {
      margin-right: 10px;
    }
  }
  </style>
