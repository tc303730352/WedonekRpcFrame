<template>
  <div>
    <el-card>
      <el-table
        :data="serverList"
        :border="true"
        style="width: 100%"
        :fit="true"
        @sort-change="sortChange"
      >
        <el-table-column
          type="index"
          fixed="left"
          :index="indexMethod"
        />
        <el-table-column
          prop="ServerCode"
          label="服务编号"
          fixed="left"
          sortable="custom"
          width="120"
        />
        <el-table-column
          prop="ServerName"
          label="服务名"
          fixed="left"
          sortable="custom"
          width="250"
        />
        <el-table-column
          prop="ServerIp"
          label="内网IP"
          sortable="custom"
          min-width="130"
          resizable="true"
        >
          <template slot-scope="scope">
            <span>{{ scope.row.ServerIp + ':' + scope.row.ServerPort }}</span>
          </template>
        </el-table-column>
        <el-table-column
          prop="RemoteIp"
          label="远端IP"
          sortable="custom"
          resizable="true"
          min-width="130"
        >
          <template slot-scope="scope">
            <span>{{ scope.row.RemoteIp + ':' + scope.row.RemotePort }}</span>
          </template>
        </el-table-column>
        <el-table-column
          prop="ServiceType"
          label="服务类型"
          width="80"
        >
          <template slot-scope="scope">
            <span>{{ ServiceType[scope.row.ServiceType].text }}</span>
          </template>
        </el-table-column>
        <el-table-column
          prop="SystemTypeName"
          label="服务类目"
          sortable="custom"
          min-width="240"
        >
          <template slot-scope="scope">
            <span>{{ scope.row.GroupName + '-' + scope.row.SystemTypeName }}</span>
          </template>
        </el-table-column>
        <el-table-column
          prop="Region"
          label="所在机房"
          sortable="custom"
          width="120"
        />
        <el-table-column
          prop="HoldRpcMer"
          label="所属集群"
          sortable="custom"
          width="120"
        />
        <el-table-column
          v-if="queryParam.IsContainer"
          prop="Container"
          label="容器简称"
          width="100"
        >
          <template v-if="scope.row.IsContainer" slot-scope="scope">
            <el-tooltip effect="dark" :content="getContainerStr(scope.row.Container)" placement="top-start">
              <span>{{ scope.row.Container.Name }}</span>
            </el-tooltip>
          </template>
        </el-table-column>
        <el-table-column
          v-if="queryParam.IsContainer"
          prop="ContainerIp"
          label="容器内IP"
          width="130"
        />
        <el-table-column
          prop="IsOnline"
          label="是否在线"
          sortable="custom"
          width="110"
        >
          <template slot-scope="scope">
            <el-tag v-if="scope.row.IsOnline" type="success">在线</el-tag>
            <el-tag v-else type="danger">离线</el-tag>
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
        <el-table-column
          prop="ServerMac"
          label="MAC"
          sortable="custom"
          width="140"
        />
        <el-table-column
          prop="ApiVer"
          label="客户端版本号"
          sortable="custom"
          width="140"
        >
          <template slot-scope="scope">
            {{ formatVerNum(scope.row.ApiVer) }}
          </template>
        </el-table-column>
        <el-table-column
          prop="VerNum"
          label="应用版本号"
          sortable="custom"
          width="140"
        >
          <template slot-scope="scope">
            {{ formatVerNum(scope.row.VerNum) }}
          </template>
        </el-table-column>
        <el-table-column
          prop="ConIp"
          label="当前链接IP"
          width="100"
        />
        <el-table-column
          prop="LastOffliceDate"
          label="最后离线日期"
          width="120"
        >
          <template slot-scope="scope">
            <span>{{ moment(scope.row.LastOffliceDate).format('YYYY-MM-DD') }}</span>
          </template>
        </el-table-column>
        <el-table-column
          prop="AddTime"
          label="添加日期"
          width="120"
        >
          <template slot-scope="scope">
            <span>{{ moment(scope.row.AddTime).format('YYYY-MM-DD') }}</span>
          </template>
        </el-table-column>
        <el-table-column
          prop="Action"
          fixed="right"
          label="操作"
          min-width="300"
        >
          <template slot-scope="scope">
            <el-option-group>
              <el-button size="small" plain @click="showDetailed(scope.row)">详细</el-button>
              <el-button size="small" plain @click="setVerNum(scope.row)">设置版本号</el-button>
              <el-button v-if="!scope.row.IsOnline && scope.row.ServiceState != ServerState[0].value" type="primary" size="small" plain @click="enableServer(scope.row)">启用</el-button>
              <el-button v-if="!scope.row.IsOnline && scope.row.ServiceState != ServerState[3].value && scope.row.ServiceState != ServerState[1].value" type="warning" size="small" plain @click="stopServer(scope.row)">停用</el-button>
              <el-button v-if="scope.row.IsOnline && scope.row.ServiceState == ServerState[0].value" size="small" type="danger" plain @click="closeServer(scope.row)">下线</el-button>
              <el-button v-if="!scope.row.IsOnline && scope.row.ServiceState != ServerState[0].value" size="small" plain @click="editServer(scope.row)">编辑</el-button>
              <el-button v-if="!scope.row.IsOnline && (scope.row.ServiceState == ServerState[3].value || scope.row.ServiceState == ServerState[1].value)" size="small" type="danger" plain @click="deleteServer(scope.row)">删除</el-button>
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
    </el-card>
    <editServer :visible="visibleEdit" :server-id="serverId" @cancel="closeEdit" />
    <el-dialog
      :title="title"
      :visible="setVisible"
      :close-on-click-modal="false"
      width="500px"
      :before-close="()=>setVisible=false"
    >
      <el-option-group>
        <el-input-number
          v-model="verNum.One"
          style="width: 120px"
          :min="0"
        />
        <span>.</span>
        <el-input-number
          v-model="verNum.Two"
          style="width: 120px"
          :min="0"
          :max="99"
        />
        <span>.</span>
        <el-input-number
          v-model="verNum.Three"
          style="width: 120px"
          :min="0"
          :max="99"
        />
      </el-option-group>
      <el-row slot="footer" style="text-align:center;line-height:20px">
        <el-button type="primary" @click="saveVerNum">保存</el-button>
      </el-row>
    </el-dialog>
  </div>
