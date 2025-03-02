<template>
  <div>
    <el-row style="margin-top: 20px" :gutter="24" type="flex" justify="center">
      <el-col :xl="3" :sm="5">
        <el-card>
          <div slot="header" class="clearfix">
            <span style="line-height:40px">服务组别</span>
            <el-button style="float: right;" icon="el-icon-plus" type="text" @click="addGroup" />
          </div>
          <el-row v-for="(item) in groupType" :key="item.Id" class="groupTypeItem">
            <span class="title" @click="chooseGroupType(item.Id)">{{ item.GroupName + '(' + item.SystemTypeNum + ')' }}</span>
            <el-button-group>
              <el-link icon="el-icon-edit" @click="editGroup(item.Id)" />
              <el-link v-if="item.SystemTypeNum == 0" icon="el-icon-delete" @click="deleteGroup(item.Id)" />
            </el-button-group>
          </el-row>
        </el-card>
      </el-col>
      <el-col :xl="20" :sm="19">
        <el-card>
          <div slot="header" class="clearfix">
            <span style="line-height:40px">服务节点类型</span>
            <el-button style="float: right;" type="success" @click="addSystemType">新增</el-button>
          </div>
          <el-table
            :data="serverType"
            style="width: 100%"
            @sort-change="sortChange"
          >
            <el-table-column type="index" fixed="left" :index="indexMethod" />
            <el-table-column
              prop="SystemName"
              label="类型名"
              min-width="200"
            />
            <el-table-column prop="TypeVal" sortable="custom" label="类型值" min-width="200" />
            <el-table-column
              prop="GroupName"
              label="服务组别"
              min-width="200"
            />
            <el-table-column
              prop="DefPort"
              sortable="custom"
              label="默认端口"
              min-width="200"
            />
            <el-table-column
              prop="ServerNum"
              sortable="custom"
              label="服务节点数"
              min-width="120"
            >
              <template slot-scope="scope">
                <el-link @click="openServerType(scope.row)">{{ scope.row.ServerNum }}</el-link>
              </template>
            </el-table-column>
            <el-table-column prop="ServiceType" sortable="custom" label="节点类型" width="80">
              <template slot-scope="scope">
                <span>{{ ServiceType[scope.row.ServiceType].text }}</span>
              </template>
            </el-table-column>
            <el-table-column
              prop="Action"
              fixed="right"
              label="操作"
              width="150"
            >
              <template slot-scope="scope">
                <el-button-group>
                  <el-button
                    size="small"
                    type="primary"
                    @click="editSysType(scope.row)"
                  >编辑</el-button>
                  <el-button
                    v-if="scope.row.ServerNum == 0"
                    size="small"
                    type="danger"
                    @click="deleteSysType(scope.row)"
                  >删除</el-button>
                </el-button-group>
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
      </el-col>
    </el-row>
    <addServerGroup :visible="visible" @cancel="closeGroupAdd" />
    <editServerGroup :id="id" :visible="visibleEdit" @cancel="closeGroupEdit" />
    <addServerType :group-id="queryParam.GroupId" :visible="visibleTypeAdd" @cancel="closeTypeAdd" />
    <editServerType :id="typeId" :visible="visibleTypeEdit" @cancel="closeTypeEdit" />
  </div>
</template>
<script>
import moment from 'moment'
import * as groupApi from '@/api/basic/serverGroup'
import * as serverTypeApi from '@/api/basic/serverType'
import { ServiceType } from '@/config/publicDic'
import addServerGroup from './components/addServerGroup'
import editServerGroup from './components/editServerGroup'
import addServerType from './components/addServerType.vue'
import editServerType from './components/editServerType.vue'
export default {
  components: {
    addServerGroup,
    editServerGroup,
    addServerType,
    editServerType
  },
  data() {
    return {
      ServiceType,
      groupType: [],
      id: null,
      typeId: null,
      visible: false,
      visibleTypeAdd: false,
      visibleTypeEdit: false,
      visibleEdit: false,
      defaultProps: {
        children: 'children',
        label: 'label'
      },
      expandedKeys: [],
      queryParam: {
        GroupId: null
      },
      pagination: {
        size: 20,
        page: 1,
        total: 0,
        sort: null,
        order: null
      },
      serverType: []
    }
  },
  mounted() {
    this.loadGroupType()
    this.pagination.page = 1
    this.loadTable()
  },
  methods: {
    moment,
    addSystemType() {
      this.visibleTypeAdd = true
    },
    deleteSysType(row) {
      const that = this
      this.$confirm('确定删除该服务节点类型?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        that.submitDeleteSysType(row)
      }).catch(() => {
      })
    },
    async submitDeleteSysType(row) {
      await serverTypeApi.Delete(row.Id)
      this.$message({
        message: '删除成功！',
        type: 'success'
      })
      this.loadTable()
    },
    closeGroupEdit(isRefresh) {
      this.visibleEdit = false
      if (isRefresh) {
        this.loadGroupType()
      }
    },
    closeTypeAdd(isRefresh) {
      this.visibleTypeAdd = false
      if (isRefresh) {
        this.loadTable()
      }
    },
    closeTypeEdit(isRefresh) {
      this.visibleTypeEdit = false
      if (isRefresh) {
        this.loadTable()
      }
    },
    editSysType(row) {
      this.typeId = row.Id
      this.visibleTypeEdit = true
    },
    closeGroupAdd(isRefresh) {
      this.visible = false
      if (isRefresh) {
        this.loadGroupType()
      }
    },
    editGroup(id) {
      this.id = id
      this.visibleEdit = true
    },
    openServerType(row) {
      // serverList
      this.$router.push({
        name: 'serverList',
        params: {
          systemTypeId: row.Id
        }
      })
    },
    deleteGroup(id) {
      const that = this
      this.$confirm('确定删除该服务组别?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        that.submitDeleteGroup(id)
      }).catch(() => {
      })
    },
    async submitDeleteGroup(id) {
      await groupApi.Delete(id)
      this.$message({
        message: '删除成功！',
        type: 'success'
      })
      this.loadGroupType()
    },
    addGroup() {
      this.visible = true
    },
    async loadGroupType() {
      this.groupType = await groupApi.GetAll()
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
      return index + 1 + (this.pagination.page - 1) * this.pagination.size
    },
    async loadTable() {
      const res = await serverTypeApi.Query(this.queryParam, this.pagination)
      this.serverType = res.List
      this.pagination.total = res.Count
    },
    chooseGroupType(id) {
      this.queryParam.GroupId = id
      this.loadTable()
    }
  }
}
</script>
<style scoped>
.groupTypeItem {
    width: 100%;
    line-height: 40px;
    height: 40px;
    .title {
        cursor: pointer;
    };
    .el-button-group {
        float:right;
        line-height: 40px;
        height: 40px;
        .el-link {
            margin-right: 5px;
        }
    }
}
</style>
