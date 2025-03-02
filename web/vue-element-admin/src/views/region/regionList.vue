<template>
  <el-card>
    <div slot="header" class="clearfix">
      <span style="line-height:40px">机房管理</span>
      <el-button style="float: right;" type="success" @click="addRegion">新增</el-button>
    </div>
    <el-table
      :data="regions"
      style="width: 100%"
      @sort-change="sortChange"
    >
      <el-table-column type="index" fixed="left" :index="indexMethod" />
      <el-table-column
        prop="RegionName"
        label="机房名"
        min-width="200"
      />
      <el-table-column
        prop="ProCity"
        label="省市"
        min-width="100"
      />
      <el-table-column
        prop="Address"
        label="地址"
        min-width="100"
      />
      <el-table-column
        prop="Contacts"
        label="联系人"
        min-width="100"
      />
      <el-table-column
        prop="Phone"
        label="联系电话"
        min-width="150"
      />
      <el-table-column
        prop="ServerNum"
        label="服务节点数"
        min-width="120"
      >
        <template slot-scope="scope">
          <el-link @click="openServer(scope.row)">{{ scope.row.ServerNum }}</el-link>
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
              @click="editRegion(scope.row)"
            >编辑</el-button>
            <el-button
              v-if="scope.row.ServerNum == 0"
              size="small"
              type="danger"
              @click="deleteRegion(scope.row)"
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
    <addRegion :visible="visibleAdd" @cancel="closeAdd" />
    <setRegion :id="id" :visible="visibleEdit" @cancel="closeEdit" />
  </el-card>
</template>
<script>
import moment from 'moment'
import * as regionApi from '@/api/basic/region'
import addRegion from './components/addRegion'
import setRegion from './components/setRegion'
export default {
  components: {
    addRegion,
    setRegion
  },
  data() {
    return {
      regions: [],
      visibleAdd: false,
      visibleEdit: false,
      id: null,
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
    this.loadTable()
  },
  methods: {
    moment,
    closeEdit(isRefresh) {
      this.visibleEdit = false
      if (isRefresh) {
        this.loadTable()
      }
    },
    editRegion(row) {
      this.id = row.Id
      this.visibleEdit = true
    },
    addRegion() {
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
          regionId: row.Id
        }
      })
    },
    deleteRegion(row) {
      const that = this
      this.$confirm('确定删除该机房?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        that.submitDelete(row)
      }).catch(() => {
      })
    },
    async submitDelete(row) {
      await regionApi.Delete(row.Id)
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
      const res = await regionApi.Query(this.pagination)
      this.regions = res.List
      this.pagination.total = res.Count
    }
  }
}
</script>
