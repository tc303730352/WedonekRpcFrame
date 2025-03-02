<template>
  <div>
    <el-dialog
      title="资源列表"
      :visible="visible"
      width="85%"
      :before-close="handleClose"
    >
      <span slot="header"> 资源列表 </span>
      <el-form :inline="true" :model="queryParam">
        <el-form-item label="关键字">
          <el-input v-model="queryParam.QueryKey" placeholder="资源路径" />
        </el-form-item>
        <el-form-item label="资源状态">
          <el-select
            v-model="queryParam.ResourceState"
            clearable
            placeholder="资源状态"
          >
            <el-option label="正常" value="1" />
            <el-option label="失效" value="2" />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="search">查询</el-button>
          <el-button type="default" @click="reset">重置</el-button>
        </el-form-item>
      </el-form>
      <el-card>
        <el-table
          :data="resourceList"
          style="width: 100%"
          @sort-change="sortChange"
        >
          <el-table-column type="index" fixed="left" :index="indexMethod" />
          <el-table-column
            prop="ResourcePath"
            label="相对路径"
            sortable="custom"
            min-width="150"
          />
          <el-table-column prop="FullPath" label="方法/完整URL" min-width="320" />
          <el-table-column
            prop="ResourceShow"
            label="备注说明"
            min-width="150"
          />
          <el-table-column prop="ResourceState" sortable="custom" label="资源状态" min-width="80">
            <template slot-scope="scope">
              <span v-if="scope.row.ResourceState ==1">正常</span>
              <span v-else-if="scope.row.ResourceState ==2">失效</span>
            </template>
          </el-table-column>
          <el-table-column
            prop="VerNumStr"
            sortable="custom"
            label="版本号"
            min-width="80"
          />
          <el-table-column
            prop="IsShield"
            label="屏蔽状态"
            min-width="120"
          >
            <template slot-scope="scope">
              <template v-if="scope.row.IsShield">
                已屏蔽
                <span v-if="scope.row.ShieldEndTime">{{ formatTime(scope.row.ShieldEndTime) }}</span>
              </template>
              <template v-else>未屏蔽</template>
            </template>
          </el-table-column>
          <el-table-column
            prop="LastTime"
            label="最后更新时间"
            sortable="custom"
            min-width="150"
          >
            <template slot-scope="scope">
              <span>{{
                moment(scope.row.LastTime).format("YYYY-MM-DD HH:mm")
              }}</span>
            </template>
          </el-table-column>
          <el-table-column
            prop="Action"
            fixed="right"
            label="操作"
            width="100"
          >
            <template slot-scope="scope">
              <el-button-group>
                <el-button v-if="scope.row.IsShield == false" size="small" type="danger" @click="addShield(scope.row)">屏蔽</el-button>
                <el-button v-else size="small" type="primary" @click="cancelShield(scope.row)">取消屏蔽</el-button>
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
    </el-dialog>
    <addRescourceShield :resource-id="resourceId" :visible="visibleShield" @cancel="shieIdClose" />
  </div>
</template>
<script>
import moment from 'moment'
import * as ResourceApi from '@/api/module/resource'
import addRescourceShield from '../shield/addRescourceShield'
import { CancelResourceShieId } from '@/api/module/resourceShieId'
export default {
  components: {
    addRescourceShield
  },
  props: {
    modularId: {
      type: String,
      default: null
    },
    visible: {
      type: Boolean,
      required: true,
      default: false
    }
  },
  data() {
    return {
      resourceId: null,
      visibleShield: false,
      pagination: {
        size: 20,
        page: 1,
        total: 0,
        sort: null,
        order: null
      },
      resourceList: [],
      rpcMer: [],
      queryParam: {
        QueryKey: null,
        ModularId: null,
        ResourceState: null
      }
    }
  },
  watch: {
    visible: {
      handler(val) {
        if (val) {
          this.reset()
        }
      },
      immediate: true
    }
  },
  mounted() {
  },
  methods: {
    moment,
    shieIdClose(isRefresh) {
      this.visibleShield = false
      if (isRefresh) {
        this.loadTable()
      }
    },
    async cancelShield(row) {
      await CancelResourceShieId(row.Id)
      this.$message({
        message: '取消成功！',
        type: 'success'
      })
      this.loadTable()
    },
    handleClose() {
      this.$emit('cancel', false)
    },
    formatTime(time) {
      let num = Math.floor(time / 3600)
      let res = ''
      if (num > 0) {
        res = num + ':'
        time = time % 3600
      }
      num = Math.floor(time / 60)
      if (num > 0) {
        res = res + num + ':'
        num = num % 60
      }
      return res + num
    },
    addShield(row) {
      this.resourceId = row.Id
      this.visibleShield = true
    },
    reset() {
      this.pagination.page = 1
      this.queryParam.ModularId = this.modularId
      this.queryParam.ResourceState = null
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
      const res = await ResourceApi.QueryResource(this.queryParam, this.pagination)
      this.resourceList = res.List
      this.pagination.total = res.Count
    }
  }
}
</script>