</template>
<script>
import moment from 'moment'
import editServer from './serverEdit'
import * as serverApi from '@/api/server/server'
import { ServerState, ContainerType, ServiceType } from '@/config/publicDic'
export default {
  components: {
    editServer
  },
  props: {
    queryParam: {
      type: Object,
      required: true,
      default: () => new {
        QueryKey: null,
        ServerState: [],
        SystemTypeId: null,
        GroupId: null,
        RegionId: null,
        IsOnline: null,
        IsContainer: null,
        ContainerGroup: null,
        RpcMerId: null
      }()
    }
  },
  data() {
    return {
      ServiceType,
      ServerState,
      ContainerType,
      visibleEdit: false,
      setVisible: false,
      verNum: {},
      serverId: null,
      title: null,
      pagination: {
        size: 20,
        page: 1,
        totoal: 0,
        sort: null,
        order: null
      },
      serverList: []
    }
  },
  mounted() {
  },
  methods: {
    moment,
    getContainerStr(row) {
      return '容器类型：' + ContainerType[row.ContainerType].text + ' 容器内宿主机IP：' + row.HostIp
    },
    setVerNum(row) {
      this.serverId = row.Id
      this.title = '服务节点（' + row.ServerName + ')版本号配置'
      const num = this.formatVerNum(row.VerNum).split('.')
      this.verNum.One = num[0]
      this.verNum.Two = num[1]
      this.verNum.Three = num[2]
      this.setVisible = true
    },
    async saveVerNum() {
      this.setVisible = false
      const verNum = parseInt(this.verNum.One) * 10000 + parseInt(this.verNum.Two) * 100 + parseInt(this.verNum.Three)
      await serverApi.SetVerNum(this.serverId, verNum)
      this.loadTable()
    },
    showDetailed(row) {
      this.$router.push({
        name: 'serverDetailed',
        params: {
          routeTitle: row.ServerName + '-节点详情',
          Id: row.Id
        }
      })
    },
    closeEdit(isEdit) {
      this.visibleEdit = false
      if (isEdit) {
        this.reset()
      }
    },
    formatVerNum(num) {
      const str = num.toString()
      if (str.length <= 2) {
        return '000.00.' + str.padStart(2, '0')
      } else if (str.length <= 2) {
        return '000.' + str.substring(0, 2) + '.' + str.substring(2)
      } else {
        const len = str.length
        return str.substring(0, len - 4).padStart(3, '0') + '.' + str.substring(len - 4, len - 2) + '.' + str.substring(len - 2)
      }
    },
    reset() {
      this.pagination.page = 1
      this.loadTable()
    },
    editServer(row) {
      this.serverId = row.Id
      this.visibleEdit = true
    },
    add() {
      this.visibleAdd = true
    },
    deleteServer(row) {
      const that = this
      this.$confirm('确定删除(' + row.ServerName + ')服务节点?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        that.delete(row)
      }).catch(() => {
      })
    },
    enableServer(row) {
      this.setState(row, ServerState[0].value)
    },
    closeServer(row) {
      const that = this
      this.$confirm('确定下线(' + row.ServerName + ')服务节点?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        that.setState(row, ServerState[2].value)
      })
    },
    stopServer(row) {
      const that = this
      this.$confirm('确定停用(' + row.ServerName + ')服务节点?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        that.setState(row, ServerState[3].value)
      })
    },
    async setState(row, state) {
      await serverApi.SetState(row.Id, state)
      this.$message({
        message: ServerState[state].text + '成功！',
        type: 'success'
      })
      this.loadTable()
    },
    async delete(row) {
      await serverApi.Delete(row.Id)
      this.$message({
        message: '删除成功！',
        type: 'success'
      })
      this.loadTable()
    },
    pagingChange(index) {
      this.pagination.page = index
      this.loadTable()
    },
    sortChange(e) {
      this.pagination.order = e.order
      this.pagination.sort = e.prop
      this.loadTable()
    },
    indexMethod(index) {
      return (index + 1) + ((this.pagination.page - 1) * this.pagination.size)
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
      const res = await serverApi.Query(this.queryParam, this.pagination)
      this.serverList = res.List
      this.pagination.totoal = res.Count
    }
  }
}
</script>
