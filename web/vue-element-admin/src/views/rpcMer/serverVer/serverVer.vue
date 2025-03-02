<template>
  <div>
    <el-row style="width: 96%; margin-left: 2%;margin-top: 20px;">
      <el-table
        :data="vers"
        :border="true"
        style="width: 100%;"
      >
        <el-table-column prop="TypeName" align="right" header-align="center" label="节点类型" width="300" />
        <el-table-column prop="CurrentVer" align="center" label="当前版本号" width="300" />
        <el-table-column prop="LastVerNum" align="center" label="最新版本号" width="300">
          <template slot-scope="scope">
            <el-select v-if="scope.row.LastVerNum.length > 0" v-model="scope.row.VerNum" style="width: 100%;">
              <el-option v-for="(item) in scope.row.LastVerNum" :key="item">{{ item }}</el-option>
            </el-select>
          </template>
        </el-table-column>
        <el-table-column prop="action" label="操作" min-width="150">
          <template slot-scope="scope">
            <el-button v-if="scope.row.LastVerNum.length > 0" type="primary" @click="save(scope.row)">设为正式版本</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-row>
  </div>
</template>
<script>
import moment from 'moment'
import { GetVerList, SetCurrentVer } from '@/api/rpcMer/serverVer'
export default {
  components: {},
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
      vers: []
    }
  },
  watch: {
    isLoad: {
      handler(val) {
        if (val) {
          this.load()
        }
      },
      immediate: true
    }
  },
  mounted() {},
  methods: {
    moment,
    async save(row) {
      if (row.VerNum == null) {
        this.$message({
          message: '版本号不能为空',
          type: 'error'
        })
        return
      }
      await SetCurrentVer({
        RpcMerId: this.rpcMerId,
        SystemTypeId: row.SystemTypeId,
        VerNum: parseInt(row.VerNum.replace('.', '').replace('.', ''))
      })
      this.$message({
        message: '保存成功!',
        type: 'success'
      })
    },
    async load() {
      const res = await GetVerList(this.rpcMerId)
      res.forEach(c => {
        c.VerNum = c.LastVerNum.length === 0 ? null : c.LastVerNum[0]
      })
      this.vers = res
    }
  }
}
</script>
