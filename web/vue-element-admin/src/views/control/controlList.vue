<template>
  <el-card>
    <div slot="header" class="clearfix">
      <span style="line-height:40px">服务中心</span>
      <el-button style="float: right;" type="success" @click="addControl">新增</el-button>
    </div>
    <el-table
      :data="controls"
      style="width: 100%"
      @sort-change="sortChange"
    >
      <el-table-column type="index" fixed="left" :index="indexMethod" />
      <el-table-column
        prop="ServerIp"
        label="服务节点IP"
        min-width="300"
      />
      <el-table-column
        prop="Port"
        label="端口"
        min-width="100"
      />
      <el-table-column
        prop="RegionName"
        label="所在机房"
        min-width="200"
      />
      <el-table-column
        prop="Show"
        label="说明"
        min-width="150"
      />
      <el-table-column
        prop="Action"
        fixed="right"
        label="操作"
        width="250"
      >
        <template slot-scope="scope">
          <el-button-group>
            <el-button
              size="small"
              type="primary"
              @click="editControl(scope.row)"
            >编辑</el-button>
            <el-button
              size="small"
              type="danger"
              style="margin-left: 10px;"
              @click="deleteControl(scope.row)"
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
    <addControl :visible="visibleAdd" @cancel="closeAdd" />
    <setControl :id="id" :visible="visibleEdit" @cancel="closeEdit" />
  </el-card>
</template>
<script>
import moment from 'moment'
import * as controlApi from '@/api/basic/control'
import addControl from './components/addControl'
import setControl from './components/editControl'
export default {
  components: {
    addControl,
    setControl
  },
  data() {
    return {
      controls: [],
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
    editControl(row) {
      this.id = row.Id
      this.visibleEdit = true
    },
    addControl() {
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
    deleteControl(row) {
      const that = this
      this.$confirm('确定删除该服务中心?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        that.submitDelete(row)
      }).catch(() => {
      })
    },
    async submitDelete(row) {
      await controlApi.Delete(row.Id)
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
      const res = await controlApi.Query(this.pagination)
      this.controls = res.List
      this.pagination.total = res.Count
    }
  }
}
</script>
