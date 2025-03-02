<template>
  <el-card>
    <span slot="header">
      容器组管理
    </span>
    <el-form :inline="true" :model="queryParam">
      <el-form-item label="关键字">
        <el-input v-model="queryParam.QueryKey" placeholder="名称" />
      </el-form-item>
      <el-form-item label="容器类型">
        <el-select v-model="queryParam.ContainerType" placeholder="容器类型">
          <el-option v-for="item in ContainerType" :key="item.value" :label="item.text" :value="item.value" />
        </el-select>
      </el-form-item>
      <el-form-item label="所在机房">
        <el-select v-model="queryParam.RegionId" clearable placeholder="所在机房">
          <el-option v-for="item in region" :key="item.Id" :label="item.RegionName" :value="item.Id" />
        </el-select>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="loadTable">查询</el-button>
        <el-button type="default" @click="reset">重置</el-button>
        <el-button type="success" @click="addGroup">新增</el-button>
      </el-form-item>
    </el-form>
    <el-table
      :data="containerGroup"
      style="width: 100%"
      @sort-change="sortChange"
    >
      <el-table-column type="index" fixed="left" :index="indexMethod" />
      <el-table-column
        prop="Name"
        label="容器组名"
        min-width="200"
      />
      <el-table-column
        prop="RegionName"
        label="机房名"
        min-width="200"
      />
      <el-table-column
        prop="ContainerType"
        label="容器类型"
        min-width="100"
      >
        <template slot-scope="scope">
          {{ ContainerType[scope.row.ContainerType].text }}
        </template>
      </el-table-column>
      <el-table-column
        prop="HostMac"
        label="宿主MAC"
        min-width="150"
      />
      <el-table-column
        prop="ServerNum"
        label="服务节点数"
        align="center"
        min-width="120"
      >
        <template slot-scope="scope">
          <el-link @click="openServer(scope.row)">{{ scope.row.ServerNum }}</el-link>
        </template>
      </el-table-column>
      <el-table-column
        prop="Remark"
        label="备注"
        min-width="100"
      />
      <el-table-column
        prop="CreateTime"
        label="创建时间"
        min-width="150"
      >
        <template slot-scope="scope">
          {{ moment(scope.row.CreateTime).format("YYYY-MM-DD HH:mm:ss") }}
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
              @click="editGroup(scope.row)"
            >编辑</el-button>
            <el-button
              v-if="scope.row.ServerNum == 0"
              size="small"
              type="danger"
              @click="deleteGroup(scope.row)"
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
    <addContainerGroup :visible="visibleAdd" @cancel="closeAdd" />
    <editContainerGroup :id="id" :visible="visibleEdit" @cancel="closeEdit" />
  </el-card>
</template>
<script>
import moment from 'moment'
import * as containerGroupApi from '@/api/basic/containerGroup'
import { ContainerType } from '@/config/publicDic'
import { GetList } from '@/api/basic/region'
import addContainerGroup from './components/addContainerGroup'
import editContainerGroup from './components/editContainerGroup'
export default {
  components: {
    addContainerGroup,
    editContainerGroup
  },
  data() {
    return {
      ContainerType,
      containerGroup: [],
      visibleAdd: false,
      visibleEdit: false,
      id: null,
      region: [],
      queryParam: {
        QueryKey: null,
        RegionId: null,
        ContainerType: null
      },
      pagination: {
        size: 20,
        page: 1,
        total: 0,
        sort: null,
        order: null
      }
    }
  },
  mounted() {
    this.pagination.page = 1
    this.loadRegion()
    this.loadTable()
  },
  methods: {
    moment,
    reset() {
      this.pagination.page = 1
      this.queryParam = {
        QueryKey: null,
        RegionId: null,
        ContainerType: null
      }
      this.loadTable()
    },
    closeEdit(isRefresh) {
      this.visibleEdit = false
      if (isRefresh) {
        this.loadTable()
      }
    },
    async loadRegion() {
      this.region = await GetList()
    },
    editGroup(row) {
      this.id = row.Id
      this.visibleEdit = true
    },
    addGroup() {
      this.visibleAdd = true
    },
    closeAdd(isRefresh) {
      this.visibleAdd = false
      if (isRefresh) {
        this.loadTable()
      }
    },
    openServer(row) {
      // serverList
      this.$router.push({
        name: 'serverList',
        params: {
          containerGroup: row.Id
        }
      })
    },
    deleteGroup(row) {
      const that = this
      this.$confirm('确定删除该容器组?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        that.submitDelete(row)
      }).catch(() => {
      })
    },
    async submitDelete(row) {
      await containerGroupApi.Delete(row.Id)
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
      return index + 1 + (this.pagination.page - 1) * this.pagination.size
    },
    async loadTable() {
      const res = await containerGroupApi.Query(this.queryParam, this.pagination)
      this.containerGroup = res.List
      this.pagination.total = res.Count
    }
  }
}
</script>
