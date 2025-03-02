<template>
  <el-dialog
    title="服务降级"
    :visible="visible"
    width="45%"
    :before-close="handleClose"
  >
    <el-form ref="form" :model="formData" label-width="150px">
      <el-form-item label="启用" prop="IsEnable">
        <el-switch v-model="formData.IsEnable" />
      </el-form-item>
      <template v-if="formData.IsEnable">
        <el-form-item label="触发限制错误数" prop="LimitNum">
          <el-input-number
            v-model="formData.LimitNum"
            :min="1"
          />
        </el-form-item>
        <el-form-item label="熔断错误数" prop="FusingErrorNum">
          <el-input-number
            v-model="formData.FusingErrorNum"
            :min="1"
          />
        </el-form-item>
        <el-form-item label="刷新统计数的时间(秒)" prop="RefreshTime">
          <el-input-number
            v-model="formData.RefreshTime"
            :min="1"
          />
        </el-form-item>
        <el-form-item label="最短融断时长" prop="BeginDuration">
          <el-input-number
            v-model="formData.BeginDuration"
            :min="1"
          />
        </el-form-item>
        <el-form-item label="最长熔断时长" prop="EndDuration">
          <el-input-number
            v-model="formData.EndDuration"
            :min="1"
          />
        </el-form-item>
      </template>
    </el-form>
    <el-row slot="footer" style="text-align:center;line-height:20px;">
      <el-button type="primary" @click="saveLimit">保存</el-button>
      <el-button type="default" @click="handleReset">重置</el-button>
    </el-row>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import * as reduceInRankApi from '@/api/rpcMer/reduceInRank'
export default {
  props: {
    rpcMerId: {
      type: String,
      default: null
    },
    serverId: {
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
      formData: {
        IsEnable: false,
        LimitNum: 0,
        FusingErrorNum: 0,
        RefreshTime: 0,
        BeginDuration: 0,
        EndDuration: 0
      },
      rules: {}
    }
  },
  watch: {
    visible: {
      handler(val) {
        if (val) {
          this.load()
        }
      },
      immediate: true
    }
  },
  mounted() {
  },
  methods: {
    moment,
    async load() {
      const res = await reduceInRankApi.Get(this.rpcMerId, this.serverId)
      if (res) {
        this.formData = res
      } else {
        this.formData = {
          IsEnable: false,
          LimitNum: 0,
          FusingErrorNum: 0,
          RefreshTime: 0,
          BeginDuration: 0,
          EndDuration: 0
        }
      }
    },
    handleClose() {
      this.$emit('cancel', false)
    },
    saveLimit() {
      const that = this
      this.$refs['form'].validate((valid) => {
        if (valid) {
          that.save()
        }
      })
    },
    async save() {
      await reduceInRankApi.Sync({
        RpcMerId: this.rpcMerId,
        ServerId: this.serverId,
        IsEnable: this.formData.IsEnable,
        FusingErrorNum: this.formData.FusingErrorNum,
        RefreshTime: this.formData.RefreshTime,
        BeginDuration: this.formData.BeginDuration,
        LimitNum: this.formData.LimitNum,
        EndDuration: this.formData.EndDuration
      })
      this.$message({
        message: '保存成功！',
        type: 'success'
      })
      this.$emit('cancel', true)
    },
    handleReset() {
      this.load()
    }
  }
}
</script>
